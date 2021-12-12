using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game;
using Script.Game.Grow;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class AttributeCtrl : MonoBehaviour
{
    public TMP_Text atk;
    public TMP_Text crit;
    public TMP_Text critdmg;
    public TMP_Text def;
    public TMP_Text bloc;
    
    
    public GameObject atkBtn;
    public GameObject critBtn;
    public GameObject critdmgBtn;
    public GameObject defBtn;
    public GameObject blocBtn;

    private void Awake()
    {
        UGUIEventListener.Get(atkBtn).onLongClick = delegate(GameObject go)
        {
            BoxManager.OpenTipsPage(LanguageUtil.GetLanguage("atk_desc"),go.transform.position);
        };
        UGUIEventListener.Get(atkBtn).onPress = BoxManager.CloseTipsPageOnPress;
        UGUIEventListener.Get(critBtn).onLongClick = delegate(GameObject go)
        {
            BoxManager.OpenTipsPage(LanguageUtil.GetLanguage("crit_desc"),go.transform.position);
        };
        UGUIEventListener.Get(critBtn).onPress = BoxManager.CloseTipsPageOnPress;
        UGUIEventListener.Get(critdmgBtn).onLongClick = delegate(GameObject go)
        {
            BoxManager.OpenTipsPage(LanguageUtil.GetLanguage("critdmg_desc"),go.transform.position);
        };
        UGUIEventListener.Get(critdmgBtn).onPress = BoxManager.CloseTipsPageOnPress;
        UGUIEventListener.Get(defBtn).onLongClick = delegate(GameObject go)
        {
            BoxManager.OpenTipsPage(LanguageUtil.GetLanguage("def_desc"),go.transform.position);
        };
        UGUIEventListener.Get(defBtn).onPress = BoxManager.CloseTipsPageOnPress;
        UGUIEventListener.Get(blocBtn).onLongClick = delegate(GameObject go)
        {
            BoxManager.OpenTipsPage(LanguageUtil.GetLanguage("bloc_desc"),go.transform.position);
        };
        UGUIEventListener.Get(blocBtn).onPress = BoxManager.CloseTipsPageOnPress;
        NotificationCenter.Default.AddObserver(this,OnAttUpdate,(int)GameMessageId.FreshAttributeUI);

    }

    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }


    private void OnAttUpdate(Notification notification)
    {
        Fresh();
    }

    public void Fresh()
    {
        var player = GrowFun.Instance.growData.growPlayer;
        
        atk.text = player.GetAttributeByProp(DictAbilityPropEnum.ATK).ToString("F1");
        crit.text = (100*player.GetAttributeByProp(DictAbilityPropEnum.CRIT)).ToString("F1")+"%";
        critdmg.text = (100*player.GetAttributeByProp(DictAbilityPropEnum.CRITDMG)).ToString("F1")+"%";
        def.text = player.GetAttributeByProp(DictAbilityPropEnum.DEF).ToString("F1");
        bloc.text = player.GetAttributeByProp(DictAbilityPropEnum.BLOC).ToString("F1");
    }
}
