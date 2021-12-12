using System.Collections;
using System.Collections.Generic;
using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using TextEquip.System;
using TMPro;
using UnityEngine;

public class BaseAttItemUI : MonoBehaviour
{
    public TMP_Text source;
    public TMP_Text target;

    public void SetInfo(GrowEquip equip, AbilityItem baseItem)
    {
        var enhanceItem = GrowEquipAPI.CalculateStrAttr(baseItem, equip.Getenchantlvl());
        var enhanceAddItem = GrowEquipAPI.CalculateStrAttr(baseItem, equip.Getenchantlvl()+1);
        var enchanceDesc = AttributeItemUtil.GetShowVal(baseItem.type, enhanceItem.value, enhanceItem.recastLev);
        ;
        if (AttributeItemUtil.IsShowPercent(baseItem.type))
        {
            source.text = string.Format("{0}", enchanceDesc);
            target.text = string.Format("{0}(+{1})", (enhanceAddItem.value * 100).ToString("F1") + "%",
                ((enhanceAddItem.value - enhanceItem.value) * 100).ToString("F1")+"%");
        }
        else
        {
            source.text = string.Format("{0}", enchanceDesc);
            target.text = string.Format("{0}(+{1})", (enhanceAddItem.value * 1).ToString("F1") + "",
                ((enhanceAddItem.value - enhanceItem.value) * 1).ToString("F1")+"");
        }
    }
}
