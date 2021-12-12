using System.Collections.Generic;
using Script.Game.Grow;
using TextEquip.System;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    public class MudCopyEntityImp:CopyEntityImp
    {
         int curEventIndex = -1;
         bool repeated = false;
        CopyEventEntity curEvent
        {
            get
            {
                return eventEntities[curEventIndex];
            }
        }

        public MudCopyEntityImp(CopyEntity _copyEntity)
        {
            this.copyEntity = _copyEntity;
        }
        
        public override void StartCopy(bool repeated,int layer)
        {
            this.curEventIndex = -1;
            this.repeated = repeated;
            foreach (var eventConfig in copyEntity.config.eventConfigs)
            {
                var eventEntity = new CopyEventEntity();
                eventEntity.Init(copyEntity,eventConfig);
                this.eventEntities.Add(eventEntity);
            }

            foreach (var eventEntity in this.eventEntities)
            {
                eventEntity.Active();
            }
            StartNextAction();
            NotificationCenter.Default.PostNotification((int)GameMessageId.MudStartCopy);
        }

        public override  void Tick()
        {
            if (curEventIndex >= 0)
            {
                curEvent.ActionTick();
                if (curEvent.IsActionDone())
                {
                    if (ContinueNext())
                    {
                        if (curEventIndex == eventEntities.Count - 1)
                        {
                            if (repeated)
                            {
                                EndOnceAndStartNextAction();
                            }
                            else
                            {
                                InnerEnd();
                            }
                        }
                        else
                        {
                            StartNextAction();
                        }
                    }
                    else
                    {
                        InnerEnd();
                    }
                }
            }
        }


        private bool ContinueNext()
        {
            if (GrowFun.Instance.growData.growEquips.Count >= WorldConfig.MaxBagCount-1)
            {
                return false;
            }

            return curEvent.ContinueNext();
        }

        private void EndOnceAndStartNextAction()
        {
            NotificationCenter.Default.PostNotification((int)GameMessageId.LoopOnceCopy);
            GrowFun.Instance.SaveData();
            StartNextAction();
        }


        private void StartNextAction()
        {
            if (curEventIndex >= 0)
            {
                curEvent.ActionEnd();
            }
            curEventIndex++;
            curEventIndex = curEventIndex % eventEntities.Count;
            curEvent.ActionStart();
        }


        public override void EndCopy()
        {
            Clear();
            NotificationCenter.Default.PostNotification((int)GameMessageId.EndCopy);
        }

        void InnerEnd()
        {
            Clear();
            GrowFun.Instance.SaveData();
            NotificationCenter.Default.PostNotification((int)GameMessageId.MudEndCopy);
        }

        void Clear()
        {
            if (curEventIndex >= 0)
            {
                curEvent.ActionEnd();
            }
            curEventIndex = -1;
            foreach (var eventEntity in this.eventEntities)
            {
                eventEntity.DeActive();
            }
            this.eventEntities.Clear();
        }
        

        public override bool IsMudMode()
        {
            return true;
        }
    }
}