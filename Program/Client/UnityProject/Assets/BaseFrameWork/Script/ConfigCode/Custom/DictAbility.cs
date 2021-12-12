using System.Collections.Generic;
using TextEquip.System;

public partial class DictAbility
{
        public Dictionary<string,List<AbilityItem>> kvs = new Dictionary<string, List<AbilityItem>>();
        // public List<AbilityItem> GetCorrectModel(DictAbility.Model model)
        // {
        //         if (kvs.ContainsKey(model.id))
        //         {
        //                 return kvs[model.id];
        //         }
        //         else
        //         {
        //                 kvs[model.id] = ConvertAbility(model);
        //                 
        //                 return kvs[model.id];
        //         }
        // }

        // private List<AbilityItem> ConvertAbility(Model model)
        // {
        //         List<AbilityItem> abilityItems = new List<AbilityItem>();
        //         if (model.phy_attack > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.ATK;
        //                 abilityItem.value = model.phy_attack;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.phy_attack_per > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.ATKPERCENT;
        //                 abilityItem.value = model.phy_attack_per;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.add_maxhp > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.MAX_HP;
        //                 abilityItem.value = model.add_maxhp;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.add_maxhp_percent > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.HPMAXPERCENT;
        //                 abilityItem.value = model.add_maxhp_percent;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.pyh_def > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.DEF;
        //                 abilityItem.value = model.pyh_def;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.pyh_def_percent > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.DEFPERCENT;
        //                 abilityItem.value = model.pyh_def_percent;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.crit_radio > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.CRIT;
        //                 abilityItem.value = model.crit_radio;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.crit_dmg_percent > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.CRITDMG;
        //                 abilityItem.value = model.crit_dmg_percent;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.hit_radio > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.EVAPERCENT;
        //                 abilityItem.value = model.hit_radio;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         if (model.hp_treat_speed > 0)
        //         {
        //                 AbilityItem abilityItem = new AbilityItem();
        //                 abilityItem.type = DictAbilityPropEnum.HP_TREAT_SPEED;
        //                 abilityItem.value = model.hp_treat_speed;
        //                 abilityItems.Add(abilityItem);
        //         }
        //         
        //         return abilityItems;
        // }
}