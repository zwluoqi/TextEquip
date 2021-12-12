using System;
using Script.Game.Grow;
using Script.Game.System;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class ImportPage:UIPage
{
        public TMP_InputField record;

        public GameObject btn;

        private void Awake()
        {
                UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
                {
                        BoxManager.OpenYesAndNoPage("读取存档将会覆盖当前存档，是否确认?", delegate(bool b)
                        {
                                if (b)
                                {
                                        GameSystem.ReadRecordAndStart(record.text);
                                }
                        });
                };
        }

        protected override void DoOpen()
        {
                record.text = "";
        }
}