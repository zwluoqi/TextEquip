using System;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class RegisterPage:UIPage
{
        public TMP_InputField userName;
        public GameObject registerBtn;


        private void Awake()
        {
                UGUIEventListener.Get(registerBtn).onClick = delegate(GameObject go)
                {
                        if (string.IsNullOrEmpty(userName.text))
                        {
                                BoxManager.CreatePopTis("请输入名称");
                                return;
                        }

                        if (string.IsNullOrEmpty(GrowFun.Instance.growData.growPlayer.playerName))
                        {
                                AccountUtil.RequestLogin(true, userName.text);
                        }
                        else
                        {
                                AccountUtil.RequestLogin(false, GrowFun.Instance.growData.growPlayer.playerName);
                        }
                };
                NotificationCenter.Default.AddObserver(this,OnLogin,(int)GameMessageId.SCLoginDone);
        }

        private void OnLogin(Notification notification)
        {
                this.Close();
        }

        protected override void DoOpen()
        {
                userName.text = GrowFun.Instance.growData.growPlayer.playerName;
        }
}