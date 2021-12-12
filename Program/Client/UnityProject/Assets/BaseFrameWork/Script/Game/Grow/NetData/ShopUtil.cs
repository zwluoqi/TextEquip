using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices.ComTypes;
using NetWork.Layer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Script.Game.Grow.GrowAPI;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.Grow.NetData
{
    public class ShopData
    {
        public List<GrowEquip> equips = new List<GrowEquip>();
        public List<CommonDrop> drops = new List<CommonDrop>();
        public int page;

        public void PrintInfo()
        {
            foreach (var equip in equips)
            {
                equip.PrintInfo();
            }
            foreach (var drop in drops)
            {
                Debug.LogWarning(drop.propId);
            }
        }
    }
    
    
    public class ShopUtil
    {
        public static Dictionary<int,ShopData> netData = new Dictionary<int,ShopData>();
        public static Dictionary<int,ShopData> localData = new Dictionary<int,ShopData>();
        public static void RequestShopItems(int page,bool forceFresh)
        {        
            if (GrowFun.Instance.shopItemFromNet)
            {
                if (forceFresh)
                {
                    CreateNetGrowEquips(page);

                }
                else
                {
                    if(netData.ContainsKey(page)){
                        NotificationCenter.Default.PostNotification((int) GameMessageId.SCGetShopItems);
                    }else
                    {
                        CreateNetGrowEquips(page);
                    }
                }
            }
            else
            {
                if (forceFresh)
                {
                    CreateLocalGrowEquips(page);
                }
                else
                {
                    if (localData.ContainsKey(page))
                    {
                        NotificationCenter.Default.PostNotification((int) GameMessageId.SCGetShopItems);
                    }
                    else
                    {
                        CreateLocalGrowEquips(page);
                    }
                }
            }
        }

        public static ShopData GetShopData(int page)
        {
            ShopData shopData;
            if (GrowFun.Instance.shopItemFromNet)
            {
                 netData.TryGetValue(page,out shopData);
            }
            else
            {
                localData.TryGetValue(page,out shopData);
            }

            return shopData;
        }

        static void CreateNetGrowEquips(int page){
                JObject jObject = new JObject();
                jObject["page"] = page;
                NetManager.Instance.SendHttp("cs_shop_items", jObject.ToString(), delegate(Packet data, bool success)
                {
                    if (success)
                    {
                        var shopData = JsonConvert.DeserializeObject<ShopData>(data.kBody);
                        netData[page] = shopData;
                        shopData.PrintInfo();
                        NotificationCenter.Default.PostNotification((int) GameMessageId.SCGetShopItems);
                    }
                });
        }

        private static void CreateLocalGrowEquips(int page)
        {
            ShopData shopData = new ShopData();
            shopData.page = page;
            for (int i = 0; i < 5; i++) {
                var lv = (int)Math.Floor(GrowFun.Instance.growData.growPlayer.playerLv + GrowFun.Instance.randomUtil.value * 3);
                //装备等级最高200
                // lv = lv > 200 ? 200 : lv
                var equip = CreateShopItem(lv);
                if (equip != null)
                {
                    shopData.equips.Add(equip);
                }
            }
            for (int i = 0; i < 5; i++) {
                CommonDrop drop = new CommonDrop();
                drop.propId = DictPlayerPropEnum.wucai_yello + i;
                shopData.drops.Add(drop);
            }
            localData[shopData.page] = shopData;
            shopData.PrintInfo();
                            NotificationCenter.Default.PostNotification((int) GameMessageId.SCGetShopItems);
        }

        private static GrowEquip CreateShopItem(int lv)
        {
            var equip = new double[]{0.4, 0.342, 0.25, 0.008};
            // var equip = [0.4, 0.30, 0.25, 0.05];
            // var equip = [0, 0, 0,1];
            var equipQua = -1;
            var r = GrowFun.Instance.randomUtil.value;
            if (r <= equip[0]) {
                // 获得普通装备
                equipQua = 11;
            } else if (r < equip[1] + equip[0] && r >= equip[0]) {
                // 获得神器装备
                equipQua = 12;
            } else if (
                r < equip[2] + equip[1] + equip[0] &&
                r >= equip[1] + equip[0]
            ) {
                // 获得史诗装备
                equipQua = 13;
            } else if (
                r < equip[3] + equip[2] + equip[1] + equip[0] &&
                r >= equip[2] + equip[1] + equip[0]
            ) {
                // 获得独特装备
                equipQua = 14;
            } else {
                // 未获得装备
            }
            if (equipQua != -1) {
                // this.createEquip(equipQua,lv)
                var index = GrowFun.Instance.randomUtil.Range(0, 4);
                var item = GrowEquipAPI.CreateNewEquip(GrowFun.Instance.randomUtil, (DictEquipEquipTypeEnum)index, equipQua, lv);
                return item;
            }
            else
            {
                return null;
            }
        }

        
        public static void BuyShopItem(GrowEquip buyItem)
        {
            if (GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.coin) < buyItem.GetBuyCostCoin())
            {
                BoxManager.CreatePopTis("金币不足，无法购买！");
                return;
            }
            if (GrowFun.Instance.shopItemFromNet)
            {
                BuyNetEquip(buyItem);
            }
            else
            {
                BuyLocalEquip(buyItem);
            }
        }

        private static void BuyNetEquip(GrowEquip buyItem)
        {
            JObject jObject = new JObject();
            jObject["guid"] = buyItem.guid;
            jObject["buyPlayerName"] = Grow.GrowFun.Instance.growData.growPlayer.playerName;
            NetManager.Instance.SendHttp("cs_buy_shop_item", jObject.ToString(), delegate(Packet data, bool success)
            {
                if (success)
                {
                    RemoveItem(netData,buyItem);
                    GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin,-buyItem.GetBuyCostCoin());
                    buyItem.inHeritList.Add(GrowFun.Instance.growData.growPlayer.playerName);
                    GrowFun.Instance.growData.AddEquip(buyItem);
                    GrowFun.Instance.SaveData();
                    NotificationCenter.Default.PostNotification((int) GameMessageId.SCBuyShopEquip);
                }
            });
        }



        private static void BuyLocalEquip(GrowEquip buyItem)
        {
            RemoveItem(localData,buyItem);
            GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin,-buyItem.GetBuyCostCoin());
            GrowFun.Instance.growData.AddEquip(buyItem);
            NotificationCenter.Default.PostNotification((int) GameMessageId.SCBuyShopEquip);
        }
        
        private static void RemoveItem(Dictionary<int, ShopData> shopDatas, GrowEquip buyItem)
        {
            foreach (var kv in shopDatas)
            {
                var items = kv.Value;
                items.equips.Remove(buyItem);
            }
        }

        public static GrowEquip GetEquipByGuid(long equipGuid)
        {
            if (GrowFun.Instance.shopItemFromNet)
            {
                return GetItem(netData, equipGuid);
            }
            else
            {
                return GetItem(localData, equipGuid);
            }
        }

        private static GrowEquip GetItem(Dictionary<int, ShopData> p0, long equipGuid)
        {
            foreach (var kv in p0)
            {
                foreach (var item in kv.Value.equips)
                {
                    if (item.guid == equipGuid)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public static void PopNetItem(GrowEquip buyItem,
            string authorName,
            string equipName,
            string equipDesc,
            long authorCostCoin)
        {
            JObject jObject = new JObject();
            jObject["pop_equip"] = Newtonsoft.Json.JsonConvert.SerializeObject(buyItem);
            jObject["authorName"] = authorName;
            jObject["equipName"] = equipName;
            jObject["equipDesc"] = equipDesc;
            jObject["authorCostCoin"] = authorCostCoin;
            NetManager.Instance.SendHttp("cs_pop_shop_item", jObject.ToString(), delegate(Packet data, bool success)
            {
                if (success)
                {
                    GrowFun.Instance.growData.RemoveEquip(buyItem);
                    GrowFun.Instance.SaveData();
                    NotificationCenter.Default.PostNotification((int) GameMessageId.SCPopShopEquip);
                }
            });
        }
    }
}