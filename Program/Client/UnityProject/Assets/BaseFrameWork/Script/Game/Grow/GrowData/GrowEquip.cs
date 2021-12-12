using System;
using System.Collections.Generic;
using System.Text;
using Script.Game.Grow.GrowAPI;
using Script.Game.System;
using TextEquip.System;
using UnityEditor;
using UnityEngine;

namespace Script.Game.Grow
{
    public class GrowEquip:BaseGrowData
    {
        public int qulity;
        public int lev;
        public int enchantlvl;
        public long lastStrenchTime;
        public string equipId =  "";
        public List<AbilityItem> baseItems = new List<AbilityItem>();
        public List<AbilityItem> extraItems = new List<AbilityItem>();
        public List<AbilityItem> uniqueItems = new List<AbilityItem>();
        public bool locked = false;
        public string authorName ="";
        public string equipName ="";
        public string equipDesc = "";
        public long authorCostCoin = 0;
        /// <summary>
        /// 传承者
        /// </summary>
        public List<string> inHeritList = new List<string>();

        public string GetAuthorName()
        {
            if (string.IsNullOrEmpty(authorName))
            {
                return "开发者";
            }
            else
            {
                return this.authorName;
            }
        }
        
        public string GetEquipName()
        {
            if (string.IsNullOrEmpty(equipName))
            {
                return this.GetModel().name;
            }
            else
            {
                return this.equipName;
            }
        }
        
        public string GetEquipDesc()
        {
            if (string.IsNullOrEmpty(equipDesc))
            {
                return this.GetModel().desc;
            }
            else
            {
                return this.equipDesc;
            }
        }
        
        
        private double[] attributeItems = new double[WorldConfig.MaxAttribute];
        

        public double[] GetAttribute()
        {
            return attributeItems;
        }

        public void UpdateAttribute()
        {
            Array.Clear(attributeItems, 0, attributeItems.Length);

            foreach (var item in baseItems)
            {
                var enhanceItem = GrowEquipAPI.CalculateStrAttr(item, Getenchantlvl());
                attributeItems[(int) item.type] += enhanceItem.value;
            }
            foreach (var item in extraItems)
            {
                attributeItems[(int) item.type] += item.value;
            }
            foreach (var item in uniqueItems)
            {
                attributeItems[(int) item.type] += item.value;
            }
        }
        



        public void CheckNew()
        {
            if (qulity == 0)
            {
                qulity = 10;
            }
        }

        public void BuildData()
        {
            UpdateAttribute();
        }

        public DictEquipEquip.Model GetModel()
        {
            return DictDataManager.Instance.dictEquipEquip.GetModel(this.equipId);
        }
        
        public DictEquipQulity.Model GetQulityModel()
        {
            return DictDataManager.Instance.dictEquipQulity.GetModelByQulityAndType(this.qulity,GetModel().equip_type_int);
        }

        public List<string> GetAttributeDescs()
        {
            return AttributeItemUtil.GetAttributeDescs(attributeItems);
        }
        
        public string GetAttributeDesc()
        {
            return AttributeItemUtil.GetAttributeDesc(attributeItems);
        }

        // public string GetBaseAttribute()
        // {
        //     return GetAbilityDesc(baseItems);
        // }

        public List<AbilityItem> GetEnhanceAttributeItems()
        {
            List<AbilityItem>  tmps = new List<AbilityItem>();
            foreach (var baseItem in baseItems)
            {
                var enItem = GrowEquipAPI.CalculateStrAttr(baseItem, Getenchantlvl());
                tmps.Add(enItem);
            }
            return tmps;
        }
        
        public string GetEnhanceAttribute()
        {
            return GetAbilityDesc(GetEnhanceAttributeItems());
        }

        
        public string GetExtraAttribute()
        {
            return GetAbilityDesc(extraItems);
        }
        
        public string GetUniqueAttribute()
        {
            return GetAbilityDesc(uniqueItems);
        }

        private string GetAbilityDesc(List<AbilityItem> abilityItems)
        {
            var items = AttributeItemUtil.GetAttributeDescs(abilityItems);
            StringBuilder sb= new StringBuilder();
            foreach (var item in items)
            {
                sb.Append(item+"\t");
            }
            return sb.ToString();
        }

 

        
        
