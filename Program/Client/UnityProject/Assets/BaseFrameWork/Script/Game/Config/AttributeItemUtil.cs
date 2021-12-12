using System;
using System.Collections.Generic;
using System.Text;
using Script.Game.Grow;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace TextEquip.System
{
    public sealed class AttributeItemUtil
    {
        public static void AddAtibute(double[] attributeItems, double[] attribute)
        {
            for (int i = 0; i < WorldConfig.MaxAttribute; i++)
            {
                attributeItems[i] += attribute[i];
            }
        }

        public static string GetShowVal(DictAbilityPropEnum type,double value,int recastLev)
        {
            var qulity = recastLev / 20;
            var model = DictDataManager.Instance.dictAbilityProp.GetModel((int) type);
            if (model.showPercent > 0)
            {
                return RichTextUtil.AddColor(model.name.ToString() + ":" + (value*100).ToString("F1")+"%", 10 + qulity);
            }
            else
            {
                return RichTextUtil.AddColor(model.name.ToString() + ":" + value.ToString("F1"), 10 + qulity);
            }
        }

        public static List<string> GetAttributeDescs(double[] attributeItems)
        {
            List<string> strs = new List<string>();
            for(int i=0;i<(int)DictAbilityPropEnum.MAX_COUNT;i++)
            {
                var type = (DictAbilityPropEnum)i;
                if (attributeItems[i] <= 0)
                {
                    continue;
                }
                strs.Add(GetShowVal(type,attributeItems[i],0));
            }

            return strs;
        }
        
        public static string GetAttributeDesc(double[] attributeItems)
        {
            var items = AttributeItemUtil.GetAttributeDescs(attributeItems);
            StringBuilder sb= new StringBuilder();
            foreach (var item in items)
            {
                sb.AppendLine(item+"\t");
            }

            return sb.ToString();
        }

        public static List<string> GetAttributeDescs(List<AbilityItem> abilityItems)
        {
            List<string> strs = new List<string>();
            for(int i=0;i<abilityItems.Count;i++)
            {
                var type = abilityItems[i].type;
                if (abilityItems[i].value <= 0)
                {
                    continue;
                }
                strs.Add(GetShowVal(type,abilityItems[i].value,abilityItems[i].recastLev));
            }

            return strs;
        }

        static char[] sp = { '|' };
        public static List<AttributeConfig> ConvertString2Struc(List<string> modelBaseAbility)
        {
            List<AttributeConfig> configs = new List<AttributeConfig>();
            foreach (var ability in modelBaseAbility)
            {
                string[] data = ability.Split ( sp,StringSplitOptions.RemoveEmptyEntries);
                if (data.Length < 3)
                {
                    continue;
                }
                AttributeConfig config = new AttributeConfig();

                config.type = data[0];

                if (!double.TryParse(data[1], out config.val))
                {
                    Debug.LogError(ability);
                    continue;
                }

                if (!double.TryParse(data[2], out config.valCoefficient))
                {
                    Debug.LogError(ability);
                    continue;
                }

                configs.Add(config);
            }

            return configs;
        }

        public static bool IsShowPercent(DictAbilityPropEnum baseItemType)
        {
            var model = DictDataManager.Instance.dictAbilityProp.GetModel((int) baseItemType);
            return model.showPercent>0;
        }
    }
}