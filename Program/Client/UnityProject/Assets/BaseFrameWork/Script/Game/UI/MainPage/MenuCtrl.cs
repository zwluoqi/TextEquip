using System;
using Script.Game.Grow;
using Script.Game.System;
using UnityEngine;
using XZXD.UI;

public class MenuCtrl:MonoBehaviour
{
    public GameObject bagBtn;
    public GameObject shopBtn;
    public GameObject freshBtn;
    public GameObject chongshengBtn;
    public GameObject saveBtn;
    public GameObject exportBtn;
    public GameObject importBtn;

    public GameObject cleatRecordBtn;
    public GameObject updateRecordBtn;
    public GameObject QABtn;


    private void Awake()
    {
        UGUIEventListener.Get(bagBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("BagPage", "");
        };
        UGUIEventListener.Get(shopBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("ShopPage", "");
        };
        UGUIEventListener.Get(chongshengBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("ChongShengPage", "");
        };
        UGUIEventListener.Get(saveBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            bool success = GrowFun.Instance.SaveData();
            if (success)
            {
                BoxManager.CreatePopTis("保存成功");
            }
        };
        UGUIEventListener.Get(exportBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("ExportPage", "");
        };
        UGUIEventListener.Get(importBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("ImportPage", "");
        };
        UGUIEventListener.Get(freshBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            BoxManager.OpenYesAndNoPage("是否刷新当前世界副本?刷新副本会结束当前正在进行的副本", delegate(bool b)
            {
                if (b)
                {
                    GameSystem.Instance.FreshWorld();
                }
            });
            
        };
        UGUIEventListener.Get(cleatRecordBtn.transform.GetChild(0).gameObject).onClick += delegate(GameObject go)
        {
            BoxManager.OpenYesAndNoPage("是否清除存档?清除存档将会销毁所有数据，是否确认？", delegate(bool b)
            {
                if (b)
                {
                    BoxManager.OpenYesAndNoPage("你想清楚了吗，是否一定要清除存档？", delegate(bool b1)
                    {
                        if (b1)
                        {
                            GameSystem.ClearRecordAndStart();
                        }
                    });
                }
            });
        };
        
    }
}