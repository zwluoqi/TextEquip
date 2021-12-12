using System;
using Script.Game.Grow;
using TextEquip.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class ExtraAttItemUI:MonoBehaviour
{
    public Image back;
    public Image frame;
    public TMP_Text att;
    public TMP_Text costCoin;
    private int extraIndex;
    private GrowEquip extraEquip;

    private void Awake()
    {
        UGUIEventListener.Get(this.gameObject).onClick = delegate(GameObject go)
        {
            GrowFun.Instance.growData.RecastTheEquiment(extraEquip, extraIndex);
            FreshUI();
        };
    }

    public void SetExtraItem(GrowEquip equip, int index)
    {
        this.extraEquip = equip;
        this.extraIndex = index;
        FreshUI();
    }

    private void FreshUI()
    {
        var baseItem = extraEquip.extraItems[extraIndex];
        var baseDesc = AttributeItemUtil.GetShowVal(baseItem.type, baseItem.value, baseItem.recastLev);
        att.text = string.Format("{0}({1}%)", baseDesc, (baseItem.recastLev).ToString());
        var qulity = baseItem.recastLev / 25;
        frame.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetFrameByQuality(qulity));
        back.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetDiByQuality(qulity));
        costCoin.text = "消耗金币："+extraEquip.RecastNeedGold(this.extraIndex);
    }
}