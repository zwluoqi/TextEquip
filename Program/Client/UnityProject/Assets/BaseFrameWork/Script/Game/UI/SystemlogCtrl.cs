using System;
using System.Collections;
using System.Collections.Generic;
using Script.Game;
using UnityEngine;
using XZXD.UI;

public class SystemlogCtrl : MonoBehaviour
{
    public GameObject go;
    private RectTransform _rectTransform;
    public List<Systemlog> systemlogs = new List<Systemlog>();
    public int curPosy = 0;
    public int defaultYHeight = 0;
    public GameObject clearBtn;
    private void Awake()
    {
        NotificationCenter.Default.AddObserver(this,OnAddMessage,(int)GameMessageId.SystemLogId);
        _rectTransform = GetComponent<RectTransform>();
        // defaultYHeight = (int)_rectTransform.sizeDelta.y;
        defaultYHeight = 200;
        go.SetActive(false);
        UGUIEventListener.Get(clearBtn).onClick = delegate(GameObject o)
        {
            foreach (var systemlog in systemlogs)
            {
                GameObject.Destroy(systemlog.gameObject);
            }
            systemlogs.Clear();
            curPosy = 0;
        };
    }
    
    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }

    private void OnAddMessage(Notification notification)
    {
        var item = GameObject.Instantiate(go);
        var log =item.GetComponent<Systemlog>();
        log.text.text= "系统： "+(string)notification.info;
        systemlogs.Add(log);
        if (systemlogs.Count > 100)
        {
            var top = systemlogs[0];
            systemlogs.RemoveAt(0);
            GameObject.Destroy(top.gameObject);
        }
        UnityTools.SetCenterParent(item.transform,this.transform);
        item.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,1);
        item.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1);
        item.GetComponent<RectTransform>().pivot = new Vector2(0.5f,1);
        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, curPosy);
        item.SetActive(true);
        var height = (int)Math.Ceiling(log.text.preferredHeight)+8;
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, -(curPosy - height));
        this.curPosy -= height;
        if (_rectTransform.sizeDelta.y > defaultYHeight)
        {
            _rectTransform.anchoredPosition =new Vector2(_rectTransform.anchoredPosition.x,_rectTransform.anchoredPosition.y+height);
        }
    }

    public static void PostSystemLog(string log)
    {
        NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,log);
    }
    public static void PostSystemRedLog(string log)
    {
        NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor( log,Color.red));
    }
    
}
