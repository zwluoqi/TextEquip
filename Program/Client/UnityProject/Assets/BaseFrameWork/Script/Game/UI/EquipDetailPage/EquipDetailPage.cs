using System;
using System.Collections.Generic;
using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using Script.Game.System;
using TextEquip.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class EquipDetailPage:UIPage
{
    public EquipItem equipItem;

    public TMP_Text equipName;
    // public RectTransform baseAttributeBack;
    // public RectTransform frameBack;
    public TMP_Text strenthText;
    public TMP_Text strenthCoin;
    public GameObject strenthBtn;
    public GameObject autoStrenthBtn;
    public GameObject stopAutoStrenthBtn;
    public TMP_InputField autoStrenthTargetLev;
    
    public BaseAttItemUI attributePrefab;
    public ExtraAttItemUI extraItemUIPrefab;
    public UnityEngine.UI.GridLayoutGroup baseGridLayoutGroup;
    public UnityEngine.UI.GridLayoutGroup extraGridLayoutGroup;
    // public Toggle baodi;
    // public TMP_Dropdown xingyun;
    List<GameObject> atts = new List<GameObject>();
    List<GameObject> extras = new List<GameObject>();
    
    private long equip_guid;

    private void Awake()
    {
        attributePrefab.gameObject.SetActive(false);
        extraItemUIPrefab.gameObject.SetActive(false);
        UGUIEventListener.Get(strenthBtn).onClick  = delegate(GameObject go)
        {
            bool success = GrowFun.Instance.growData.StartStreng(equip_guid,false,99,false,0);
            if (success)
            {
                FreshU();
            }
        };
        UGUIEventListener.Get(autoStrenthBtn).onClick  = delegate(GameObject go)
        {
            _timeEventHandler = GameSystem.Instance.timeeventManager.CreateEvent(AutoStartStreng, 0, 1);
            autoStrenthBtn.SetActive(false);
            stopAutoStrenthBtn.SetActive(true);
        };
        UGUIEventListener.Get(stopAutoStrenthBtn).onClick = delegate(GameObject go)
        {
            TimeEventManager.Delete(ref _timeEventHandler);
            autoStrenthBtn.SetActive(true);
            stopAutoStrenthBtn.SetActive(false);
        };
    }
    
    
    private TimeEventHandler _timeEventHandler;
    private void AutoStartStreng()
    {
        bool success = GrowFun.Instance.growData.StartStreng(equip_guid,true,int.Parse(autoStrenthTargetLev.text),false,0);
        if (!success)
        {
            TimeEventManager.Delete(ref _timeEventHandler);
            autoStrenthBtn.SetActive(true);
            stopAutoStrenthBtn.SetActive(false);
        }
        else
        {
            // var equip = GrowFun.Instance.growData.GetEquipByGuid(this.equip_guid);
            // strenthCoin.text = equip.StrengthenNeedGold().ToString();
            // strenthText.text = "强化至+" + (equip.Getenchantlvl()+1);
            FreshU();
        }
    }



    void FreshU()
    {
        ClearUI();
        equip_guid = this.GetLongOptionValue("equip_guid");
        var equip = GrowFun.Instance.growData.GetEquipByGuid(this.equip_guid);
        equipName.text = RichTextUtil.AddColor( equip.GetEquipName()+"(+"+equip.enchantlvl+",Lv."+equip.lev+")",equip.qulity);        
        equipItem.Init(equip);
        var gridY = baseGridLayoutGroup.cellSize.y + baseGridLayoutGroup.spacing.y;
        foreach (var baseItem in equip.baseItems)
        {
            var att = GetAtt(baseGridLayoutGroup);
            

            att.SetInfo(equip,baseItem);
            atts.Add(att.gameObject);
        }
        
        for(int i=0;i<equip.extraItems.Count;i++)
        {
            var att = GameObject.Instantiate(extraItemUIPrefab);
            UnityTools.SetCenterParent(att.transform,extraGridLayoutGroup.transform);
            att.gameObject.SetActive(true);
            att.SetExtraItem(equip, i);
            extras.Add(att.gameObject);
        }
        // var equip = GrowFun.Instance.growData.GetEquipByGuid(this.equip_guid);
        strenthCoin.text = "需要金币："+equip.StrengthenNeedGold().ToString();
        strenthText.text = "强化至+" + (equip.Getenchantlvl()+1);
    }
    
    
    protected override void DoOpen()
    {
        autoStrenthBtn.SetActive(true);
        stopAutoStrenthBtn.SetActive(false);
        FreshU();
    }


    private BaseAttItemUI GetAtt(GridLayoutGroup gridLayoutGroup)
    {
        var att = GameObject.Instantiate(attributePrefab);
        UnityTools.SetCenterParent(att.transform,gridLayoutGroup.transform);
        att.gameObject.SetActive(true);
        return att;
    }

    protected override void DoClose()
    {
        TimeEventManager.Delete(ref _timeEventHandler);
        ClearUI();
    }

    void ClearUI()
    {
        foreach (var att in atts)
        {
            GameObject.Destroy(att);
        }
        atts.Clear();
        foreach (var att in extras)
        {
            GameObject.Destroy(att);
        }
        extras.Clear();
    }
}