using System;
using System.Collections.Generic;
using Script.Game.Grow;
using TextEquip.System;
using UnityEngine;

namespace Script.Game.System.Entity
{
    public class CopyEventEntityImp:BaseEntity
    {
        protected CopyEventEntity eventEntity;
        
        private EventState _eventState;
        private TimeEventHandler _eventActionHandler;
        private TimeEventHandler _eventOpenHandler;

        public bool operationNear = false;
        protected CopyEventResult result = new CopyEventResult();


        public CopyEventConfig config
        {
          get
          {
            return eventEntity.config;
          }
        }

        protected CopyEntity _copyEntity
        {
          get
          {
            return eventEntity._copyEntity;
          }
        }
        
        public EventState eventState
        {
          get
          {
            return _eventState;
          }
          set
          {
            _eventState = value;
            NotificationCenter.Default.PostNotification((int)GameMessageId.BattleCopyEntityStateChange,config.posIndex);
          }
        }
        public enum EventState
        {
          None,
          Opening,
          Opened,
          Actioning,
          ActionDone,
        }
        
        public void Init(CopyEventEntity _eventEntity,EventState initState = EventState.None)
        {
          this.eventEntity = _eventEntity;
          // this._copyEntity = copyEntity;
          this.world = this.eventEntity.world;

          this._eventState = initState;
          // this.config = config;
          _OnInit();
        }

        protected virtual void _OnInit()
        {
          
        }


        public void ActionStart()
        {
          if (!CheckCanOperation())
          {
            return;
          }
          this.world.player.FreshData();
          this._OnActionStart();
          _eventActionHandler = world.timeEventManager.CreateEvent(OnActionTimeDone,this.config.costTime);
          this.eventState = EventState.Actioning;
        }

        private void OnActionTimeDone()
        {
          _OnActionTimeDone();
          eventState = EventState.ActionDone;
          eventEntity.OnImpActionDone();
        }

        protected virtual void _OnActionTimeDone()
        {
          
        }

        public virtual void _OnActionStart()
        {
          
        }

        public virtual void ActionTick()
        {
          
        }

        public void ActionEnd()
        {
          this._OnActionEnd();
          // this.eventState = EventState.None;
          TimeEventManager.Delete(ref _eventActionHandler);
          TimeEventManager.Delete(ref _eventOpenHandler);
          // NotificationCenter.Default.PostNotification((int)GameMessageId.EndCopyEvent);
        }

        protected virtual void _OnActionEnd()
        {
          
        }



        public virtual void Active()
        {
          
        }

        public virtual void DeActive()
        {
          
        }

        public virtual bool IsActionDone()
        {
          return this.eventState == EventState.ActionDone;

        }


        public virtual bool ContinueNext()
        {
          return true;
        }

        public void Open()
        {
          if (!CheckCanOperation())
          {
            return;
          }
          _OnOpen();
          _eventOpenHandler = world.timeEventManager.CreateEvent(OnOpenTimeDone,this.config.openCostTime);
          eventState = EventState.Opening;
        }

        public virtual bool CheckLocked()
        {
          return !CheckCanOperation();
        }

        public virtual bool CheckCanOperation()
        {
          //周围如果任何一个非单位开启，则自己可以开启
          //周围任何一个战斗单位未开启，则自己不可以开启
          var nearByIndex = WorldConfigAPI.GetNearByIndex(_copyEntity.config.maxIndex, config.posIndex);
          var list = _copyEntity.copyEntityImp.GetEventEntity();
          
          if (this.config.eventType != "battle")
          {
            foreach (var nearIndex in nearByIndex)
            {
              var near = list[nearIndex];
              if (!near.operationNear && near.config.eventType == "battle" &&
                  (near.eventState == EventState.Opened || near.eventState == EventState.Actioning
                  )
              )
              {
                return false;
              }
            }
          }

          foreach (var nearIndex in nearByIndex)
          {
            var near = list[nearIndex];
            if (near.operationNear)
            {
              return true;
            }
          }
          return operationNear;
        }

        private void OnOpenTimeDone()
        {
          _OnOpenTimeDone();
          eventState = EventState.Opened;
        }

        protected virtual void _OnOpenTimeDone()
        {
          
        }

        protected virtual void _OnOpen()
        {
          
        }

        public CopyEventResult GetResult()
        {
          return result;
        }

