using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.Game;
using Script.Game.Grow;
using Script.Game.System;
using Script.Game.System.Entity;
using TextEquip.System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTileMapItem:MonoBehaviour
{

    public GameObject hero;
    public int curPos;
    public int targetPos;
    // public MoveState moveState;

    public RectTransform _RectTransform;
    public Slider hp;
//     private CopyEntity copyEntity;
//      TileMapCopyCtrl tileMapCopyCtrl;
//     private void Awake()
//     {
//         _RectTransform = this.GetComponent<RectTransform>();
//         NotificationCenter.Default.AddObserver(this,OnBattleCopyEntityStateChange,(int)GameMessageId.BattleCopyEntityStateChange);
//     }
//
//     private void OnDestroy()
//     {
//         NotificationCenter.Default.RemoveObserver(this);
//     }
//
//     private void OnBattleCopyEntityStateChange(Notification notification)
//     {
//         hp.value = (float)GrowFun.Instance.growData.growPlayer.hpPercent;
//     }
//
//     
//     public enum  MoveState
//     {
//         Idle,
//         Moveing,
//     }
//     
//     
//     /// <summary>
//     /// moveDir 0上，1下，2左，3右
//     /// </summary>
//     /// <param name="moveToTargetPos"></param>
//     /// <param name="moveDir"></param>
//     /// <returns></returns>
//     public bool CanMove(int moveToTargetPos)
//     {
//         if (targetPos == moveToTargetPos)
//         {
//             SystemlogCtrl.PostSystemRedLog("正在前往该位置");
//             return false;
//         }
//
//         if (curPos == moveToTargetPos)
//         {
//             return false;
//         }
//
//         // return IsNearBy(moveToTargetPos);
//         roadPath.Clear();
//         bool succrss = GetRoadPath(ref roadPath, curPos, moveToTargetPos);
//         return succrss;
//     }
//     
//     List<int> roadPath = new List<int>();
//     
//     public bool GetRoadPath(ref List<int> path,int sourcePath,int targetPath)
//     {
//         if (sourcePath == targetPath)
//         {
//             return true;
//         }
//         else
//         {
//             var nearByIndexs = WorldConfigAPI.GetNearByIndex(copyEntity.config.maxIndex,sourcePath);
//             var tarCol = copyEntity.copyEntityImp.GetEventEntity()[targetPath].config.curCol;
//             var tarRow = copyEntity.copyEntityImp.GetEventEntity()[targetPath].config.curRow;
//             
//             //首先排序，
//             nearByIndexs.Sort(delegate(int x, int y)
//             {
//                 var colx = copyEntity.copyEntityImp.GetEventEntity()[x].config.curCol;
//                 var rowx = copyEntity.copyEntityImp.GetEventEntity()[x].config.curRow;
//
//                 var spacex = Math.Abs(tarCol - colx) + Math.Abs(tarRow - rowx);
//                 
//                 var coly = copyEntity.copyEntityImp.GetEventEntity()[y].config.curCol;
//                 var rowy = copyEntity.copyEntityImp.GetEventEntity()[y].config.curRow;
//                 var spacey = Math.Abs(tarCol - coly) + Math.Abs(tarRow - rowy);
//                 return spacex.CompareTo(spacey);
//             });
//             
//             foreach (var index in nearByIndexs)
//             {
//                 //点已经在路径中，说明准备返回，所以没必要，直接false
//                 if (path.Contains(index))
//                 {
//                     return false;
//                 }
//                 
//                 //点可以移动，添加到路径，递归
//                 if (copyEntity.copyEntityImp.GetEventEntity()[index].canMove)
//                 {
//                     path.Add(index);
//                     bool success = GetRoadPath(ref path, index, targetPath);
//                     if (success)
//                     {
//                         return true;
//                     }
//                 }
//             }
//
//             return false;
//         }
//     }
//
//
//     // public bool CanOpen(int moveToTargetPos)
//     // {
//     //     var curCopy = GameSystem.Instance.currentWorld.curEntity;
//     //     List<int> ress = WorldConfigAPI.GetNearByIndex( curCopy.config.maxIndex, moveToTargetPos);
//     //     foreach (var ret in ress)
//     //     {
//     //         if (!curCopy.copyEntityImp.GetEventEntity()[ret].canMove)
//     //         {
//     //             return false;
//     //         }
//     //     }
//     //
//     //     return true;
//     // }
//
//     
//
//    
//
//     public void SetInit(TileMapCopyCtrl tileMapCopyCtrl,CopyEntity copyEntity)
//     {
//         hp.value = (float)GrowFun.Instance.growData.growPlayer.hpPercent;
//
//         this.tileMapCopyCtrl = tileMapCopyCtrl;
//         this.copyEntity = copyEntity;
//         curPos = copyEntity.config.heroPosIndex;
//         hero.GetComponent<Animator>().Play("player_idle");
//         moveState = MoveState.Idle;
//         
//         var row =  this.copyEntity.copyEntityImp.GetEventEntity()[curPos].config.curRow;
//         var col =  this.copyEntity.copyEntityImp.GetEventEntity()[curPos].config.curCol;
//         var x = (tileMapCopyCtrl.grid.cellSize.x + tileMapCopyCtrl.grid.spacing.x) * (col+0.5f);
//         var y = -(tileMapCopyCtrl.grid.cellSize.y + tileMapCopyCtrl.grid.spacing.y) * row - 60;
//         _RectTransform.anchoredPosition = new Vector2(x,y);
//     }
//
//     public void Clear()
//     {
//         TimeEventManager.Delete(ref _event);
//     }
//
//
//     public float lastStartMoveTime;
//     void MoveTo(TileMapItem tileMapItem)
//     {
//         targetPos = tileMapItem._copyEventEntity.config.posIndex;
//         moveState = MoveState.Moveing;
//         var x = targetPos - curPos;
//         if (x == 1)
//         {
//             // moveDir = 3;
//             hero.GetComponent<Animator>().Play("player_move_r");
//         }
//         if (x == -1)
//         {
//             // moveDir = 2;
//             hero.GetComponent<Animator>().Play("player_move_l");
//         }
//         if (x == CopyEventConfig.MaxCol)
//         {
//             // moveDir = 0;
//             hero.GetComponent<Animator>().Play("player_move_d");
//         }
//         if (x == -CopyEventConfig.MaxCol)
//         {
//             // moveDir = 1;
//             hero.GetComponent<Animator>().Play("player_move_t");
//         }
//
//         var row =  this.copyEntity.copyEntityImp.GetEventEntity()[targetPos].config.curRow;
//         var col =  this.copyEntity.copyEntityImp.GetEventEntity()[targetPos].config.curCol;
//         var anchorX = (tileMapCopyCtrl.grid.cellSize.x + tileMapCopyCtrl.grid.spacing.x) * (col+0.5f);
//         var anchorY = -(tileMapCopyCtrl.grid.cellSize.y + tileMapCopyCtrl.grid.spacing.y) * row - 60;
//
//         var spaceX = anchorX - _RectTransform.anchoredPosition.x;
//         var spacey = anchorY - _RectTransform.anchoredPosition.y;
//         var per = (Math.Abs(spaceX) + Math.Abs(spacey)) /
//                   (tileMapCopyCtrl.grid.cellSize.x + tileMapCopyCtrl.grid.spacing.x);
//         this.transform.DOMove(tileMapItem.transform.position, per);
//
//         _event = GameSystem.Instance.timeeventManager.CreateEvent(OnMoveDone, per);
//     }
//
//     private void OnMoveDone()
//     {
//         moveState = MoveState.Idle;
//         curPos = targetPos;
//         InnerMoveToByPath();
//     }
//
//
//     public TimeEventHandler _event;
//
//     void InnerMoveToByPath()
//     {
//         if (roadPath.Count > 0)
//         {
//             var next = roadPath[0];
//             roadPath.RemoveAt(0);
//             var item = tileMapCopyCtrl.GetItemByIndex(next);
//             MoveTo(item);
//         }
//         else
//         {
//             
//             hero.GetComponent<Animator>().Play("player_idle");
// #if UNITY_EDITOR
//             Debug.Log("没有路径了");
// #endif
//             if (onMoveDoneByPath != null)
//             {
//                 var tmp = onMoveDoneByPath;
//                 onMoveDoneByPath = null;
//                 tmp();
//             }
//         }
//     }
//
//     private Action onMoveDoneByPath;
//     public void MoveToByPath(Action moveDone)
//     {
//         this.onMoveDoneByPath = moveDone;
//         TimeEventManager.Delete(ref _event);
//         InnerMoveToByPath();
//     }
    
}