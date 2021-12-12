using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game.Grow;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class PropBuyPage : UIPage
{

    public ShopItem shopItem;
    public GameObject btn;
    public TMP_Text itemName;
    public TMP_Text desc;
    public TMP_Text text;
    private DictPlayerPropEnum propId;
    private void Awake()
    {
        UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
        {
            var propNum = GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.wucai_suipian);
            if (propNum >= 20)
            {
                //
                GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.wucai_suipian, -20);
                GrowFun.Instance.growData.AddProp(propId, 1);
            }
            else
            {
                BoxManager.CreatePopTis("五彩碎片不足");
            }
        };
    }

    protected override void DoOpen()
    {
        propId = (DictPlayerPropEnum)this.GetIntOptionValue("propId");
        CommonDrop commonDrop = new CommonDrop();
        commonDrop.propId = (DictPlayerPropEnum)propId;
        shopItem.Init(commonDrop);
        var num = GrowFun.Instance.growData.GetPropByID(propId);
        var suipian_num = GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.wucai_suipian);
        var propModel = DictDataManager.Instance.dictPlayerProp.GetModel((int)propId); 
        text.text = string.Format( "{3}个五彩碎片可以兑换一个\r\n当前五彩碎片:X{0}\r\n当前{1}:X{2}" ,suipian_num, propModel.propName,num,RichTextUtil.AddColor("20",Color.green));
        desc.text = propModel.detail;
        itemName.text = propModel.propName;
    }
    
}