        //战利品计算
    public CopyEventDropResult CaculateTrophy()
    {
      CopyEventDropResult result = new CopyEventDropResult();
      List<GrowEquip> drops = result.drops;
      var lv = this._copyEntity.config.lv;
      // 获取独特装备
      if (this.config.type == "boss" && this.config.type != "endless")
      {
        var randow = 1 - 0.02 * ((this._copyEntity.config.difficulty - 1) * 2 + 1);
        if (world.randomUtil.value > randow)
        {
          var random = world.randomUtil.value;
          if (random <= 0.3 && random > 0)
          {
            var item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.weapon, 14,
              (int) Math.Floor(lv + world.randomUtil.value * 6));
            drops.Add(item);
          }
          else if (random <= 0.5 && random > 0.3)
          {
            var item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.dress, 14,
              (int) Math.Floor(lv + world.randomUtil.value * 6));
            drops.Add(item);
          }
          else if (random <= 0.75 && random > 0.5)
          {
            var item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.necklace, 14,
              (int) Math.Floor(lv + world.randomUtil.value * 6));
            drops.Add(item);
          }
          else
          {
            var item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.ring, 14,
              (int) Math.Floor(lv + world.randomUtil.value * 6));
            drops.Add(item);
          }
        }
      }

      var trophy = this.config.copyEventDropConfig;
      // var equipTypeRadios = new double[]
      // {
      //   0.25, 0.25, 0.25, 0.25
      // };
      var equip = trophy.equipDropRadios;
      var equipQua = -1;
      var r = world.randomUtil.value;
      if (r <= equip[0])
      {
        // 获得破旧装备
        equipQua = 10;
      }
      else if (r < equip[1] + equip[0] && r >= equip[0])
      {
        // 获得普通装备
        equipQua = 11;
      }
      else if (r < equip[2] + equip[1] + equip[0] && r >= equip[1] + equip[0])
      {
        // 获得神器装备
        equipQua = 12;
      }
      else if (r < equip[3] + equip[2] + equip[1] + equip[0] && r >= equip[2] + equip[1] + equip[0])
      {
        // 获得史诗装备
        equipQua = 13;
      }
      else
      {
        // 未获得装备
      }
      
      //

      //获得装备时
      if (equipQua != -1)
      {
        // this.createEquip(equipQua,lv)
        var index = (int) Math.Floor((world.randomUtil.value * 4));
        GrowEquip item = null;
        if (index == 0)
        {
          item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.weapon, equipQua, lv);
          drops.Add(item);
        }
        else if (index == 1)
        {
          item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.dress, equipQua, lv);
          drops.Add(item);
        }
        else if (index == 2)
        {
          item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.necklace, equipQua, lv);
          drops.Add(item);
        }
        else
        {
          item = GrowFun.Instance.growData.CreateNewEquip(DictEquipEquipTypeEnum.ring, equipQua, lv);
          drops.Add(item);
        }

        //自动售卖
        if (GrowFun.Instance.growData.autoCostEquips[equipQua-10])
        {
          GrowFun.Instance.growData.CostEquip(item,true);
          drops.Remove(item);
        }
      }
      
      
      // var backpackPanel = this.findBrothersComponents(this, 'backpackPanel', false)[0]
      double goldObtainRatio = 1;
      var coin = (int) (this.config.copyEventDropConfig.gold * goldObtainRatio);
      GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin,
        coin);
      NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor( string.Format("获得了{0}金币",coin),Color.green));
      result.gold = coin;

      return result;
    }
    
    
        
        public static CopyEventEntityImp CreateEntity(CopyEventConfig eventConfig)
        {
          CopyEventEntityImp copyEventEntity = null;
          switch (eventConfig.eventType)
          {
            case "battle":
              copyEventEntity = new CopyEventBattleEntityImp();
              break;
            // case "story":
              // copyEventEntity = new CopyEventStoryEntity();
              // break;
            case "gift":
              copyEventEntity = new CopyEventGiftEntityImp();
              break;
            case "empty":
              copyEventEntity = new CopyEventEmptyEntityImp();
              break;
            case "blood":
              copyEventEntity = new CopyEventBloodEntityImp();
              break;
            case "door":
              copyEventEntity = new CopyEventDoorEntityImp();
              break;
            default:
              throw new Exception("不支持的时间类型");
              break;
          }

          return copyEventEntity;
        }

        
    }
}