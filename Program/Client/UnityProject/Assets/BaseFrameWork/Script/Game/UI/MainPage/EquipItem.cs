using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipItem:MonoBehaviour
{
    public Image back;
    public Image frame;
    public Image icon;
    public TMP_Text equipName;
    public TMP_Text longEquipName;


    void SetEquip(int qulity,string _name,int lev,string iconName)
    {
        equipName.text = "";
        icon.sprite = SpritePackerManager.Instance.GetSprite("equip",iconName);
        icon.gameObject.SetActive(true);

        // equipName.text =  RichTextUtil.AddColor(_name+"(Lv."+lev+")",qulity);
        frame.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetFrameByQuality(qulity));
        back.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetDiByQuality(qulity));
        // icon.sprite = SpritePackerManager.Instance.GetSprite("0","0");
        
    }

    public void Init(GrowEquip growEquip)
    {
        SetEquip(growEquip.qulity, growEquip.GetEquipName(),growEquip.lev,growEquip.GetModel().equip_icon);
        if (longEquipName != null)
        {
            longEquipName.text = RichTextUtil.AddColor(growEquip.GetEquipName()+"(+"+growEquip.enchantlvl+",Lv."+growEquip.lev+")",growEquip.qulity);
        }
    }
}