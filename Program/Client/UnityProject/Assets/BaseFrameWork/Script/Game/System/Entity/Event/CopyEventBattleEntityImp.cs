using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Script.Game.Grow;
using Script.Game.Grow.GrowAPI;
using TextEquip.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    public class CopyEventBattleEntityImp:CopyEventEntityImp
    {
        public double[] attribute = new double[WorldConfig.MaxAttribute];

        protected override void _OnInit()
        {
            Array.Copy(this.config.baseAttribute, attribute,attribute.Length);
            attribute[(int) DictAbilityPropEnum.HP] = attribute[(int) DictAbilityPropEnum.MAX_HP];
        }

        public override void _OnActionStart()
        {
            SystemlogCtrl.PostSystemLog(RichTextUtil.AddColor("你遭遇了"+this.config.name,ColorUtil.Color_Yellow));
        }
        

        protected override void _OnActionTimeDone()
        {
            var battleResult = BattleAPI.Battle(world.player.attribute, this.attribute);
            var hp = world.player.attribute[(int) DictAbilityPropEnum.HP];
            hp += battleResult.lostHp;
            if (hp <= 0)
            {
                hp = 0;
            }
            world.player.attribute[(int) DictAbilityPropEnum.HP] = hp;
            Debug.LogWarning(JsonConvert.SerializeObject(battleResult)+" hp:"+hp);
            this.result.battleResult = battleResult;
            if (this.result.battleResult.win)
            {
                NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,"击杀了"+config.name+",受到了"+result.battleResult.lostHp+"点伤害");
                if (this.config.type == "boss")
                {
                    if (!GrowFun.Instance.growData.lvCopyOpenMud[this._copyEntity.config.lv])
                    {
                        this.result.battleResult.firstWin = true;
                        GrowFun.Instance.growData.lvCopyOpenMud[this._copyEntity.config.lv] = true;
                    }

                    if (this._copyEntity.config.lv > GrowFun.Instance.growData.growPlayer.playerLv)
                    {
                        this.result.battleResult.lvUp = true;
                        GrowFun.Instance.growData.growPlayer.playerLv = this._copyEntity.config.lv;
                        NotificationCenter.Default.PostNotification((int) GameMessageId.SystemLogId,
                            RichTextUtil.AddColor("你升级了，可以刷新出更高等级的副本了", Color.green));
                    }
                }
                CaculateTrophy();
            }
            else
            {
                NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,"战斗失败！受到了"+result.battleResult.lostHp+"点伤害");
                NotificationCenter.Default.PostNotification((int)GameMessageId.SystemLogId,RichTextUtil.AddColor("你可以尝试强化或者重铸装备之后在来挑战哦",Color.red));
            }
            if (GrowFun.Instance.growData.growEquips.Count >= WorldConfig.MaxBagCount-1)
            {
                SystemlogCtrl.PostSystemLog("背包空间即将满载，请及时整理背包");
            }

            GrowFun.Instance.growData.growPlayer.hpPercent = world.player.attribute[(int) DictAbilityPropEnum.HP] /
                                                             world.player.attribute[(int) DictAbilityPropEnum.MAX_HP];
            
            if (battleResult.win)
            {
                this.operationNear = true;
            }

        }



        public override bool ContinueNext()
        {
            if (!this.result.battleResult.win)
            {
                return false;
            }
            return true;
        }
    }
}