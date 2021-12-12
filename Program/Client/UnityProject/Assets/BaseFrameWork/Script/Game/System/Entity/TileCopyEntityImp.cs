using System.Collections.Generic;
using Script.Game.Grow;
using TextEquip.System;
using UnityEditor;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    public class TileCopyEntityImp:CopyEntityImp
    {
         // int curEventIndex = -1;

        //  CopyEventEntity curEvent
        // {
        //     get
        //     {
        //         return eventEntities[curEventIndex];
        //     }
        // }

        public TileCopyEntityImp(CopyEntity _copyEntity)
        {
            this.copyEntity = _copyEntity;
        }
        
        public override void StartCopy(bool repeated,int layer)
        {
            // this.curEventIndex = -1;
            WorldConfigAPI.Instance.GenerateGameEventConfigs(copyEntity.config,layer);
            foreach (var eventConfig in copyEntity.config.gameEventConfigs)
            {
                var eventEntity = new CopyEventEntity();
                eventEntity.Init(copyEntity,eventConfig);
                eventEntity.onActionDone = OnActionDone;
                this.eventEntities.Add(eventEntity);
            }

            foreach (var eventEntity in this.eventEntities)
            {
                eventEntity.Active();
            }

            NotificationCenter.Default.PostNotification((int)GameMessageId.TileStartCopy);
        }

        private bool copyOver = false;
        private CopyEventResult _result;
        private void OnActionDone(CopyEventEntity entity)
        {
            if (entity.GetResult().battleResult != null)
            {
                if (entity.GetResult().battleResult.win && entity.config.isLastBoss)
                {
                    _result = entity.GetResult();
                    copyOver = true;
                    return;
                }
                else if(!entity.GetResult().battleResult.win)
                {
                    _result = entity.GetResult();
                    copyOver = true;
                    return;
                } 
            }
            
            if (entity.config.nextConfig != null)
            {
                entity.ChangeConfig(entity.config.nextConfig);
                return;
            }

            if (entity.config.eventType == "door")
            {
                _result = entity.GetResult();
                copyOver = true; 
            }
        }

        public override  void Tick()
        {
            if (copyOver)
            {
                copyOver = false;
                EndWithCopyResult(_result);
            }
        }
        

        public override void StartIndexAction(int index)
        {
            // if (curEventIndex >= 0)
            // {
            //     curEvent.ActionEnd();
            // }
            // curEventIndex = index;
            eventEntities[index].ActionStart();
        }
        public override void OpenIndexAction(int index)
        {
            eventEntities[index].Open();
        }


        public override void EndCopy()
        {
            Clear();
            NotificationCenter.Default.PostNotification((int)GameMessageId.EndCopy);
        }

        void EndWithCopyResult(CopyEventResult result)
        {
            Clear();
            if (result.battleResult != null)
            {
                if (result.battleResult.firstWin)
                {
                    GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.wucai_suipian, 1);
                }
            }

            GrowFun.Instance.SaveData();
            NotificationCenter.Default.PostNotification((int)GameMessageId.TileEndCopy,result);
        }


        void Clear()
        {
            // if (curEventIndex >= 0)
            // {
            //     curEvent.ActionEnd();
            // }
            // curEventIndex = -1;
            foreach (var eventEntity in this.eventEntities)
            {
                eventEntity.ActionEnd();
            }
            foreach (var eventEntity in this.eventEntities)
            {
                eventEntity.DeActive();
            }
            this.eventEntities.Clear();
        }
    }
}