using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game;
using Script.Game.Grow;
using TMPro;
using UnityEngine;

public class BaseInfoCtrl : MonoBehaviour
{

    public TMP_Text playerLv;
    public TMP_Text chongshengCount;
    public TMP_Text curHP;
    public TMP_Text DPS;
    public TMP_Text COIN;
    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }
    private void Awake()
    {
        NotificationCenter.Default.AddObserver(this,OnHpUpdate,(int)GameMessageId.FreshHPUI);
        // NotificationCenter.Default.AddObserver(this,OnLvUpdate,(int)GameMessageId.FreshLvUI);
        NotificationCenter.Default.AddObserver(this,OnFresh,(int)GameMessageId.BattleCopyEntityStateChange);
        NotificationCenter.Default.AddObserver(this,OnFresh,(int)GameMessageId.FreshAttributeUI);
    }

    private void OnFresh(Notification notification)
    {
        Fresh();
    }

    private void OnLvUpdate(Notification notification)
    {
        var player = GrowFun.Instance.growData.growPlayer;
        playerLv.text = "Lv." + player.playerLv;

    }

    private void OnHpUpdate(Notification notification)
    {
        var player = GrowFun.Instance.growData.growPlayer;
        var maxhp = (int) player.GetAttributeByProp(DictAbilityPropEnum.MAX_HP);
        curHP.text = (int)(maxhp*player.hpPercent) + "/" +
                     (int)maxhp;
    }

    public void Fresh()
    {
        var player = GrowFun.Instance.growData.growPlayer;
        playerLv.text = "Lv." + player.playerLv;
        chongshengCount.text = "重生次数:" + player.chongshengCount;
        var maxhp = (int) player.GetAttributeByProp(DictAbilityPropEnum.MAX_HP);
        curHP.text = (int)(maxhp*player.hpPercent) + "/" +
                     (int)maxhp;
        DPS.text = player.GetAttributeByProp(DictAbilityPropEnum.DPS).ToString("F2");
        COIN.text = GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.coin).ToString();
    }
}
