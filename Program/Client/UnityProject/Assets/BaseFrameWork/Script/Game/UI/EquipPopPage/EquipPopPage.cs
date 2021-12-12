using System;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class EquipPopPage:UIPage
{
        public TMP_InputField authorName;
        public TMP_InputField equipName;
        public TMP_InputField equipDesc;
        public TMP_InputField costCoin;
        
        public GameObject btn;

        private GrowEquip equip;
        private void Awake()
        {
                UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
                {
                        ShopUtil.PopNetItem(equip,authorName.text,
                                equipName.text,
                                equipDesc.text,
                                long.Parse( costCoin.text));
                };
                NotificationCenter.Default.AddObserver(this,OnSCEquipPop,(int)GameMessageId.SCPopShopEquip);

        }

        private void OnSCEquipPop(Notification notification)
        {
                this.Close();
        }

        protected override void DoOpen()
        {
                costCoin.contentType = TMP_InputField.ContentType.IntegerNumber;  
                var equip_guid = this.GetLongOptionValue("equip_guid");
                equip = GrowFun.Instance.growData.GetEquipByGuid(equip_guid);
                if (!string.IsNullOrEmpty(equip.authorName))
                {
                        authorName.text = equip.authorName;
                        authorName.enabled = false;
                        equipName.text = equip.equipName;
                        equipName.enabled = false;
                        equipDesc.text = equip.equipDesc;
                        equipDesc.enabled = false;
                }
                else
                {
                        authorName.text = GrowFun.Instance.growData.growPlayer.playerName;
                        authorName.enabled = true;
                        equipName.text = equip.GetModel().name;
                        equipName.enabled = true;
                        equipDesc.text = equip.GetModel().desc;
                        equipDesc.enabled = true; 
                }
        }
}