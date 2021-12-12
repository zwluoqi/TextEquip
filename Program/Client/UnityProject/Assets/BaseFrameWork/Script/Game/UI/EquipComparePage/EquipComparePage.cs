using System;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class EquipComparePage:UIPage
{
    public EquipInfoCtrl sourceInfoCtrl;
    public EquipInfoCtrl targetInfoCtrl;

    public GameObject equipBtn;
    public GameObject strenthBtn;
    public GameObject recastBtn;
    public GameObject costBtn;
    public GameObject popNetBtn;

    public Toggle lockToggle;
    public GameObject opeMode;
    
    // public GameObject popShopBtn;
    
    GrowEquip equip = null;

    private void Awake()
    {
        // UGUIEventListener.Get(popShopBtn).onClick = delegate(GameObject go)
        // {
        //     ShopUtil.PopItem(equip);
        // };
        
        UGUIEventListener.Get(equipBtn).onClick += delegate(GameObject go)
        {
            GrowFun.Instance.growData.PlayerEquip(equip.guid);
            this.Close();
        };
        UGUIEventListener.Get(strenthBtn).onClick += delegate(GameObject go)
        {
            long guid = equip.guid;
            this.Close();
            var page = UIPageManager.Instance.OpenPage("EquipDetailPage", "equip_guid="+guid);
        };
        UGUIEventListener.Get(recastBtn).onClick += delegate(GameObject go)
        {
            long guid = equip.guid;

            this.Close();
            var page = UIPageManager.Instance.OpenPage("EquipDetailPage", "equip_guid="+guid);
        };
        UGUIEventListener.Get(costBtn).onClick += delegate(GameObject go)
        {
            GrowFun.Instance.growData.CostEquip(equip.guid,false);
            this.Close();
        };
        UGUIEventListener.Get(popNetBtn).onClick = delegate(GameObject go)
        {
            if (equip.locked)
            {
                SystemlogCtrl.PostSystemLog("请先解锁后再上架商城");
                return;
            }
            // var equip = GrowFun.Instance.growData.GetEquipByGuid(equip_guid);
            long guid = equip.guid;
            // this.Close();
            var page = UIPageManager.Instance.OpenPage("EquipPopPage", "equip_guid="+guid);
        };

        NotificationCenter.Default.AddObserver(this,OnSCEquipPop,(int)GameMessageId.SCPopShopEquip);
    }

    private void OnSCEquipPop(Notification notification)
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
            opeMode.SetActive(false);
        }
        else
        {
            equip = GrowFun.Instance.growData.GetEquipByGuid(equip_guid);
            opeMode.SetActive(true);
        }
        targetInfoCtrl.Open(equip);
        var sourceEquip = GrowFun.Instance.growData.growPlayer.GetEquipByIndex(equip.GetModel().equip_type_int);
        sourceInfoCtrl.Open(sourceEquip);


        lockToggle.isOn = equip.locked;
        
        lockToggle.onValueChanged.AddListener(delegate(bool lockVal)
        {
            GrowFun.Instance.growData.LockEquip(equip.guid,lockVal);
            costBtn.SetActive(!lockVal);
            popNetBtn.SetActive(!lockVal);
        });
    }

    protected override void DoClose()
    {
        lockToggle.onValueChanged.RemoveAllListeners();
        equip = null;
    }
}