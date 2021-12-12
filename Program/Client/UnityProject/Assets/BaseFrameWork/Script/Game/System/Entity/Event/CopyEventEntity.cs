using System;
using System.Collections.Generic;
using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using TextEquip.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
  public class CopyEventEntity : BaseEntity
  {
    public CopyEntity _copyEntity;
    public CopyEventConfig config;
    public Action<CopyEventEntity> onActionDone;

    protected CopyEventEntityImp _imp;
    
    public void Init(CopyEntity copyEntity, CopyEventConfig eventConfig)
    {
      this._copyEntity = copyEntity;
      this.world = copyEntity.world;
      
      this.config = eventConfig;
      this._imp = CopyEventEntityImp.CreateEntity(eventConfig);
      this._imp.Init(this);
    }

    public void ChangeConfig( CopyEventConfig eventConfig)
    {
      this._imp.ActionEnd();
      this._imp.DeActive();
      
      this.config = eventConfig;
      this._imp = CopyEventEntityImp.CreateEntity(eventConfig);
      this._imp.Init(this,CopyEventEntityImp.EventState.Opened);
      this._imp.Active();
      NotificationCenter.Default.PostNotification((int)GameMessageId.OnCopyEventEntityChangeConfig,config.posIndex);
    }
    
    public bool operationNear
    {
      get
      {
        return _imp.operationNear;
      }
    }

    public CopyEventEntityImp.EventState eventState
    {
      get
      {
        return _imp.eventState;
      }
    }


    public void OnImpActionDone()
    {
      if (this.onActionDone != null)
      {
        this.onActionDone(this);
      }
    }

    public CopyEventResult GetResult()
    {
      return _imp.GetResult();
    }
    
    public void Active()
    {
      _imp.Active();
    }



    public void ActionStart()
    {
      _imp.ActionStart();
    }

    public void Open()
    {
      _imp.Open();
    }

    public void ActionEnd()
    {
      _imp.ActionEnd();
    }

    public void DeActive()
    {
      _imp.DeActive();
    }

    public void ActionTick()
    {
      _imp.ActionTick();
    }

    public bool IsActionDone()
    {
      return _imp.IsActionDone();
    }

    public bool ContinueNext()
    {
      return _imp.ContinueNext();
    }

    public bool CheckCanOperation()
    {
      return _imp.CheckCanOperation();
    }

    public bool CheckLocked()
    {
      return _imp.CheckLocked();
    }
    
  }
}