using System;
using Script.Game.Grow;
using Script.Game.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class CopyPage:UIPage
{

    public TMP_Text copyName;
    public TMP_Text copyDPS;
    public TMP_Text copyLv;
    public TMP_Text diff;
    public Toggle mud;
    public GameObject battleBtn;
    int copyIndex;


    private void Awake()
    {
        UGUIEventListener.Get(battleBtn).onClick = delegate(GameObject go)
        {
            this.Close();
            GameSystem.Instance.currentWorld.StartCopy(copyIndex, mud.isOn);
        };
    }

    protected override void DoOpen()
    {
        copyIndex = this.GetIntOptionValue("copyIndex");
        var configCopy =  GameSystem.Instance.currentWorld.config.copyList[copyIndex];
        copyName.text = "当前副本:"+configCopy.name;
        copyDPS.text = "推荐DPS:" + configCopy.needDPS;
        copyLv.text = "副本等级:" + configCopy.lv;
        diff.text = "当前副本难度等级:" + configCopy.difficultyName;
        
        if (GrowFun.Instance.growData.HasLvCopyCanMud(configCopy.lv))
        {
            mud.gameObject.SetActive(true);
        }
        else
        {
            mud.isOn = false;
            mud.gameObject.SetActive(false);
        }
        mud.isOn = GrowFun.Instance.repeatedBattleCopy[configCopy.lv];
        mud.onValueChanged.AddListener(delegate(bool arg0)
        {
            GrowFun.Instance.repeatedBattleCopy[configCopy.lv] = arg0;
        });
    }

    protected override void DoClose()
    {
        mud.onValueChanged.RemoveAllListeners();
    }
}