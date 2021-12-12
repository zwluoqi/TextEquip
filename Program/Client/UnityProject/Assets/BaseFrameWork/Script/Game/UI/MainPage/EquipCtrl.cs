using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game;
using Script.Game.Grow;
using UnityEngine;
using XZXD.UI;

public class EquipCtrl : MonoBehaviour
{

    public EquipItem[] equipItems;
    // public GameObject[] equipBtnItems;
    private void Awake()
    {
        for (int i = 0; i < equipItems.Length; i++)
        {
            UGUIEventListener.Get(equipItems[i].gameObject).onClick = OnEquipDetail;
        }
        NotificationCenter.Default.AddObserver(this,OnEquipStrenth,(int)GameMessageId.StrenthEquip);
        NotificationCenter.Default.AddObserver(this,OnEquipStrenth,(int)GameMessageId.RecastEquip);
        NotificationCenter.Default.AddObserver(this,OnPlayerEquipEquip,(int)GameMessageId.PlayerEquipEquip);
        
    }
    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }
    private void OnPlayerEquipEquip(Notification notification)
    {
        Fresh();
    }

    private void OnEquipStrenth(Notification notification)
    {
        var equip_guid = (long)notification.info;
        if (GrowFun.Instance.growData.growPlayer.HasEquipEquip(equip_guid))
        {
            Fresh();
        }
    }

    private void OnEquipDetail(GameObject go)
    {
        int index = -1;
        for (int i = 0; i < equipItems.Length; i++)
        {
            if (equipItems[i].gameObject == go)
            {
                index = i;
                break;
            }
        }
        GrowEquip growEquip = GrowFun.Instance.growData.growPlayer.GetEquipByIndex(index);
        UIPageManager.Instance.OpenPage("EquipDetailPage", "equip_guid=" + growEquip.guid);
    }

    public void Fresh()
    {
        for(int i=0;i<equipItems.Length;i++)
        {
            GrowEquip growEquip = GrowFun.Instance.growData.growPlayer.GetEquipByIndex(i);
            equipItems[i].Init(growEquip);
        }
    }
}
