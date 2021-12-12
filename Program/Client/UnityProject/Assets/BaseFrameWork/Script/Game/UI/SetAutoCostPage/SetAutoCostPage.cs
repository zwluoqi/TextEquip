using System;
using Script.Game.Grow;
using XZXD.UI;

public class SetAutoCostPage:UIPage
{
        public UnityEngine.UI.Toggle qulity10Btn;
        public UnityEngine.UI.Toggle qulity11Btn;
        public UnityEngine.UI.Toggle qulity12Btn;
        public UnityEngine.UI.Toggle qulity13Btn;


        private void Awake()
        {
                
        }

        protected override void DoOpen()
        {
                qulity10Btn.isOn = GrowFun.Instance.growData.GetAutoCostEquip(10);
                qulity11Btn.isOn = GrowFun.Instance.growData.GetAutoCostEquip(11);
                qulity12Btn.isOn = GrowFun.Instance.growData.GetAutoCostEquip(12);
                qulity13Btn.isOn = GrowFun.Instance.growData.GetAutoCostEquip(13);
                qulity10Btn.onValueChanged.AddListener(OnQulity10Btn);
                qulity11Btn.onValueChanged.AddListener(OnQulity11Btn);
                qulity12Btn.onValueChanged.AddListener(OnQulity12Btn);
                qulity13Btn.onValueChanged.AddListener(OnQulity13Btn);
        }

        protected override void DoClose()
        {
                qulity10Btn.onValueChanged.RemoveAllListeners();
                qulity11Btn.onValueChanged.RemoveAllListeners();
                qulity12Btn.onValueChanged.RemoveAllListeners();
                qulity13Btn.onValueChanged.RemoveAllListeners();
        }

        private void OnQulity13Btn(bool arg0)
        {
                GrowFun.Instance.growData.SetAutoCostEquip(13,arg0);
        }

        private void OnQulity12Btn(bool arg0)
        {
                GrowFun.Instance.growData.SetAutoCostEquip(12,arg0);
        }

        private void OnQulity11Btn(bool arg0)
        {
                GrowFun.Instance.growData.SetAutoCostEquip(11,arg0);       
        }

        private void OnQulity10Btn(bool arg0)
        {
                GrowFun.Instance.growData.SetAutoCostEquip(10,arg0);
        }
}