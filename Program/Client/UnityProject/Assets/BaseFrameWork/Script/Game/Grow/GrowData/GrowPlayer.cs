using System;
using System.Collections.Generic;
using Script.Game.Grow.GrowAPI;
using Script.Game.System;
using TextEquip.System;
using UnityEngine;

namespace Script.Game.Grow
{

    public class GrowPlayer:BaseGrowData
    {
        public int playerLv = 1;
        public GrowEquip[] equipEquips = new GrowEquip[WorldConfig.MaxEquip];
        public double hpPercent =1;

        public int chongshengCount;
        public int chongshengPoint;
        public List<ChongShengAbilityItem> chonshengAbilityListItems = new List<ChongShengAbilityItem>();
        public string playerName;
        private double[] attributeItems = new double[WorldConfig.MaxAttribute];


        public void UpdateAttribute()
        {
            ResetAttribute();
            AddEquipAttribute();
            AddLvAttribute();
            AddChongshengAttribute();
            CalculateAttribute();
            NotificationCenter.Default.PostNotification((int)GameMessageId.FreshAttributeUI);
        }

        private void CalculateAttribute()
        {
            
            attributeItems[(int)DictAbilityPropEnum.ATK] = (attributeItems[(int)DictAbilityPropEnum.ATK] * (1 + attributeItems[(int)DictAbilityPropEnum.ATKPERCENT]) / 1);

            attributeItems[(int)DictAbilityPropEnum.DEF] = (attributeItems[(int) DictAbilityPropEnum.DEF] *
                (1 + attributeItems[(int) DictAbilityPropEnum.DEFPERCENT]) / 1);

            attributeItems[(int)DictAbilityPropEnum.MAX_HP] = (attributeItems[(int) DictAbilityPropEnum.MAX_HP] *
                (1 + attributeItems[(int) DictAbilityPropEnum.HPMAXPERCENT]) / 1);

            attributeItems[(int)DictAbilityPropEnum.BLOC] = (attributeItems[(int) DictAbilityPropEnum.BLOC] *
                (1 + attributeItems[(int) DictAbilityPropEnum.BLOCPERCENT]) / 1);
        
            
            // attribute.EVA.value = ((1 - HitChance) * 100).toFixed(1)
            

            // console.log(vueInstance.$store.state)
            attributeItems[(int) DictAbilityPropEnum.MAX_HP] += 200;
            
            // if (hpP) {
            //
            //     attribute.CURHP.value = parseInt(attribute.MAXHP.value * hpP)
            //     attribute.CURHP.showValue = '+' + (attribute.CURHP.value)
            // } else {
            //     attribute.CURHP = vueInstance.$deepCopy(attribute.MAXHP)
            // }

            // 初始暴击伤害150%
            attributeItems[(int) DictAbilityPropEnum.CRITDMG] += 1.5;
            
            // 初始命中率
            attributeItems[(int) DictAbilityPropEnum.EVAPERCENT] += 1;
            
            //初始攻击力
            attributeItems[(int) DictAbilityPropEnum.ATK] += 10;
            
            //初始恢复能力
            attributeItems[(int) DictAbilityPropEnum.HP_TREAT_SPEED] += 1;

            var atk = attributeItems[(int) DictAbilityPropEnum.ATK];
            var crit = attributeItems[(int) DictAbilityPropEnum.CRIT];
            var critdmg = attributeItems[(int) DictAbilityPropEnum.CRITDMG];
            
            // 暴击率最多100%
            if (crit > 1)
            {
                crit = 1;
            }

            attributeItems[(int) DictAbilityPropEnum.DPS] =
                ((1 - crit / 1) * atk * 1 + crit / 1 * (critdmg) / 1 * atk * 1);
            var armor = attributeItems[(int) DictAbilityPropEnum.DEF];

            //承受伤害比例
            // attribute.REDUCDMG = 1 - 0.06 * armor / (1 + (0.06 * armor))
            attributeItems[(int) DictAbilityPropEnum.REDUCDMG] = 1 - 0.05 * armor / (1 + (0.0525 * armor));
        }

        private void AddChongshengAttribute()
        {
            foreach (var chongShengAbilityItem in chonshengAbilityListItems)
            {
                var item = GrowEquipAPI.ConvertChongsheng2Ability(chongShengAbilityItem);
                attributeItems[(int) item.type] += item.value;
            }
        }

        private void AddLvAttribute()
        {
            
        }

        private void AddEquipAttribute()
        {
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }
                var attribute =  equip.GetAttribute();
                AttributeItemUtil.AddAtibute(attributeItems, attribute);
            }
        }

        private void ResetAttribute()
        {
            Array.Clear(attributeItems,0,attributeItems.Length);
        }



        public void BuildData()
        {
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }
                equip.BuildData();
            }
            UpdateAttribute();
        }

        public double[] GetAttribute()
        {
            return attributeItems;
        }

        public GrowEquip GetEquipByIndex(int i)
        {
            return equipEquips[i];
        }

        public double GetAttributeByProp(DictAbilityPropEnum atk)
        {
            return attributeItems[(int) atk];
        }

  
        public void CheckForNewData()
        {
            //默认给的装备
            EquipNewEquips();

            if (playerLv == 0)
            {
                playerLv = 1;
            }
        }

        private void EquipNewEquips()
        {
            if (equipEquips == null || equipEquips[0] == null)
            {
                var init_equip = DictDataManager.Instance.dictSystemConfig.GetModel("init_equip");
                for (int i = 0; i < WorldConfig.MaxEquip; i++)
                {
                    DictEquipEquip.Model equip = DictDataManager.Instance.dictEquipEquip.GetModel(init_equip.vals[i]);
                    var euip = GrowEquipAPI.CreateNewEquip(GrowFun.Instance.randomUtil, (DictEquipEquipTypeEnum)equip.equip_type_int, 10, 1);
                    equipEquips[i]= euip;
                }
            }
        }

        public string GetAttributeDesc()
        {
            return AttributeItemUtil.GetAttributeDesc(attributeItems);
        }

        public int GetEquipPoint()
        {
            int point = 0;
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }

                point+=equip.GetEquipPoint();
            }

            return point;
        }


        public ChongShengAbilityItem GetChongshengAbilitiItem(string type)
        {
            foreach (var chonshengAbilityItem in chonshengAbilityListItems)
            {
                if (chonshengAbilityItem.type == type)
                {
                    return chonshengAbilityItem;
                }
            }

            return null;
            // if (chonshengAbilityItems.ContainsKey(type))
            // {
            //     return chonshengAbilityItems[type];
            // }
            // else
            // {
            //     return null;
            // }
        }

        public void ChongSheng()
        {
            this.playerLv = 1;
            this.hpPercent = 1;
            // Array.Clear(equipEnchanceLev,0,equipEnchanceLev.Length);
            Array.Clear(equipEquips,0,equipEquips.Length);
            EquipNewEquips();
        }

        public bool HasEquipEquip(GrowEquip source)
        {
            if (source == null)
            {
                return false;
            }
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }

                if (equip.guid == source.guid)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasEquipEquip(long guid)
        {
            var equip = GetEquipByGuid(guid);
            return HasEquipEquip(equip);
        }

        public void PrintPlayerEquip()
        {
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }

                equip.PrintInfo();
            }
        }

        public GrowEquip GetEquipByGuid(long l)
        {
            for (int i=0;i<equipEquips.Length;i++)
            {
                var equip = equipEquips[i];
                if (equip == null)
                {
                    continue;
                }

                if (equip.guid == l)
                {
                    return equip;
                }
            }

            return null;
        }


    }
}