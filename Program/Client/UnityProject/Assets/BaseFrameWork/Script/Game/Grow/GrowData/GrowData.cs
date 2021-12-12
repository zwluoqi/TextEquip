using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Script.Game.Grow.GrowAPI;
using TextEquip.System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using XZXD.UI;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.Grow
{
    public class GrowData:BaseGrowData
    {
        public long randomSeed;
        public GrowPlayer growPlayer = new GrowPlayer();
        public List<GrowEquip> growEquips = new List<GrowEquip>();
        public List<GrowEquip> lostEquips = new List<GrowEquip>();
        public Dictionary<int,GrowPlayerProp> kvgrowPlayers = new Dictionary<int,GrowPlayerProp>();
        public bool[] islandOpen = new bool[64];
        public bool[] lvCopyOpenMud = new bool[1024];
        public bool[] storyTips = new bool[1024];

        //E
        public bool[] autoCostEquips = new bool[16];

        
        private Dictionary<long, GrowEquip> kvEquips = new Dictionary<long, GrowEquip>();


        public void CheckForNewData()
        {
            //扩展功能
            islandOpen[0] = true;
            if (growPlayer == null)
            {
                growPlayer = new GrowPlayer();
                growPlayer.playerLv = 1;
                growPlayer.chongshengCount = 0;
            }
            growPlayer.CheckForNewData();
        }

        public void BuildData()
        {
            for (int i=0;i<growEquips.Count;)
            {
                var equip = growEquips[i];
                var equipModel = DictDataManager.Instance.dictEquipEquip.GetModel(equip.equipId);
                if (equipModel == null)
                {
                    growEquips.RemoveAt(i);  
                    lostEquips.Add(equip);
                }
                else
                {
                    if (kvEquips.ContainsKey(equip.guid))
                    {
                        Debug.LogError("重复equip:"+equip.guid);
                        growEquips.RemoveAt(i);
                        lostEquips.Add(equip);
                    }
                    else
                    {
                        if (growPlayer.HasEquipEquip(equip.guid))
                        {
                            Debug.LogError("重复equip:"+equip.guid);
                            growEquips.RemoveAt(i);
                            lostEquips.Add(equip);
                        }
                        else
                        {
                            i++;
                            kvEquips.Add(equip.guid, equip);
                            equip.BuildData();
                        }
                    }
                }
            }


            growPlayer.BuildData();
        }

        public GrowEquip CreateNewEquip(DictEquipEquipTypeEnum equipTypeEnum,int qulity, int lv)
        {
            GrowEquip weapon = GrowEquipAPI.CreateNewEquip(GrowFun.Instance.randomUtil, equipTypeEnum, qulity, lv);
            AddEquip(weapon);
            return weapon;
        }
        
        public void AddEquip(GrowEquip weapon)
        {
            weapon.guid = DataBaseSystem.GetUniqueId();
            growEquips.Add(weapon);
            kvEquips.Add(weapon.guid,weapon);

            var equipModel = GrowEquipAPI.GetEquipModel(weapon);
            var equipName = RichTextUtil.AddColor(equipModel.name, weapon.qulity);
            var info = string.Format("获得了{0}Lv.{1}",equipName, weapon.lev);
            NotificationCenter.Default.PostNotification((int) GameMessageId.SystemLogId,
                info);
#if UNITY_EDITOR
            weapon.PrintInfo();
#endif
        }
        
        
        public void RemoveEquip(GrowEquip weapon)
        {
            growEquips.Remove(weapon);
            kvEquips.Remove(weapon.guid);
            var equipModel = GrowEquipAPI.GetEquipModel(weapon);
            var equipName = RichTextUtil.AddColor(equipModel.name, weapon.qulity);
            var info = string.Format("失去了{0}Lv.{1}",equipName, weapon.lev);
            NotificationCenter.Default.PostNotification((int) GameMessageId.SystemLogId,
                info);
        }

        public void AddProp(DictPlayerPropEnum gold, long goldObtainRatio)
        {
            if (!kvgrowPlayers.ContainsKey((int) gold))
            {
                var item  = new GrowPlayerProp();
                item.type = gold;
                kvgrowPlayers[(int) gold] = item;
                item.val += goldObtainRatio;
            }
            else
            {
                kvgrowPlayers[(int) gold].val += goldObtainRatio;
            }
            NotificationCenter.Default.PostNotification((int)GameMessageId.FreshAttributeUI);
        }
        
        public void AddProp(string gold, long goldObtainRatio)
        {
            for (int i=0;i<DictPlayerPropEnumString.vlas.Length;i++)
            {
                if (DictPlayerPropEnumString.vlas[i] == gold)
                {
                    AddProp((DictPlayerPropEnum)(i+DictPlayerPropEnum.coin), goldObtainRatio);
                }
            }

        }

        public long GetPropByID(DictPlayerPropEnum gold)
        {
            if (kvgrowPlayers.ContainsKey((int) gold))
            {
                return kvgrowPlayers[(int) gold].val;
            }
            else
            {
                return 0;
            }
        }

        public void LockEquip(GrowEquip equip,bool locked)
        {
            equip.locked = locked;
        }
        
        public void LockEquip(long equip_guid,bool locked)
        {
            var equip = GetEquipByGuid(equip_guid);
            LockEquip(equip,locked);
        }
        
        /// <summary>
        /// 出售
        /// </summary>
        public void CostEquips()
        {
            List<GrowEquip> tmps = new List<GrowEquip>();
            tmps.AddRange(growEquips);
            foreach (var growequip in tmps)
            {
                CostEquip(growequip,false);
            }
            GrowFun.Instance.SaveData();
        }
        
        public void SortEquips()
        {
            growEquips.Sort(OnSort);
        }

        private int OnSort(GrowEquip x, GrowEquip y)
        {
            if (x.lev != y.lev)
            {
                return -x.lev.CompareTo(y.lev);
            }else if (x.qulity != y.qulity)
            {
                return -x.qulity.CompareTo(y.qulity);
            }
            // else if (x.enchantlvl != y.enchantlvl)
            // {
            //     return -x.enchantlvl.CompareTo(y.enchantlvl);
            // }
            else
            {
                return x.guid.CompareTo(y.guid);
            }
        }

        /// <summary>
        /// 自动出售所有垃圾装备
        /// </summary>
        public void AutoCostEquips()
        {
            List<GrowEquip> tmps = new List<GrowEquip>();
            tmps.AddRange(growEquips);
            foreach (var growequip in tmps)
            {
                CostEquip(growequip,true);
            }
            GrowFun.Instance.SaveData();
        }

        /// <summary>
        /// 出售
        /// </summary>
        /// <param name="growequip"></param>
        public void CostEquip(GrowEquip growequip,bool autoCost)
        {
            if (growequip.locked)
            {
                BoxManager.CreatePopTis(growequip.GetEquipName()+"已锁定，请解锁后再出售");
                return;
            }

            if (!string.IsNullOrEmpty(growequip.authorName))
            {
                BoxManager.CreatePopTis(growequip.GetEquipName()+"由"+growequip.authorName+"制作,无法直接出售,只能上架网络商城处理");
                return;
            }

            if (autoCost)
            {
                if (!autoCostEquips[growequip.qulity - 10])
                {
                    return;
                }
            }

            growEquips.Remove(growequip);
            kvEquips.Remove(growequip.guid);
            var gold = growequip.GetCostCoin();
            
            AddProp(DictPlayerPropEnum.coin,gold);
            SystemlogCtrl.PostSystemLog((autoCost?"自动出售":"出售")+growequip.GetEquipName()+",获得金币"+gold);
        }

        public void CostEquip(long equip_guid, bool autoCost)
        {
            var equip = GetEquipByGuid(equip_guid);
            CostEquip(equip,autoCost);
        }

        public GrowEquip GetRandomEquip()
        {
            if (growEquips.Count > 0)
            {
                int index = GrowFun.Instance.randomUtil.Range(0, growEquips.Count);
                return growEquips[index];
            }
            {
                return null;
            }
        }

        /// <summary>
        /// 装备
        /// </summary>
        /// <param name="growEquip"></param>
        public void PlayerEquip(GrowEquip growEquip)
        {
            if (growPlayer.HasEquipEquip(growEquip))
            {
                BoxManager.CreatePopTis("装备已经穿戴，无需再次穿戴");
                // SystemlogCtrl.PostSystemLog("装备已经穿戴，无需再次穿戴");
                return;
            }
            var source = growPlayer.equipEquips[growEquip.GetModel().equip_type_int];
            if (kvEquips.ContainsKey(source.guid))
            {
                BoxManager.CreatePopTis("该装备存在异常数据，只能进行售卖处理");
                // SystemlogCtrl.PostSystemLog("该装备存在异常数据，只能进行售卖处理");
                return;
            }
            growPlayer.equipEquips[growEquip.GetModel().equip_type_int] = growEquip;
            if (source != null)
            {
                growEquips.Add(source);
                kvEquips.Add(source.guid,source);
            }
            growEquips.Remove(growEquip);
            kvEquips.Remove(growEquip.guid);

            growPlayer.UpdateAttribute();
            GrowFun.Instance.SaveData();
            NotificationCenter.Default.PostNotification((int)GameMessageId.PlayerEquipEquip);
        }

        public void PlayerEquip(long guid)
        {
            var equip = GetEquipByGuid(guid);
            PlayerEquip(equip);
        }
        
        
        /// <summary>
        /// 重生
        /// </summary>
        public void ReincarnationConfirm() {
            if (growPlayer.playerLv < 30)
            {
                SystemlogCtrl.PostSystemLog("等级这么低就先别转了吧，超过lv:30再来看看");
            }

            int point = CaculateWillGetreincarnationPoint();

            BoxManager.OpenYesAndNoPage("你将获得" + point + "转生点数，同时你的金币和装备都会消失。", delegate(bool yes)
            {
                if (yes)
                {
                    growPlayer.chongshengCount += 1;
                    growPlayer.chongshengPoint += point;
                    this.ChongSheng();
                    GrowFun.Instance.SaveData();
                    NotificationCenter.Default.PostNotification((int)GameMessageId.ChongSheng);
                }
            });
        }

        private void ChongSheng()
        {
            kvgrowPlayers.Clear();
            growEquips.Clear();
            growPlayer.ChongSheng();
        }

        public int CaculateWillGetreincarnationPoint()
        {
            var lv = growPlayer.playerLv;
            var lvPoint = lv >= 20 ? Math.Floor(Math.Pow(lv - 20, 1.1) / 2.1) : 0;

            var equipPoint = growPlayer.GetEquipPoint();

            return (int)((lvPoint + equipPoint) * 1.2);
        }
        public GrowEquip GetEquipByGuid(long guid)
        {
            GrowEquip equip = null;
            if (kvEquips.TryGetValue(guid, out equip))
            {
                return equip;
            }

            return growPlayer.GetEquipByGuid(guid);
        }

        /// <summary>
        /// 强化
        /// </summary>
        /// <param name="equip"></param>
        public bool StartStreng(GrowEquip equip,bool auto,int targetLev,bool use_back,int use_luck_lev)
        {
            if (equip.StartStreng(equip.GetModel().equip_type_int,auto, targetLev, use_back, use_luck_lev))
            {
                if (this.growPlayer.HasEquipEquip(equip))
                {
                    this.growPlayer.UpdateAttribute();
                }

                GrowFun.Instance.SaveData();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool StartStreng(long equip_guid,bool auto,int targetLev,bool use_back,int use_luck_lev)
        {
            var equip = this.GetEquipByGuid(equip_guid);
            return StartStreng(equip,auto,targetLev,use_back, use_luck_lev);
        }

        public void RecastTheEquiment(GrowEquip equip,int index)
        {
            equip.RecastTheEquiment(index);
            if (this.growPlayer.HasEquipEquip(equip))
            {
                this.growPlayer.UpdateAttribute();
            }
            GrowFun.Instance.SaveData();
        }


        public void SetAutoCostEquip(int qulity,bool autoCost)
        {
            this.autoCostEquips[qulity - 10] = autoCost;
            GrowFun.Instance.SaveData();
        }
        
        public bool GetAutoCostEquip(int qulity)
        {
            return this.autoCostEquips[qulity - 10];
        }

        public void PrintPlayerEquip()
        {
            growPlayer.PrintPlayerEquip();
        }
        

        public void PrintAllEquip()
        {
            foreach (var equip in growEquips)
            {
                equip.PrintInfo();
            }
        }


        public bool HasOpenIsland(int i)
        {
            return islandOpen[i];
        }

        public bool HasLvCopyCanMud(int i)
        {
            return lvCopyOpenMud[i];
        }
    }
}