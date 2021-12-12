using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.Game;
using Script.Game.Grow;
using Script.Game.System;
using Script.Game.System.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class MapCtrl : MonoBehaviour
{
    public GameObject mapItemPrefab;
    
    public GameObject endBattleBtn;

    public RawImage islandImage;
    public GameObject mapMode;
    public GameObject battleMode;
    public GameObject biggerBtn;
    public GameObject smallBtn;
    public TileMapCopyCtrl tileMapCopyCtrl;
    public GameObject mudMapCtrl;
    public GameObject worldMap;
    public List<GameObject> islands;
    public GameObject startTip;
    public GameObject endTip;
    public GameObject winTip;
    public TMP_Text winText;
    public GameObject loseTip;
    public TMP_Text loseText;
    public GameObject tileModeResultBtn;
    public TMP_Text tileModeResultBtnText;
    List<MapItem> mapItems = new List<MapItem>();

    private void Awake()
    {
        mapMode.SetActive(true);
        worldMap.SetActive(false);
        battleMode.SetActive(false);
        foreach (var island in islands)
        {
            UGUIEventListener.Get(island).onClick = OnIsnland;
        }
        mapItemPrefab.SetActive(false);
        // endBattleBtn.SetActive(false);
        NotificationCenter.Default.AddObserver(this,OnFreshMap,(int)GameMessageId.FreshMap);
        NotificationCenter.Default.AddObserver(this,OnMudStartCopy,(int)GameMessageId.MudStartCopy);
        NotificationCenter.Default.AddObserver(this,OnMudEndCopy,(int)GameMessageId.MudEndCopy);
        NotificationCenter.Default.AddObserver(this,OnTileStartCopy,(int)GameMessageId.TileStartCopy);
        NotificationCenter.Default.AddObserver(this,OnTileEndCopy,(int)GameMessageId.TileEndCopy);
        NotificationCenter.Default.AddObserver(this,OnEndCopy,(int)GameMessageId.EndCopy);
        
        UGUIEventListener.Get(endBattleBtn).onClick = delegate(GameObject go)
        {
            GameSystem.Instance.currentWorld.ForceEndCopy();
        };
        UGUIEventListener.Get(biggerBtn).onClick = delegate(GameObject go)
        {
            worldMap.SetActive(true);
            mapMode.SetActive(false);
        };
        UGUIEventListener.Get(smallBtn).onClick = delegate(GameObject go)
        {
            worldMap.SetActive(false);
            mapMode.SetActive(true);
        };
    }

    private void OnIsnland(GameObject go)
    {
        int i = 0;
        foreach (var island in islands)
        {
            if (island == go)
            {
                if (GrowFun.Instance.growData.HasOpenIsland(i))
                {
                    worldMap.SetActive(false);
                    mapMode.SetActive(true);
                }
                else
                {
                    BoxManager.CreatePopTis("该区域暂未开放");
                    // islandImage.texture = ;
                }
                break;
            }

            i++;
        }
    }

    private void OnDestroy()
    {
        NotificationCenter.Default.RemoveObserver(this);
    }
    private void OnTileEndCopy(Notification notification)
    {
        var copyEventResult = (CopyEventResult) notification.info;
        endTip.SetActive(true);
        startTip.SetActive(false);
        // startTip.GetComponent<Animator>().Play("start_battle");
        
        string desc = "";
        if (copyEventResult.battleResult != null)
        {
            if (copyEventResult.battleResult.firstWin)
            {
                winTip.SetActive(true);
                loseTip.SetActive(false);
                winText.text = "探险成功！首次探险成功获得一份五彩碎片,当集齐20个五彩碎片时,可以前往商店兑换一颗五彩石！";
                tileModeResultBtnText.text = "返回";
            }
            else if (copyEventResult.battleResult.win)
            {
                winTip.SetActive(true);
                loseTip.SetActive(false);
                winText.text = "探险成功！";
                tileModeResultBtnText.text = "返回";
            }
            else
            {
                winTip.SetActive(false);
                loseTip.SetActive(true);
                loseText.text = "探险失败,提高能力后再来吧！";
                tileModeResultBtnText.text = "返回";
            }
            
            UGUIEventListener.Get(tileModeResultBtn).onClick = delegate(GameObject go)
            {
                OnTileResult();
            };
        }
        else
        {
            if (copyEventResult.enterNextLayer)
            {
                winTip.SetActive(true);
                loseTip.SetActive(false);
                winText.text = "本层胜利,前往下一层";
                tileModeResultBtnText.text = "确定";
                UGUIEventListener.Get(tileModeResultBtn).onClick = delegate(GameObject go)
                {
                    OnTileNext();
                };
            }
            else
            {
                // UGUIEventListener.Get(tileModeResultBtn).onClick = delegate(GameObject go)
                // {
                //     OnTileResult();
                // };
            }
        }

        // BoxManager.CreatOneButtonBox(desc,OnTileResult);
    }

    private void OnTileResult()
    {
        for (int i = 0; i < mapItems.Count; i++)
        {
            mapItems[i].gameObject.SetActive(true);
        }
        tileMapCopyCtrl.DestroyTile();
        tileMapCopyCtrl.gameObject.SetActive(false);
        mapMode.SetActive(true);
        battleMode.SetActive(false);
    }

    private void OnTileNext()
    {
        tileMapCopyCtrl.DestroyTile();
        GameSystem.Instance.currentWorld.StartCopyNextLayer();
    }

    private void OnTileStartCopy(Notification notification)
    {
        for (int i = 0; i < mapItems.Count; i++)
        {
            mapItems[i].gameObject.SetActive(false);
        }
        // endBattleBtn.SetActive(true);
        mapMode.SetActive(false);
        battleMode.SetActive(true);
        tileMapCopyCtrl.gameObject.SetActive(true);
        mudMapCtrl.gameObject.SetActive(false);
        tileMapCopyCtrl.CreateTile(GameSystem.Instance.currentWorld.curEntity);
        SystemlogCtrl.PostSystemLog("点击格子进行探索,探索后的区域才能继续前进,可能会遇到敌人或宝物,尽情探险吧！");
        endTip.SetActive(false);
        startTip.SetActive(true);
        startTip.GetComponent<Animation>().Play("start_battle");
    }

    private void OnMudEndCopy(Notification notification)
    {
        for (int i = 0; i < mapItems.Count; i++)
        {
            mapItems[i].gameObject.SetActive(true);
        }
        mapMode.SetActive(true);
        battleMode.SetActive(false);
        tileMapCopyCtrl.gameObject.SetActive(false);
        mudMapCtrl.gameObject.SetActive(false);
    }

    private void OnEndCopy(Notification notification)
    {
        OnMudEndCopy(notification);
    }

    private void OnMudStartCopy(Notification notification)
    {
        for (int i = 0; i < mapItems.Count; i++)
        {
            mapItems[i].gameObject.SetActive(false);
        }
        // endBattleBtn.SetActive(true);
        mapMode.SetActive(false);
        battleMode.SetActive(true);
        tileMapCopyCtrl.gameObject.SetActive(false);
        mudMapCtrl.gameObject.SetActive(true);
    }

    private void OnFreshMap(Notification notification)
    {
        RandomMapItems(GameSystem.Instance.currentWorld.copyEntities);
        // endBattleBtn.SetActive(false);
        mapMode.SetActive(true);
        battleMode.SetActive(false);
    }

    public void RandomMapItems(List<CopyEntity> copyEntities)
    {
        ClearMapItems();
        for (int i = 0; i < copyEntities.Count; i++)
        {
            var item = RandomMapItem(i);
            item.Set(i,copyEntities[i]);
        }
    }

    private void ClearMapItems()
    {
        for (int i = 0; i < mapItems.Count; i++)
        {
            GameObject.Destroy(mapItems[i].gameObject);
        }
        mapItems.Clear();
    }


    public MapItem RandomMapItem(int copyIndex)
    {
        int x = 0;
        int y = 0;
        int loopCount = 1;
        do
        {
            x = GrowFun.Instance.randomUtil.Range(-217, 217);
            y = GrowFun.Instance.randomUtil.Range(-140, 217);
        } while (!CheckPos(x, y,loopCount++));

        var item = GameObject.Instantiate(mapItemPrefab);
        UnityTools.SetCenterParent(item.transform,mapMode.transform);
        item.SetActive(true);
        item.GetComponent<RectTransform>().anchoredPosition = new Vector2(x,y);
        var mapItemMono = item.GetComponent<MapItem>();
        mapItems.Add(mapItemMono);
        return mapItemMono;
    }

    private bool CheckPos(int x, int y,int loop)
    {
        foreach (var mapItem in mapItems)
        {
            var xy =mapItem.GetComponent<RectTransform>().anchoredPosition;
            var disx = xy.x - x;
            var disy = xy.y - y;
            if (Mathf.Abs(disx) < 30/loop)
            {
                return false; 
            }
            if (Mathf.Abs(disy) < 40/loop)
            {
                return false; 
            }
        }

        return true;
    }

    public void DoClose()
    {
        tileMapCopyCtrl.DestroyTile();
    }
}
