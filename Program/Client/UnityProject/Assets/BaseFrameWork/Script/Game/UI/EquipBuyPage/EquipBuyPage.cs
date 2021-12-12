using System;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class EquipBuyPage:UIPage
{
    public EquipInfoCtrl sourceInfoCtrl;
    public EquipInfoCtrl targetInfoCtrl;

    public GameObject btn;
    public TMP_Text coin;
    private GrowEquip equip;

    private void Awake()
    {
        UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
        {
            ShopUtil.BuyShopItem(equip);
        };
        NotificationCenter.Default.AddObserver(this,OnBuyDone,(int)GameMessageId.SCBuyShopEquip);
    }

    private void OnBuyDone(Notification notification)
    {
        this.Close();
    }

    protected override void DoOpen()
    {
        var equip_guid = this.GetLongOptionValue("equip_guid");
        var from = this.GetOptionValue("from");
        if (from == "shop")
        {
            equip = ShopUtil.GetEquipByGuid(equip_guid);
        }
        else
        {
            equip = GrowFun.Instance.growData.GetEquipByGuid(equip_guid);
        }
        targetInfoCtrl.Open(equip);
        var sourceEquip = GrowFun.Instance.growData.growPlayer.GetEquipByIndex(equip.GetModel().equip_type_int);
        sourceInfoCtrl.Open(sourceEquip);
        coin.text = "售价:" + equip.GetBuyCostCoin();
    }

    protected override void DoClose()
    {
        equip = null;
    }
}