using System;
using Script.Game;
using Script.Game.Grow.NetData;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class MailPage:UIPage
{
    public GridLayoutGroup gridLayoutGroup;
    public MailItemUI bagItemUiPrefab;

    
    private void Awake()
    {
        NotificationCenter.Default.AddObserver(this,OnMailList,(int)GameMessageId.SCMailList);
    }

    private void OnMailList(Notification notification)
    {
        FreshUI();
    }

    private void FreshUI()
    {
        foreach (var mKvmail in MailUtil.kvmails)
        {
            var item = GameObject.Instantiate(bagItemUiPrefab);
            UnityTools.SetCenterParent(item.transform,gridLayoutGroup.transform);
            item.gameObject.SetActive(true);
            item.Init(mKvmail.Value);
        }
    }

    protected override void DoOpen()
    {
        MailUtil.RequestMail();
    }
}