using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game.Grow;
using UnityEngine;
using XZXD.UI;

public class MainPage : UIPage
{
    public EquipCtrl equipCtrl;
    public BaseInfoCtrl baseInfoCtrl;
    public AttributeCtrl attributeCtrl;
    public MapCtrl mapCtrl;
    public MenuCtrl menuCtrl;


    protected override void DoClose()
    {
        mapCtrl.DoClose();
    }


    protected override void DoOpen()
    {
        if (string.IsNullOrEmpty(VersionTool.url))
        {
            #if UNITY_EDITOR
            // VersionTool.url = "http://127.0.0.1:10008/xzxdweb/";
            VersionTool.url = "http://114.67.88.112:8080/xzxdweb/";

            #else
            VersionTool.url = "http://114.67.88.112:8080/xzxdweb/";
            #endif
            NetManager.ResetZoneUrl(VersionTool.url);
        }
        equipCtrl.Fresh();
        baseInfoCtrl.Fresh();
        attributeCtrl.Fresh();
        RunCoroutine.Run(_ShowStory());
    }

    protected override void DoOnCoverPageRemove(UIPage coverPage)
    {
        RunCoroutine.Run(_ShowStory());
    }

    private IEnumerator _ShowStory()
    {
        yield return null;
        if (!GrowFun.Instance.growData.storyTips[1])
        {
            UIPageManager.Instance.OpenPage("StoryPage", "story_id=1");
            yield break ;
        }
        if (!GrowFun.Instance.growData.storyTips[2])
        {
            UIPageManager.Instance.OpenPage("StoryPage", "story_id=2");
            yield break ;
        }
        if (!GrowFun.Instance.growData.storyTips[3])
        {
            UIPageManager.Instance.OpenPage("StoryPage", "story_id=3");
            yield break ;
        }
        if (!GrowFun.Instance.growData.storyTips[4])
        {
            UIPageManager.Instance.OpenPage("StoryPage", "story_id=4");
            yield break ;
        }
    }
}
