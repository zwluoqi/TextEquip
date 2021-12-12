using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game.System.Entity;
using TMPro;
using UnityEngine;
using XZXD.UI;

public class MapItem : MonoBehaviour
{
    public List<GameObject> diffs = new List<GameObject>();
    public TMP_Text text;
    public GameObject btn;

    private int copyIndex;
    private void Awake()
    {
        UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("CopyPage", "copyIndex=" + copyIndex);
        };
    }

    public void Set(int index,CopyEntity copyEntity)
    {
        this.copyIndex = index;
        foreach (var diff in diffs)
        {
            diff.SetActive(false);
        }
        diffs[copyEntity.config.difficulty-1].SetActive(true);
        text.text = "Lv."+copyEntity.config.lv;
    }
    
}
