using System;
using System.Collections.Generic;
using Script.Game;
using Script.Game.System;
using Script.Game.System.Entity;
using TextEquip.System;
using UnityEngine;
using XZXD.UI;

public class TileMapCopyCtrl:MonoBehaviour
{
        public TileMapItem tileItemPrefab;

        public UnityEngine.UI.GridLayoutGroup grid;

        // public PlayerTileMapItem hero;
        public CopyEntity cutEntity;
        List<TileMapItem> tiles = new List<TileMapItem>();

        private void Awake()
        {
            tileItemPrefab.gameObject.SetActive(false);
            NotificationCenter.Default.AddObserver(this,OnCopyEventEntityStateChange,(int)GameMessageId.BattleCopyEntityStateChange);
        }

        private void OnCopyEventEntityStateChange(Notification notification)
        {
            foreach (var tile in tiles)
            {
                tile.FreshLock();
            }
        }

        public void CreateTile(CopyEntity copyEntity)
        {
            this.cutEntity = copyEntity;
            DestroyTile();
            foreach (var copyEventEntity in copyEntity.copyEntityImp.GetEventEntity())
            {
                var tileMapItem =  GameObject.Instantiate(tileItemPrefab);
                UnityTools.SetCenterParent(tileMapItem.transform,grid.transform);
                tileMapItem.Init(copyEventEntity);
                tileMapItem.gameObject.SetActive(true);
                tiles.Add(tileMapItem);
                UGUIEventListener.Get(tileMapItem.gameObject).onClick = OnClickMapItem;
            }

            // hero.SetInit(this,copyEntity);
        }


        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.W))
        //     {
        //         OnClickMapItem(tiles[hero.curPos + 1].gameObject);
        //     }
        // }

        private void OnClickMapItem(GameObject go)
        {
            {
                var tileMapItem = go.GetComponent<TileMapItem>();
                // if (tileMapItem._copyEventEntity.canMove)
                // {
                //     if (hero.CanMove(tileMapItem._copyEventEntity.config.posIndex))
                //     {
                //         hero.MoveToByPath(null);
                //     }
                //     else
                //     {
                //         SystemlogCtrl.PostSystemLog(RichTextUtil.AddColor( "请点击就近位置进行移动",Color.red));
                //     }
                // }
                // else
                {
                    if (tileMapItem._copyEventEntity.eventState == CopyEventEntityImp.EventState.Opened)
                    {
                        // if (WorldConfigAPI.IsNearBy(hero.curPos, tileMapItem._copyEventEntity.config.posIndex))
                        // {
                        //     cutEntity.copyEntityImp.StartIndexAction(tileMapItem._copyEventEntity.config.posIndex);
                        // }
                        // else
                        // {
                        //     //不在附近的要走到附近,在附近找一个能移动的点
                        //     List<int> ress = WorldConfigAPI.GetNearByIndex( cutEntity.config.maxIndex, tileMapItem._copyEventEntity.config.posIndex);
                        //     int canMoveIndex = -1;
                        //     foreach (var ret in ress)
                        //     {
                        //         if (cutEntity.copyEntityImp.GetEventEntity()[ret].canMove)
                        //         {
                        //             canMoveIndex = ret;
                        //             break;
                        //         }
                        //     }
                        //
                        //     //有可以移动的点先移动过去
                        //     if (canMoveIndex >= 0)
                        //     {
                        //         if (hero.CanMove(canMoveIndex))
                        //         {
                        //             hero.MoveToByPath(delegate()
                        //             {
                        //                 if (WorldConfigAPI.IsNearBy(hero.curPos, tileMapItem._copyEventEntity.config.posIndex))
                        //                 {
                        //                     cutEntity.copyEntityImp.StartIndexAction(tileMapItem._copyEventEntity.config.posIndex);
                        //                 }
                        //             });
                        //         }
                        //     }
                        //     else
                        //     {
                        //         //没有可以移动的点,不做处理
                        //     }
                        // }
                        if (tileMapItem._copyEventEntity.CheckCanOperation()){
                            cutEntity.copyEntityImp.StartIndexAction(tileMapItem._copyEventEntity.config.posIndex);
                        }
                    }
                    else if (tileMapItem._copyEventEntity.eventState == CopyEventEntityImp.EventState.None)
                    {
                        // var curCopy = GameSystem.Instance.currentWorld.curEntity;
                        // List<int> ress = WorldConfigAPI.GetNearByIndex( curCopy.config.maxIndex, tileMapItem._copyEventEntity.config.posIndex);
                        // bool canOpen = false;
                        // foreach (var ret in ress)
                        // {
                        //     if (curCopy.copyEntityImp.GetEventEntity()[ret].canMove)
                        //     {
                        //         canOpen = true;
                        //         break;
                        //     }
                        // }
                        //
                        // if (canOpen)
                        // {
                        //     cutEntity.copyEntityImp.OpenIndexAction(tileMapItem._copyEventEntity.config.posIndex);
                        // }
                        // else
                        // {
                        //     SystemlogCtrl.PostSystemLog("需要先处理周围格子后再开启");
                        // }
                        if (tileMapItem._copyEventEntity.CheckCanOperation())
                        {
                            cutEntity.copyEntityImp.OpenIndexAction(tileMapItem._copyEventEntity.config.posIndex);
                        }
                    }
                    else
                    {
                        Debug.LogError("不支持"+tileMapItem._copyEventEntity.config.posIndex+" state:"+tileMapItem._copyEventEntity.eventState);
                    }
                }
            };
        }


        public void DestroyTile()
        {
            foreach (var tile in tiles)
            {
                GameObject.Destroy(tile.gameObject);
            }
            tiles.Clear();
            // hero.Clear();
        }

        public TileMapItem GetItemByIndex(int curPos)
        {
            return tiles[curPos];
        }
}