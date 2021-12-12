using System;
using System.Collections.Generic;
using TextEquip.System;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.Grow.GrowAPI
{
    public class GrowEquipAPI
    {

        public static DictEquipEquip.Model GetEquipModel(GrowEquip equip)
        {
            return DictDataManager.Instance.dictEquipEquip.GetModel(equip.equipId);
        }

        // public static DictAbility.Model GetEquipAbility(GrowEquip equip)
        // {
        //     return DictDataManager.Instance.dictAbility.GetModel(GetEquipModel(equip).abilityId);
        // }

        public static GrowEquip CreateNewEquip(RandomUtil randomUtil,DictEquipEquipTypeEnum equipTypeEnum, int qulity, int lv)
        {
            List<DictEquipEquip.Model> qulityTypeModels = DictDataManager.Instance.dictEquipEquip.GetModelsByType(equipTypeEnum,qulity);

            int index = randomUtil.Range(0, qulityTypeModels.Count);
            var equip = qulityTypeModels[index];

            GrowEquip weapon = CreateNewEquipByEquip(randomUtil, equip, qulity, lv);
            
            return weapon;
        }

        private static GrowEquip CreateNewEquipByEquip(RandomUtil randomUtil, DictEquipEquip.Model equip, int qulity, int lv)
        {
            
            GrowEquip weapon = new GrowEquip();
            weapon.equipId = equip.id;
            weapon.lev = lv;
            weapon.qulity = qulity;

            var baseability = AttributeItemUtil.ConvertString2Struc(equip.baseAbility);
            var qulityModel =
                DictDataManager.Instance.dictEquipQulity.GetModelByQulityAndType(qulity, equip.equip_type_int);
            var typeModel = DictDataManager.Instance.dictEquipEquipType.GetModel(equip.equip_type_int);
            var extraability = AttributeItemUtil.ConvertString2Struc(typeModel.extraEntrys);

            weapon.baseItems =  CreateRandomAttributeItem(randomUtil,baseability, weapon.lev, qulityModel.qualityCoefficient);
            weapon.extraItems = CreateRandomAttributeItem(randomUtil, extraability, weapon.lev,
                qulityModel.qualityCoefficient, qulityModel.extraAbilityNum);

            weapon.BuildData();
            return weapon;
        }

        private static List<AbilityItem> CreateRandomAttributeItem(RandomUtil randomUtil,List<AttributeConfig> abilitys, int lv, double equipQualityCoefficient,
            int num = -1)
        {
            List<AbilityItem> items = new List<AbilityItem>();
            if (abilitys.Count == 0)
            {
                return items;
            }
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    int index = randomUtil.Range(0, abilitys.Count);
                    var source = abilitys[index];
                    var randomCoefficient = randomUtil.value;
                    AbilityItem item = RandomAttributeOne(source,lv,equipQualityCoefficient,randomCoefficient);
                    item.recastLev = (int)Math.Floor(randomCoefficient * 100);
                    items.Add(item);
                }
            }
            else
            {
                foreach (var source in abilitys)
                {
                    var randomCoefficient = randomUtil.value;
                    AbilityItem item = RandomAttributeOne(source,lv,equipQualityCoefficient,randomCoefficient);
                    item.recastLev = (int)Math.Floor(randomCoefficient * 100);
                    items.Add(item);
                }
            }

            return items;
        }
        
        

        private static AbilityItem RandomAttributeOne(AttributeConfig source,  int lv, double equipQualityCoefficient,double randomCoefficient)
        {
            AbilityItem item =new AbilityItem();
            
            double random = 0;
            switch (source.type)
            {
                case "atk":
                    random = (lv * source.valCoefficient + (randomCoefficient * lv / 2 + 1));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random;
                    item.type = DictAbilityPropEnum.ATK;
                    break;
                case "def":
                    random = ((lv * source.valCoefficient + (randomCoefficient * lv / 2 + 1)));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random;
                    item.type = DictAbilityPropEnum.DEF;
                    break;
                case "hp_max":
                    random = ((lv * source.valCoefficient + (randomCoefficient * lv / 2 + 1)));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random;
                    item.type = DictAbilityPropEnum.MAX_HP;

                    break;
                case "crit":
                    random = (randomCoefficient * 5 + 5);
                    random = (random * equipQualityCoefficient);
                    item.value = random * 0.01f;
                    item.type = DictAbilityPropEnum.CRIT;

                    break;
                case "critdmg":
                    random = (randomCoefficient * 12 + 20);
                    random = (random * equipQualityCoefficient);
                    item.value = random * 0.01f;
                    item.type = DictAbilityPropEnum.CRITDMG;
                    break;
                case "def_percent":
                    random = (lv * source.valCoefficient + (randomCoefficient * lv / 10 + 4));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random * 0.01f;
                    item.type = DictAbilityPropEnum.DEFPERCENT;

                break;
                case "atk_percent":
                    random = (lv * source.valCoefficient + (randomCoefficient * lv / 10 + 4));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random* 0.01f;
                    item.type = DictAbilityPropEnum.DEFPERCENT;
                    break;
                case "hp_max_percent":
                    random = (lv * source.valCoefficient + (randomCoefficient * lv / 10 + 4));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random * 0.01f;
                    item.type = DictAbilityPropEnum.HPMAXPERCENT;
                    break;
                case "bloc":
                    random = ((lv * source.valCoefficient + (randomCoefficient * lv / 2 + 1)));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random;
                    item.type = DictAbilityPropEnum.BLOC;
                    break;
                case "treate_hp_speed":
                    random = ((lv * source.valCoefficient + (randomCoefficient * lv / 2 + 1)));
                    random = (random * equipQualityCoefficient);
                    random = random>0?random: 1;
                    item.value = random;
                    item.type = DictAbilityPropEnum.HP_TREAT_SPEED;
                    break;
              default:
                  Debug.LogError("不支持:"+source.type);
                  break;
              
            }
            return item;
        }
        

                
        public static AbilityItem CalculateStrAttr(AbilityItem item,int enhanceLev)
        {
            AbilityItem newItem = new AbilityItem();
            double a = 1;
            double value = 0;
            // 确定强化系数
            a = Math.Pow(Math.Pow(1.055, enhanceLev) ,1.1);
            value = a * item.value;
            newItem.value = value;
            newItem.type = item.type;
            newItem.recastLev = item.recastLev;
            return newItem;
        }
        
        /**
 *  返回一条随机属性
 * @param {number} lv  装备等级
 */
        public static AbilityItem CreateRandomEntry(RandomUtil randomUtil, int lv,double qualityCoefficient)
        {
              var abilityModel =DictDataManager.Instance.dictAbility.GetModel("random_recast");
              var attributeConfigs =  AttributeItemUtil.ConvertString2Struc(abilityModel.extraEntrys);

              var randomCoefficient = randomUtil.value;

              var index = randomUtil.Range(0, attributeConfigs.Count);
              var entry = attributeConfigs[index];
              
              var abilityItem = RandomAttributeOne(entry, lv, qualityCoefficient,randomCoefficient);
              abilityItem.recastLev = (int)Math.Floor(randomCoefficient * 100);
              return abilityItem;
        }


        public static AbilityItem ConvertChongsheng2Ability(ChongShengAbilityItem source)
        {
            AbilityItem item =  new AbilityItem();
            double random = 0;
            switch (source.type)
            {
                case "atk":
                    item.value = source.point * 3;
                    item.type = DictAbilityPropEnum.ATK;
                    break;
                case "def":
                    item.value = source.point * 2;
                    item.type = DictAbilityPropEnum.DEF;
                    break;
                case "hp_max":
                    item.value = source.point * 10 ;
                    item.type = DictAbilityPropEnum.MAX_HP;

                    break;
                case "crit":
                    item.value = source.point * 0.1 * 0.01f;
                    item.type = DictAbilityPropEnum.CRIT;
                    break;
                case "critdmg":
                    item.value = source.point * 1 * 0.01f;
                    item.type = DictAbilityPropEnum.CRITDMG;
                    break;
                case "def_percent":
                    item.value = source.point * 0.1 * 0.01f;
                    item.type = DictAbilityPropEnum.DEFPERCENT;

                break;
                case "atk_percent":
                    item.value = source.point * 0.1* 0.01f;
                    item.type = DictAbilityPropEnum.DEFPERCENT;
                    break;
                case "hp_max_percent":
                    item.value = source.point * 0.1 * 0.01f;
                    item.type = DictAbilityPropEnum.HPMAXPERCENT;
                    break;
                case "bloc":
                    item.value = source.point * 2;
                    item.type = DictAbilityPropEnum.BLOC;
                    break;
                case "treate_hp_speed":
                    item.value = source.point * 2;
                    item.type = DictAbilityPropEnum.HP_TREAT_SPEED;
                    break;
                case "move_speed":
                    item.value = source.point * 0.06;
                    item.type = DictAbilityPropEnum.MOVESPEED;
                    break;
                case "battle_speed":
                    item.value = source.point * 0.01;
                    item.type = DictAbilityPropEnum.BATTLESPEED;
                    break;
              default:
                  Debug.LogError("不支持:"+source.type);
                  break;
            }

            return item;
        }
        
    }
}