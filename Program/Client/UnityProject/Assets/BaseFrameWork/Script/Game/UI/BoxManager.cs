using System;
using UnityEngine;
using XZXD.UI;
public class BoxManager
{
    public static void OpenYesAndNoPage(string desc, Action<bool> action)
    {
        var uiPage =  (YesAndNoPage)UIPageManager.Instance.OpenPage("YesAndNoPage", desc);
        uiPage.Init("确定","取消",desc,action);
        //TODO
        // action(true);
    }

    public static void OpenTipsPage(string getLanguage,Vector3 pos)
    {
        var uiPage =  (TipsPage)UIPageManager.Instance.OpenPage("TipsPage", "");
        uiPage.SetTip( getLanguage);
        uiPage.transform.position = pos;
    }

    public static void OpenSimpleGongGao()
    {
        
    }

    public static void CreatOneButtonBox( string desc, Action<bool> action)
    {
        var uiPage =  (OneButtonPage)UIPageManager.Instance.OpenPage("OneButtonPage", desc);
        uiPage.Init("确定","提示",desc,action);
    }

    public static void CreateNetMask()
    {
        
    }

    static PopTipsManager popTipsManager = new PopTipsManager();
    public static void CreatePopTis(string input)
    {
        popTipsManager.ShowTips(input);
        #if UNITY_EDITOR
        SystemlogCtrl.PostSystemLog(input);
        #endif
    }

    public static void CloseTipsPageOnPress(GameObject go, bool press)
    {
        if (!press)
        {
            BoxManager.CloseTipsPage();
        }
    }
    
    
    public static void CloseTipsPage()
    {
        var tipsPage = UIPageManager.Instance.GetPage("TipsPage");
        if (tipsPage != null)
        {
            UIPageManager.Instance.ClosePage(tipsPage);
        }
    }
}