        public long RecastNeedGold(int extraIndex)
        {
            var recastCount = extraItems[extraIndex].recastCount;
            var a = ((this.lev) * this.GetQulityModel().qualityCoefficient * (200 + 10 * (this.lev)) / 4);
            int multiple = 1 + (recastCount / 10);
            return (long) a*multiple;
        }
        
        // 重铸装备
        public void RecastTheEquiment(int index) {
            if (extraItems.Count <= index)
            {
                Debug.LogError("参数错误："+index);
                return;
            }
            var needGold = this.RecastNeedGold(index) * 1;//ra
            if (GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.coin) < needGold)
            {
                SystemlogCtrl.PostSystemLog("钱不够啊，重铸啥呢" );
                return;
            }

            int recastLev = 1;
            var newEntry = GrowEquipAPI.CreateRandomEntry(GrowFun.Instance.randomUtil, this.lev, this.GetQulityModel().qualityCoefficient);
            newEntry.recastCount = extraItems[index].recastCount;
            extraItems[index] = newEntry;
            GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin,-needGold);
            var a = newEntry.recastLev;
            var qulityClass = "E";
            if (a < 25)
            {
                qulityClass = "E";
            } else if (a < 50 && a >= 25)
            {
                qulityClass = "D";
            } else if (a < 70 && a >= 50)
            {
                qulityClass = "C";
            } else if (a < 90 && a >= 70)
            {
                qulityClass = "B";
            } else
            {
                qulityClass = "A";
            }

            extraItems[index].recastCount = extraItems[index].recastCount + 1;
            this.UpdateAttribute();
            
