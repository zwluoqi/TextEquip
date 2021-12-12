using System;
using AssetPlugin;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using Script.Game.System;
using UnityEngine;
using XZXD.UI;

namespace Script
{
    public class Main : MonoBehaviour
    {

        private void Awake()
        {
            Application.runInBackground = true;
            NotificationCenter.Default.AddObserver(this,OnGameMessage,(int)GameMessageId.SystemLogId);
            AssetFileToolUtilManager.Instance.txt.InitInnerSetting("Dict/md5.txt");
            DictDataManager.Instance.Init("Dict",null);
            Debug.LogWarning(Application.persistentDataPath);
        }

        public void Start()
        {
            UIPageManager.Instance.OpenPage("SimpleLoginPage", "");
        }

        private void OnGameMessage(Notification notification)
        {
            Debug.Log(notification.info);
        }

        public string commond = "";
        private TimeEventHandler _timeEventHandler;
#if UNITY_EDITOR
        private void OnGUI()
        {
            commond = GUI.TextField(new Rect(20, 20, 200, 50), commond);
            if (GUI.Button(new Rect(20, 80, 100, 50), "OK"))
            {
                var commonds = commond.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                switch (commonds[0])
                {
                    case "print_player_equip":
                        GrowFun.Instance.growData.PrintPlayerEquip();
                        break;
                    case "print_all_equip":
                        GrowFun.Instance.growData.PrintAllEquip();
                        break;
                    case "streng":
                    {
                        var index = int.Parse(commonds[1]);
                        var equip = GrowFun.Instance.growData.growPlayer.equipEquips[index];
                        GrowFun.Instance.growData.StartStreng(equip,true,99,false,0);
                        equip.PrintInfo();
                    }
                        break;
                    case "auto_streng":
                    {
                        var index = int.Parse(commonds[1]);
                        var equip = GrowFun.Instance.growData.growPlayer.equipEquips[index];
                        _timeEventHandler = GameSystem.Instance.timeeventManager.CreateEvent(delegate
                        {
                            if (!GrowFun.Instance.growData.StartStreng(equip, true, 20,true,0))
                            {
                                TimeEventManager.Delete(ref _timeEventHandler);
                                equip.PrintInfo();    
                            }
                            
                        }, 0, 1);
                    }
                        break;
                    case "recast":
                    {
                        var index = int.Parse(commonds[1]);
                        var castIndex = int.Parse(commonds[2]);
                        var equip = GrowFun.Instance.growData.growPlayer.equipEquips[index];
                        GrowFun.Instance.growData.RecastTheEquiment(equip,castIndex);
                        equip.PrintInfo();
                    }
                        break;;
                    case "equip":
                    {
                        var newguid = long.Parse(commonds[1]);
                        var equip = GrowFun.Instance.growData.GetEquipByGuid(newguid);
                        GrowFun.Instance.growData.PlayerEquip(equip);
                    }
                        break;
                    case "set_auto_cost":
                        var qulity = int.Parse(commonds[1]);
                        var autocost = int.Parse(commonds[2]);
                        GrowFun.Instance.growData.SetAutoCostEquip(qulity,autocost != 0);
                        break;;
                    case "cs_shop_items":
                        ShopUtil.RequestShopItems(0,false);
                        break;
                    case "cs_pop_shop_item":
                    {
                        var equip = GrowFun.Instance.growData.growEquips[0];
                        ShopUtil.PopNetItem(equip,"author","name","desc",1);
                    }
                        break;
                    
                    case "cs_buy_shop_item":
                    {
                        var shopData =  ShopUtil.GetShopData(0);
                        ShopUtil.BuyShopItem(shopData.equips[0]);
                    }
                        break;
                    case "add_coin":
                        GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin, 999999999);
                        break;
                    case "auto_cost_coin":
                        GrowFun.Instance.growData.AutoCostEquips();
                        break;;
                        case "add_equip":
                        {
                            var qulityIndex = GrowFun.Instance.randomUtil.Range(10, 15);
                            var equipType = GrowFun.Instance.randomUtil.Range(0, 4);
                            GrowFun.Instance.growData.CreateNewEquip((DictEquipEquipTypeEnum)equipType, qulityIndex, GrowFun.Instance.growData.growPlayer.playerLv);
                            GrowFun.Instance.SaveData();
                        }
                            break;
                        case "fresh_world":
                            GameSystem.Instance.FreshWorld();
                            break;
                        case "start_tile_copy":
                            GameSystem.Instance.currentWorld.StartCopy(0, false);
                            break;

                }
            }
        }
        #endif

        public void Update()
        {
            GameSystem.Instance.Tick();
            NetManager.Instance.NetTick();

            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     GameSystem.Instance.FreshWorld();
            // }
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameSystem.ClearRecordAndStart();
            }
            #endif

            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     var qulity = GrowFun.Instance.randomUtil.Range(10, 15);
            //     var equipType = GrowFun.Instance.randomUtil.Range(0, 4);
            //     GrowFun.Instance.growData.CreateNewEquip((DictEquipEquipTypeEnum)equipType, qulity, GrowFun.Instance.growData.growPlayer.playerLv);
            //     GrowFun.Instance.SaveData();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.W))
            // {
            //     GrowFun.Instance.growData.AutoCostEquips();
            // }
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     var growEquip = GrowFun.Instance.growData.GetRandomEquip(); 
            //     var source = GrowFun.Instance.growData.growPlayer.equipEquips[growEquip.GetModel().equip_type_int];
            //     if (source.lev < growEquip.lev )
            //     {
            //         Debug.LogWarning(string.Format("变更装备{0} {1}",source.GetEquipName(),growEquip.GetEquipName()));
            //         GrowFun.Instance.growData.PlayerEquip(growEquip);
            //     }else if (source.lev ==growEquip.lev )
            //     {
            //         if (source.qulity < growEquip.qulity)
            //         {
            //             Debug.LogWarning(
            //                 string.Format("变更装备{0} {1}", source.GetEquipName(), growEquip.GetEquipName()));
            //             GrowFun.Instance.growData.PlayerEquip(growEquip);
            //         }
            //     }
            // }
            //
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     GrowFun.Instance.growData.ReincarnationConfirm();
            // }
            //
            //
            // if (Input.GetKeyDown(KeyCode.X))
            // {
            //     var indexEquip = GrowFun.Instance.randomUtil.Range(0, 4);
            //     var equip = GrowFun.Instance.growData.growPlayer.equipEquips[indexEquip];
            //     if (equip.extraItems.Count > 0)
            //     {
            //         // var index = GrowFun.Instance.randomUtil.Range(0, equip.extraItems.Count);
            //         var index = equip.GetLowestExtraIndex();
            //         GrowFun.Instance.growData.RecastTheEquiment(equip,index);
            //     }
            // }

            // if (Input.GetKeyDown(KeyCode.C))
            // {
            //     GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin, 999999999);
            // }
        }

    }
}