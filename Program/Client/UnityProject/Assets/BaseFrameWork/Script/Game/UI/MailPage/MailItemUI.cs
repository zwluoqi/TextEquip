using System;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class MailItemUI : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text desc;
    public Image icon;
    public TMP_Text numText;
    public TMP_Text nameText;
    
    public GameObject btn;

    private int num = 0;
    private string classId = "";
    private string propId = "";
    private string objId = "";

    private void Awake()
    {
        UGUIEventListener.Get(btn).onClick = delegate(GameObject go)
        {
            MailUtil.DeleteMail(guid);
        };
        
        NotificationCenter.Default.AddObserver(this,OnDeleteMail,(int)GameMessageId.SCDeleteMail);

    }

    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }

    private void OnDeleteMail(Notification notification)
    {
        btn.SetActive(false);
        if (string.IsNullOrEmpty(classId))
        {
            return;
        }
        GrowFun.Instance.growData.AddProp(propId,num);
    }

    private long guid;

    public void Init(JToken mKvmailValue)
    {
        guid = (long)mKvmailValue["id"];
        title.text = (string)mKvmailValue["title"];
        desc.text = (string)mKvmailValue["detail"];
        string rewards = (string)mKvmailValue["detail"];
        var drops = CommonUtil.GetSplitString(rewards, '.');
        for (int i = 0; i < drops.Length; i++)
        {
            if (string.IsNullOrEmpty(drops[i]))
            {
                continue;
            }
            var dropReward  =  CommonUtil.GetSplitString(drops[i], ',');
            if (string.IsNullOrEmpty(classId))
            {
                continue;
            }
            classId = dropReward[0];
            propId = dropReward[1];
            objId = dropReward[2];
            num = int.Parse(dropReward[3]);
            numText.text = "X" + num;
            nameText.text = propId;
            break;
        }
        
    }
}