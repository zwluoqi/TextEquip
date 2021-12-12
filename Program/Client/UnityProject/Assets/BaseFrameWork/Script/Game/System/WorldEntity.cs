using System;
using System.Collections.Generic;
using Script.Game.Grow;
using Script.Game.System.Entity;
using TextEquip.System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Script.Game.System
{
    public class WorldEntity
    {
        public RandomUtil randomUtil;
        public WorldConfig config;
        public PlayerEntity player;
        public List<CopyEntity> copyEntities = new List<CopyEntity>();
        public Dictionary<string,CopyEntity> dictionaryEntitys = new Dictionary<string, CopyEntity>();
        public CopyEntity curEntity;

        public TimeEventManager timeEventManager;
        public double deltaTime;
        public void Init(WorldConfig worldConfig)
        {
            this.randomUtil = new RandomUtil();
            this.randomUtil.SetSeed(DateTime.Now.Ticks);
            this.timeEventManager = new TimeEventManager();
            this.config = worldConfig;
            foreach (var copy in this.config.copyList)
            {
                var copyEntity = new CopyEntity();
                copyEntity.Init(this,copy);
                this.copyEntities.Add(copyEntity);
            }
            this.player = new PlayerEntity();
            this.player.Init(this,GrowFun.Instance.growData.growPlayer);
        }

        public void Start()
        {
            //TODO
            this.timeEventManager.Start();
            this.player.Start();
        }


        public void Tick()
        {
            deltaTime = Time.deltaTime;
            this.timeEventManager.OrderUpdate(deltaTime);
            if (curEntity != null)
            {
                curEntity.Tick();
            }
        }

        public void End()
        {
            this.player.End();
            if (curEntity != null)
            {
                curEntity.ForceEndCopy();
            }
            this.timeEventManager.Stop();
        }
        

        private bool CheckStartCondition()
        {
            if (GrowFun.Instance.growData.growEquips.Count >= WorldConfig.MaxBagCount-1)
            {
                SystemlogCtrl.PostSystemLog("背包空间即将满载，请及时整理背包");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="index"></param>
        /// <param name="r"></param>
        public void StartCopy(int index,bool mud)
        {
            if (!CheckStartCondition())
            {
                return;
            }
            if (curEntity != null)
            {
                curEntity.ForceEndCopy();
            }
            curEntity = copyEntities[index]; 
            curEntity.StartCopy(mud);
        }
        
        /// <summary>
        /// 结束当前副本
        /// </summary>
        public void ForceEndCopy()
        {
            if (curEntity != null)
            {
                curEntity.ForceEndCopy();
                curEntity = null;
            }
        }

        public void StartCopyNextLayer()
        {
            if (curEntity != null)
            {
                curEntity.StartCopyNextLayer();
            }
        }
    }

}