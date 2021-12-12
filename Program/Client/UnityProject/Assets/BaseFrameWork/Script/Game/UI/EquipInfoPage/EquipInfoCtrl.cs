using System;
using System.Collections.Generic;
using System.Net.Mime;
using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using TextEquip.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoCtrl : MonoBehaviour
{
    public Image frame;
    public EquipItem equipItem;
    public TMP_Text qulityDesc;
    public TMP_Text equipAuthor;
    public TMP_Text equipType;
    public TMP_Text equipLv;
    public TMP_Text attributePrefab;
    public GameObject baseLine;
    public GameObject extraLine;
    public UnityEngine.UI.Image backFrame;
    public TMP_Text equipDesc;
    public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
    public int baseBackY = 280;
    public TMP_Text equipName;
    
    private List<GameObject> items = new List<GameObject>();
    private void Awake()
    {
        attributePrefab.gameObject.SetActive(false);
    }


    public void Clear()
    {
        foreach (var item in items)
        {
            GameObject.Destroy(item);
        }
        items.Clear();
    }
    
    public void Open(GrowEquip equip)
    {
        Clear();
        equipName.text = RichTextUtil.AddColor( equip.GetEquipName()+"(+"+equip.enchantlvl+",Lv."+equip.lev+")",equip.qulity);
        equipItem.Init(equip);
        qulityDesc.text = RichTextUtil.AddColor(equip.GetQulityModel().qulityName, equip.qulity);
        if (equip.inHeritList.Count > 0)
        {
            equipAuthor.text = "制作者："+equip.GetAuthorName()+"\n传承者："+equip.GetInHeritList()+""; 
        }
        else
        {
            equipAuthor.text = "制作者：" + equip.GetAuthorName();
        }

        
        equipType.text = equip.GetTypeModel().typeName;
        equipLv.text = "lv."+equip.lev;

        var gridY = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
        foreach (var baseItem in equip.baseItems)
        {
            TMP_Text att = GetAtt();
            var enhanceItem = GrowEquipAPI.CalculateStrAttr(baseItem, equip.Getenchantlvl());
            var baseDesc = AttributeItemUtil.GetShowVal(baseItem.type, baseItem.value, baseItem.recastLev);
            if (AttributeItemUtil.IsShowPercent(baseItem.type))
            {
                att.text = string.Format("{0}({1})", baseDesc, ((enhanceItem.value - baseItem.value)*100).ToString("F1")+"%");
            }
            else
            {
                att.text = string.Format("{0}({1})", baseDesc, (enhanceItem.value - baseItem.value).ToString("F1"));
            }

            items.Add(att.gameObject);
        }

        var baseIns = GameObject.Instantiate(baseLine);
        UnityTools.SetCenterParent(baseIns.transform,gridLayoutGroup.transform);
        baseIns.gameObject.SetActive(true);
        items.Add(baseIns.gameObject);
        
        foreach (var baseItem in equip.extraItems)
        {
            TMP_Text att = GetAtt();
            var baseDesc = AttributeItemUtil.GetShowVal(baseItem.type, baseItem.value, baseItem.recastLev);
            att.text = string.Format("{0}({1}%)", baseDesc, (baseItem.recastLev).ToString());
            items.Add(att.gameObject);
        }

        var extraIns = GameObject.Instantiate(extraLine);
        UnityTools.SetCenterParent(extraIns.transform,gridLayoutGroup.transform);
        extraIns.gameObject.SetActive(true);
        items.Add(extraIns.gameObject);

        var rect = backFrame.GetComponent<RectTransform>();
        rect.sizeDelta =
            new Vector2(rect.sizeDelta.x, (2 + equip.baseItems.Count + equip.extraItems.Count)*gridY + baseBackY);

        equipDesc.text = equip.GetEquipDesc();
    }

    private TMP_Text GetAtt()
    {
        var att = GameObject.Instantiate(attributePrefab);
        UnityTools.SetCenterParent(att.transform,gridLayoutGroup.transform);
        att.gameObject.SetActive(true);
        return att;
    }
}