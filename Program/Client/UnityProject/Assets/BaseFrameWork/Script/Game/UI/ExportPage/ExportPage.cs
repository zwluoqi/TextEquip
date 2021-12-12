using Script.Game.Grow;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class ExportPage:UIPage
{
    public TMP_InputField record;

    public GameObject copyBtn;

    private void Awake()
    {
        UGUIEventListener.Get(copyBtn).onClick = delegate(GameObject go)
        {
            // GUIUtility.systemCopyBuffer = GrowFun.Instance.ExportData();
            BoxManager.CreatOneButtonBox("暂不支持，请选中文本，使用系统自带复制功能",null);
        };
        record.onDeselect.AddListener(OnDeselect);
    }

    private void OnDeselect(string arg0)
    {
        record.text = GrowFun.Instance.ExportData();
    }


    protected override void DoOpen()
    {
        record.text = GrowFun.Instance.ExportData();
    }  
}