using Game.View;
using Script.Game.Grow;
using TextEquip.System;
using UnityEngine;
using XZXD.UI;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System
{
    public class GameSystem
    {
        public static GameSystem Instance = new GameSystem();
        
        
        
        public InputSystem inputSystem = new InputSystem();
        public TimeEventManager timeeventManager = new TimeEventManager();
        
        public WorldEntity currentWorld;

        public void FreshWorld()
        {
            if (currentWorld != null)
            {
                currentWorld.End();
            }
            WorldConfig worldConfig =
                WorldConfigAPI.Instance.CreateWorld(GrowFun.Instance.growData.growPlayer.playerLv);
            currentWorld = new WorldEntity();
            currentWorld.Init(worldConfig);
            currentWorld.Start();
            NotificationCenter.Default.PostNotification((int)GameMessageId.FreshMap);
            NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,"刷新世界副本");
        }

        public void Tick()
        {
            inputSystem.UpdateInput();
            timeeventManager.OrderUpdate(Time.deltaTime);
            if (currentWorld != null)
            {
                currentWorld.Tick();
            }
        }


        public void Start()
        {
            GrowFun.Instance.LoadData();
            UIPageManager.Instance.OpenPage("MainPage","");
            NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor("欢迎你勇士，点击地图上的副本开始战斗",Color.red));
            NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor("集齐五颗五彩石即可通关",Color.red));
            NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor("菜单栏可以刷新当前世界副本",Color.red));
            timeeventManager.Start();
            this.FreshWorld();
        }

        public void End()
        {
            if (currentWorld != null)
            {
                currentWorld.End();
            }
            timeeventManager.Stop();
            UIPageManager.Instance.CloseAllPage();
        }

        public static void ClearRecordAndStart()
        {
            GameSystem.Instance.End();
            DataBaseSystem.ReStart();
            GameSystem.Instance.Start();
        }

        public static void ReadRecordAndStart(string text)
        {
            GameSystem.Instance.End();
            GrowFun.Instance.ImportData(text);
            GameSystem.Instance.Start();
        }
    }
}