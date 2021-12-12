using System.Collections.Generic;
using TextEquip.System;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System
{
    public class BattleAPI
    {
        
        public class BattleResult
        {
            public bool win = false;
            public List<DropConfig> dropList = new List<DropConfig>();
            public int lostHp;
            public int battleTime;
            public bool firstWin = false;
            public bool lvUp = false;
        }
        public static BattleResult Battle(double[] playerAttribute, double[] monsterAttribute)
        {
            BattleResult result = new BattleResult();
            
            double healthRecoverySpeed = playerAttribute[(int)DictAbilityPropEnum.HP_TREAT_SPEED];
            double reducedDamage = playerAttribute[(int)DictAbilityPropEnum.REDUCDMG];
            double playerDPS = playerAttribute[(int)DictAbilityPropEnum.DPS];
            double playerBLOC = playerAttribute[(int)DictAbilityPropEnum.BLOC];
            var curHP = playerAttribute[(int) DictAbilityPropEnum.HP];

            // var p = this.findComponentUpward(this, 'index')
            
            var playerDeadTime = (curHP + playerBLOC) / reducedDamage / monsterAttribute[(int) DictAbilityPropEnum.ATK];
            var monsterDeadTime = (monsterAttribute[(int) DictAbilityPropEnum.HP] / playerDPS);
            
            // 战斗获胜
            if (monsterDeadTime < playerDeadTime)
            {
                result.battleTime = (int)monsterDeadTime;
                result.win = true;
            }
            else
            {
                result.battleTime = (int)playerDeadTime;
                result.win = false;
            }
            var takeDmg = -result.battleTime * monsterAttribute[(int) DictAbilityPropEnum.ATK];
            takeDmg = (takeDmg * reducedDamage);
            takeDmg = takeDmg + playerBLOC;
            takeDmg = takeDmg > -1 ? -1 : takeDmg;
            result.lostHp = (int)takeDmg;

            return result;
        }
        
    }
}