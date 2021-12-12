using Script.Game.Grow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class ShopItem:UIScroolRectItem
{
    public Image back;
    public Image frame;
    public Image icon;
    public TMP_Text equipName;

    private long equip_guid;
    private DictPlayerProp.Model prop;

    // private void Awake()
    // {
    //     UGUIEventListener.Get(back.gameObject).onClick = OnClickEquip;
    // }

    private void OnClickEquip(GameObject go)
    {
        if (equip_guid > 0)
        {
            UIPageManager.Instance.OpenPage("EquipBuyPage", "equip_guid=" + equip_guid + "&from=shop");
        }

        if (prop !=  null)
        {
            UIPageManager.Instance.OpenPage("PropBuyPage", "propId=" + prop.classId);
        }
    }

    void SetEquip(int qulity,string name,int lev,string iconName)
    {
        // equipName.text =  RichTextUtil.AddColor(name+"(Lv."+lev+")",qulity);
        frame.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetFrameByQuality(qulity));
        back.sprite = SpritePackerManager.Instance.GetSprite("Frame", QulityToSprite.GetDiByQuality(qulity));
        equipName.text = "";
        icon.sprite = SpritePackerManager.Instance.GetSprite("equip",iconName);
        icon.gameObject.SetActive(true);

    }

    public void Init(GrowEquip growEquip)
    {
        equip_guid = growEquip.guid;
        SetEquip(growEquip.qulity, growEquip.GetEquipName(),growEquip.lev,growEquip.GetModel().equip_icon);
    }

    
    public void Init(CommonDrop drop)
    {
        prop = DictDataManager.Instance.dictPlayerProp.GetModel((int) drop.propId);
        SetEquip(prop.qulity, prop.propName, 1,prop.icon);
    }

    protected override void OnClick()
    {
        OnClickEquip(this.gameObject);
    }
}