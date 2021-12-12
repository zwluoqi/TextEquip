using System;
using Script.Game.Grow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class BagItemUI:UIScroolRectItem
{
    public Image back;
    public Image frame;
    public Image icon;
    public TMP_Text equipName;
    public GameObject btn;
    
    private long equip_guid;


    private void Awake()
    {
        // UGUIEventListener.Get(btn.gameObject).onClick = OnClickEquip;
    }

    private void OnClickEquip(GameObject go)
    {
        UIPageManager.Instance.OpenPage("EquipComparePage", "equip_guid=" + equip_guid);
    }

    public void SetEquip(int qulity,string name,int lev,string iconName)
    {
        // equipName.text =  RichTextUtil.AddColor(name+"(Lv."+lev+")",qulity);
        equipName.text = "";
        icon.sprite = SpritePackerManager.Instance.GetSprite("equip",iconName);
        icon.gameObject.SetActive(true);
        
        frame.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetFrameByQuality(qulity));
        back.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetDiByQuality(qulity));
        icon.sprite = SpritePackerManager.Instance.GetSprite("equip",iconName);
    }

    public void Init(GrowEquip growEquip)
    {
        equip_guid = growEquip.guid;
        SetEquip(growEquip.qulity, growEquip.GetEquipName(),growEquip.lev,growEquip.GetModel().equip_icon);
    }

    protected override void OnClick()
    {
        UIPageManager.Instance.OpenPage("EquipComparePage", "equip_guid=" + equip_guid);

    }
}