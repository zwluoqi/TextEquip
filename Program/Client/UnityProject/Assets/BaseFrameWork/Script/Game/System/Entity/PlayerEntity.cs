using System;
using Script.Game.Grow;
using TextEquip.System;
using UnityEngine;

namespace Script.Game.System.Entity
{
    public class PlayerEntity:BaseEntity
    {
        public double[] attribute = new double[WorldConfig.MaxAttribute];
        private TimeEventHandler _eventHandler;
        public void FreshData()
        {
            var player = GrowFun.Instance.growData.growPlayer;
            Array.Copy(player.GetAttribute(), attribute,attribute.Length);
            attribute[(int) DictAbilityPropEnum.HP] = player.hpPercent * attribute[(int) DictAbilityPropEnum.MAX_HP];
        }

        public void Init(WorldEntity worldEntity,GrowPlayer growDataGrowPlayer)
        {
            this.world = worldEntity;
            FreshData();
        }

        public void Start()
        {
            _eventHandler =
                world.timeEventManager.CreateEvent(OnRecoverHP, 1, 1);
        }

        private void OnRecoverHP()
        {
            if (world.curEntity == null)
            {
                RecoverHp();
            }
            else
            {
                if (world.curEntity.copyEntityImp.IsMudMode())
                {
                    RecoverHp();
                }   
            }
        }

        private void RecoverHp()
        {
            var hp = attribute[(int) DictAbilityPropEnum.HP];
            var maxHP = attribute[(int) DictAbilityPropEnum.MAX_HP];
            var re = attribute[(int) DictAbilityPropEnum.HP_TREAT_SPEED];
            hp += re * maxHP / 50;
            if (hp > maxHP)
            {
                hp = maxHP;
            }
            attribute[(int) DictAbilityPropEnum.HP] = hp;
            var player = GrowFun.Instance.growData.growPlayer;
            player.hpPercent = attribute[(int) DictAbilityPropEnum.HP] / attribute[(int) DictAbilityPropEnum.MAX_HP];

            NotificationCenter.Default.PostNotification((int)GameMessageId.FreshHPUI);
        }

        public void End()
        {
            TimeEventManager.Delete(ref _eventHandler);
        }

        public void RecorverHpPercent(double recorverHpPercent)
        {
            var hp = attribute[(int) DictAbilityPropEnum.HP];
            var maxHP = attribute[(int) DictAbilityPropEnum.MAX_HP];
            var addHp = maxHP * recorverHpPercent;
            hp += addHp;
            // var re = attribute[(int) DictAbilityPropEnum.HP_TREAT_SPEED];
            // hp += re * maxHP / 50;
            if (hp > maxHP)
            {
                hp = maxHP;
            }
            attribute[(int) DictAbilityPropEnum.HP] = hp;
            var player = GrowFun.Instance.growData.growPlayer;
            player.hpPercent = attribute[(int) DictAbilityPropEnum.HP] / attribute[(int) DictAbilityPropEnum.MAX_HP];

            NotificationCenter.Default.PostNotification((int)GameMessageId.FreshHPUI);
        }
    }
}