            SystemlogCtrl.PostSystemLog(this.GetEquipName()+"重铸成功 词条等级"+newEntry.recastLev+" 词条品质："+qulityClass);
            NotificationCenter.Default.PostNotification((int)GameMessageId.RecastEquip,this.guid);
        }

        private void StopAutoStreng()
        {
        }

        public int GetEquipPoint()
        {
            double weaponPonit = 0;
            double wlv = this.lev;
            double elv = this.Getenchantlvl();
            var q = GetQulityModel().qualityCoefficient;
            if (wlv >= 20)
            {
                var x = Math.Pow(((wlv - 20.0) / 10.0), 1.1);
                var y=(0.1 * Math.Pow( (double)elv , 1.5) + 1) * q / 3.5;
                weaponPonit = x * y;
            } else
            {
                weaponPonit = 0;
            }

            return (int)weaponPonit;
        }

        public int GetLowestExtraIndex()
        {
            int index = -1;
            int lowestRecastLev = int.MaxValue;
            int i = 0;
            foreach (var extra in extraItems)
            {
                if (extra.recastLev < lowestRecastLev)
                {
                    index = i;
                    lowestRecastLev = extra.recastLev;
                }
                i++;
            }

            return index;
        }


        public void PrintInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(RichTextUtil.AddColor(
                "guid:" + this.guid +" "+this.GetEquipName()+ " lv" + lev + " enlev:" + this.Getenchantlvl() + " qulity" + qulity, qulity));
            sb.AppendLine("baseenhance:"+this.GetEnhanceAttribute());
            sb.AppendLine("extra:"+this.GetExtraAttribute());
            Debug.LogWarning(sb.ToString());
            
        }

        public int GetCostCoin()
        {
            var gold = (int)(this.lev * this.GetQulityModel().qualityCoefficient * 30);
            return gold;
        }

        public long GetBuyCostCoin()
        {
            if (authorCostCoin > 0)
            {
                return authorCostCoin;
            }
            else
            {
                var gold = (int) (this.lev * this.GetQulityModel().qualityCoefficient * (250 + 20 * this.lev));
                return gold;
            }
        }

        public DictEquipEquipType.Model GetTypeModel()
        {
            return DictDataManager.Instance.dictEquipEquipType.GetModel(this.GetModel().equip_type_int);
        }

        public int Getenchantlvl()
        {
            return enchantlvl;
        }

        // public long StrengthenNeedGold()
        // {
        //     return GrowFun.Instance.growData.growPlayer.StrengthenNeedGold(GetModel().equip_type_int);
        // }
        
        public long StrengthenNeedGold()
        {
            var a = (((this.lev) + 1) * Math.Pow(Math.Pow(1.1, (this.enchantlvl)), 1.1) * (10 + (this.lev) / 5)) + 100;
            return (long) a;
        }
        
         // 强化装备
        public bool StartStreng(int equip_pos,bool auto,int targetLev,bool use_back,int use_luck_lev)
        {
            if (auto)
            {
                if (enchantlvl >= targetLev)
                {
                    return false;
                }
            }
            
            long cur = DateTime.Now.Ticks;
            long spaceSecond = (long)((cur - lastStrenchTime) * 0.01 * 0.001);
            if (!auto)
            {
                if (spaceSecond < 60 && enchantlvl >= 12)
                {
                    BoxManager.CreatePopTis("需要等待60S才能强化+12以上,仍需等待" + (60 - spaceSecond) + "秒");
                    return false;
                }
            }

            // 自动强化需要金币倍率
            // var ra = auto ? 2 : 1
            var needGold = this.StrengthenNeedGold() * 1;//ra
            if (GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.coin) < needGold)
            {
                BoxManager.CreatePopTis("钱不够啊，强化啥呢" );
                return false;
            }
            
			//保底石头
            if (use_back && GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.Guaranteedstone) < 1)
            {
                BoxManager.CreatePopTis("保底石不够啊，强化啥呢" );
                return false;
            }
			
			double luckAddRadio = 0;
			//幸运石
            if (use_luck_lev > 0)
            {
				if(!CheckLuckCount(use_luck_lev,out luckAddRadio)){
					return false;
				}
            }
            

            var lv = this.enchantlvl ;
            double probabilityOfSuccess = 1;
            if (lv  <= 5)
            {
                probabilityOfSuccess = 1;
            } else if (lv  == 6)
            {
                probabilityOfSuccess = 0.8;
            } else if (lv == 7)
            {
                probabilityOfSuccess = 0.65;
            } else if (lv  == 8)
            {
                probabilityOfSuccess = 0.45;
            } else if (lv  == 9)
            {
                probabilityOfSuccess = 0.3;
            } else
            {
                probabilityOfSuccess = 0.2;
            }

            probabilityOfSuccess += luckAddRadio;

            var r = GrowFun.Instance.randomUtil.value;
            bool success = false;
            if (r < probabilityOfSuccess) {
                // 强化成功
                lv++;
                success = true;
            } else {
                // 强化失败
                // if (use_back)
                // {
                //     if (lv >= 5)
                //     {
                //         lv = lv - 1;
                //     }
                // }
            }
            
            GrowFun.Instance.growData.AddProp(DictPlayerPropEnum.coin,-needGold);
            this.lastStrenchTime = cur;
            this.enchantlvl = lv;
            this.UpdateAttribute();
            if (success)
            {
                BoxManager.CreatePopTis(this.GetEquipName() + RichTextUtil.AddColor("强化成功(+" + enchantlvl+")",Color.green));
            }
            else
            {
                BoxManager.CreatePopTis(this.GetEquipName()  + RichTextUtil.AddColor("强化失败(+" + enchantlvl+")",Color.red));
            }

            NotificationCenter.Default.PostNotification((int)GameMessageId.StrenthEquip,this.guid);
            return true;
        }

        private bool CheckLuckCount(int useLuckLev,out double luckAddRadio)
        {
            if(GrowFun.Instance.growData.GetPropByID(DictPlayerPropEnum.Luckstone1+useLuckLev-1) < 1){
                BoxManager.CreatePopTis("1级幸运石不够大，强化啥呢" );
                luckAddRadio = 0;
                return false;
            }else{
                luckAddRadio = 0.1;
                return true;
            }
        }

        public string GetInHeritList()
        {
            string ins = "";
            foreach (var inheri in inHeritList)
            {
                ins += inheri + ",";
            }

            return ins;
        }
    }

}