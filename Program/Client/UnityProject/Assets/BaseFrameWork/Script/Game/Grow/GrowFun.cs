using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using TMPro;

using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.Grow
{
    public class GrowFun
    {
        private static GrowFun _instance;
        public static GrowFun Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GrowFun();
                }
                return _instance;
            }
        }

        public GrowData growData;
        public RandomUtil randomUtil;
        /// <summary>
        /// 远程ID 目前只用1个，200
        /// </summary>
        public long remote_zone_id = 1;
        /// <summary>
        /// 每次登陆都要判定
        /// </summary>
        public long remote_id;
        /// <summary>
        /// 每次登陆都要判定
        /// </summary>
        public bool shopItemFromNet;


        public bool[] repeatedBattleCopy = new bool[1024];

        public void LoadData()
        {
            randomUtil  = new RandomUtil();
            randomUtil.SetSeed(DateTime.Now.Ticks);
            if (DataBaseSystem.LoadData(out growData))
            {
                NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor("读取存档成功",Color.cyan));
                growData.CheckForNewData();
                growData.BuildData();
                SaveData();
            }
            else
            {
                growData = new GrowData();
                growData.CheckForNewData();
                growData.BuildData();
                SaveData();
            }
        }

        public bool SaveData()
        {
            if (!DataBaseSystem.SaveData(growData))
            {
                Debug.LogError("存档失败");
                return false;
            }
            else
            {
                return true;
#if UNITY_EDITOR
                Debug.LogWarning(GrowFun.Instance.growData.growPlayer.GetAttributeDesc());
#endif
            }
        }

        public void ImportData(string record)
        {
            GrowData tmp = null;
            if (DataBaseSystem.ImportData(record, out tmp))
            {
                growData = tmp;
                SystemlogCtrl.PostSystemLog(RichTextUtil.AddColor("读取存档成功",Color.cyan));
                growData.CheckForNewData();
                growData.BuildData();
                SaveData();
            }
            else
            {
                SystemlogCtrl.PostSystemLog(RichTextUtil.AddColor("读取存档失败",Color.red));
            }
        }

        public string ExportData()
        {
            return DataBaseSystem.ExportData(growData);
        }
    }
}