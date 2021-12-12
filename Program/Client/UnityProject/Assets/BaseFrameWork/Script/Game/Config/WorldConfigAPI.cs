using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

namespace TextEquip.System
{

    public class WorldConfigAPI
  {
      private static WorldConfigAPI _instance;
      public static WorldConfigAPI Instance
      {
          get
          {
              if (_instance == null)
              {
                  _instance = new WorldConfigAPI();
                  _instance.randomUtil = new RandomUtil();
                  _instance.randomUtil.SetSeed(DateTime.Now.Ticks);
              }
              return _instance;
          }
      }

      RandomUtil randomUtil;


      public WorldConfig CreateWorld(int playerLv)
      {
          WorldConfig worldConfig =new WorldConfig();
          
          // worldConfig.dungeonsArr = []
          var Co = new double[] {0.85, 0.1, 0.05};
          for (int i = playerLv - 1; i > playerLv - 5; i--) {
              if (i < 1)
              {
                  break;
              }

              var difficulty = 1;
              var r = randomUtil.value;
              // 生成普通副本时有几率刷新高难度副本
              if (r <= Co[0])
              {
                  difficulty = 1;
              } else if (r < Co[1] + Co[0] && r >= Co[0])
              {
                  difficulty = 2;
              } else
              {
                  difficulty = 3;
              }

              int lv = 1;
              if (i > 100)
              {
                  lv = (int)Math.Floor(playerLv * (100 - (playerLv - i)) / 100.0);
              } else
              {
                  lv = i;
              }

              worldConfig.copyList.Add(this.CreateRandomDungeons(lv, 1));
              if (difficulty != 1)
              {
                  worldConfig.copyList.Add(this.CreateRandomDungeons(i, difficulty));
              }
          }
          for (int i = playerLv; i < playerLv + 6; i++)
          {
              var difficulty = 1;
              var r = randomUtil.value;
              // 生成普通副本时有几率刷新高难度副本
              if (r <= Co[0])
              {
                  difficulty = 1;
              } else if (r < Co[1] + Co[0] && r >= Co[0])
              {
                  difficulty = 2;
              } else
              {
                  difficulty = 3;
              }
              int lv = 1;
              if (i > 100)
              {
                  lv = (int) Math.Floor(playerLv * (100 + (i - playerLv)) / 100.0);
              } else
              {
                  lv = i;
              }
              worldConfig.copyList.Add(this.CreateRandomDungeons(lv, 1));
              if (difficulty != 1) {
                  worldConfig.copyList.Add(this.CreateRandomDungeons(lv, difficulty));
              }
          }

          return worldConfig;
      }

      public static bool IsNearBy(int curPos,int targetPos)
      {
          var x = targetPos - curPos;
          if (x == 1
              || x == -1
              || x == CopyEventConfig.MaxCol
              || x == -CopyEventConfig.MaxCol)
          {
              return true;
          }
          else
          {
              return false;
          }
      }
      
      public static List<int> GetNearByIndex(int maxIndex,int check_index)
      {
          List<int> rets = new List<int>();
          int index = check_index + 1;
          if ( index< maxIndex)
          {
              rets.Add(index);
          }
        
          index = check_index - 1;
          if (index >= 0)
          {
              rets.Add(index);
          }

          index = check_index + CopyEventConfig.MaxCol; 
          if ( index< maxIndex)
          {
              rets.Add(index);
          }

          index = check_index - CopyEventConfig.MaxCol;
          if (index >= 0)
          {
              rets.Add(index);
          }

          return rets;
      }

    /**
     * 随机化生成副本
     * @param {number} lv  副本等级
     * @param {number} difficulty(1:普通 2:困难 3:极难) 副本难度
     */
    CopyConfig CreateRandomDungeons(int lv, int difficulty)
    {
        var df = difficulty == 1 ? 1 : difficulty == 2 ? 1.15 : 1.4;
        CopyConfig copy = new CopyConfig();
        copy.id = lv + ":" + difficulty;
        copy.battleTime = 2000;
        copy.name = "Lv" + lv + '_' + (difficulty == 1 ? "普通" : difficulty == 2 ? "困难" : "极难");
        copy.eventNum = 5;
        copy.lv = lv;
        copy.needDPS = Math.Floor(lv * lv  *1.3 * 2 * difficulty);
        copy.difficulty = difficulty;
        copy.difficultyName = difficulty == 1 ? "普通" : difficulty == 2 ? "困难" : "极难";
        copy.top = randomUtil.value * 70 + 15 ;
        copy.left = randomUtil.value * 70 + 10;
        copy.maxIndex = 25;

        //mud副本
        copy.eventConfigs.Add(GeneratorHero(lv,df,0,true));
        copy.eventConfigs.Add(GeneratorHero(lv,df,1,true));
        copy.eventConfigs.Add(GeneratorHero(lv,df,2,true));
        copy.eventConfigs.Add(GeneratorHero(lv,df,3,true));
        copy.eventConfigs.Add(GeneratorBoss(lv,df,4,true,false));

        //boss位置及其护卫,难度和mud一样,掉率提升一倍
        // GenerateGameEventConfigs(copy);

        //三层
        copy.maxLayer = 3;
        
        return copy;
    }

    public void GenerateGameEventConfigs(CopyConfig copy,int layer)
    {
        var lv = copy.lv;
        //五层相当于到下一个难度了
        var df = copy.difficulty * (1 + layer * 0.2);
        
        copy.gameEventConfigs = new CopyEventConfig[copy.maxIndex];
        int boss_index = randomUtil.Range(0, copy.maxIndex);
        copy.bossIndex = boss_index;
        copy.bornIndex = -1;
        copy.gameEventConfigs[boss_index] = (GeneratorBoss(lv,df,boss_index,false,layer == copy.maxLayer-1));
        //boss掉钥匙
        copy.gameEventConfigs[boss_index].nextConfig = (GeneratorKey(lv, df, boss_index, false));
        
        int index = boss_index + 1;
        if ( index< copy.maxIndex)
        {
            copy.gameEventConfigs[index] = (GeneratorHero(lv, df, index,false));
        }
        
        index = boss_index - 1;
        if (index >= 0)
        {
            copy.gameEventConfigs[index] = (GeneratorHero(lv, df, index,false));
        }

        index = boss_index + CopyEventConfig.MaxCol; 
        if ( index< copy.maxIndex)
        {
            copy.gameEventConfigs[index] = (GeneratorHero(lv, df, index,false));
        }

        index = boss_index - CopyEventConfig.MaxCol;
        if (index >= 0)
        {
            copy.gameEventConfigs[index] = (GeneratorHero(lv, df, index,false));
        }
        

        //6个边缘怪
        for (int i = 0; i < 6; i++)
        {
            //只查找三次
            for (int m = 0; m < 3; m++)
            {
                index = randomUtil.Range(0, copy.maxIndex);
                if (copy.gameEventConfigs[index] == null)
                {
                    copy.gameEventConfigs[index] = (GeneratorHero(lv, df, index,false));
                    break;
                }
            }

            // if (copy.gameEventConfigs[index] == null)
            // {
            //     copy.gameEventConfigs[index] = (GeneratorEmptyGift(lv, df, index,false));
            // }
        }
        
        //4个礼包
        for (int i = 0; i < 4; i++)
        {
            //只查找三次
            for (int m = 0; m < 3; m++)
            {
                index = randomUtil.Range(0, copy.maxIndex);
                if (copy.gameEventConfigs[index] == null)
                {
                    copy.gameEventConfigs[index] = (GeneratorGoldGift(lv, df, index,false));
                    break;
                }
            }
            // if (copy.gameEventConfigs[index] == null)
            // {
            //     copy.gameEventConfigs[index] = (GeneratorEmptyGift(lv, df, index,false));
            // }
        }
        
        //4个血瓶
        for (int i = 0; i < 4; i++)
        {
            //只查找三次
            for (int m = 0; m < 3; m++)
            {
                index = randomUtil.Range(0, copy.maxIndex);
                if (copy.gameEventConfigs[index] == null)
                {
                    copy.gameEventConfigs[index] = (GeneratorBloodGift(lv, df, index,false,randomUtil.Range(0.1,0.3)));
                    break;
                }
            }
            // if (copy.gameEventConfigs[index] == null)
            // {
            //     copy.gameEventConfigs[index] = (GeneratorEmptyGift(lv, df, index,false));
            // }
        }
        
        //剩下全是空白区域
        for (int i = 0; i < copy.maxIndex; i++)
        {
            if (copy.gameEventConfigs[i] == null)
            {
                
                if (copy.bornIndex == -1)
                {
                    copy.bornIndex = i;
                    copy.gameEventConfigs[i] = (GeneratorBornGift(lv, df, i,false));
                }
                else
                {
                    copy.gameEventConfigs[i] = (GeneratorEmptyGift(lv, df, i,false));
                }
            }
        }

        // return copy;
    }


    
    
        
    private CopyEventConfig GeneratorKey(int lv, double df,int posIndex,bool mud)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("born(Lv.{0})",lv);
        hero.type = "key";
        hero.icon = "door_key";
        hero.eventType = "gift";
        hero.posIndex = posIndex;
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;
    
        hero.copyEventDropConfig = GeneratorEmptyDropConfig(lv, df,mud);
        
        return hero;
    }
    private CopyEventConfig GeneratorBornGift(int lv, double df,int posIndex,bool mud)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("born(Lv.{0})",lv);
        hero.type = "born";
        hero.icon = "door_unlock";
        hero.maskIcon = "door_lock";
        hero.eventType = "door";
        hero.posIndex = posIndex;
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;
    
        hero.copyEventDropConfig = GeneratorEmptyDropConfig(lv, df,mud);
        
        return hero;
    }

    private CopyEventConfig GeneratorEmptyGift(int lv, double df,int posIndex,bool mud)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("empty(Lv.{0})",lv);
        hero.type = "";
        hero.icon = "";
        hero.eventType = "empty";
        hero.posIndex = posIndex;
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;

        hero.copyEventDropConfig = GeneratorEmptyDropConfig(lv, df,mud);
        
        return hero;
    }

    private CopyEventDropConfig GeneratorEmptyDropConfig(int lv, double df, bool mud)
    {
        CopyEventDropConfig copyEventDropConfig = new CopyEventDropConfig();
        if (mud)
        {
            copyEventDropConfig.gold = 0.5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0,0,0,0,0};
        }
        else
        {
            copyEventDropConfig.gold = 5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0,0,0,0,0};
        }

        return copyEventDropConfig;
    }

    private CopyEventConfig GeneratorGoldGift(int lv, double df,int posIndex,bool mud)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("gift(Lv.{0})",lv);
        hero.type = "gift";
        hero.icon = "gift";
        hero.eventType = "gift";
        hero.posIndex = posIndex;
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;

        hero.copyEventDropConfig = GeneratorGoldDropConfig(lv, df,mud);
        
        return hero;
    }
    
    private CopyEventConfig GeneratorBloodGift(int lv, double df,int posIndex,bool mud,double percent)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("blood(Lv.{0})",lv);
        hero.type = percent.ToString();
        hero.eventType = "blood";
        hero.icon = "blood";
        hero.posIndex = posIndex;
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;

        hero.copyEventDropConfig = GeneratorGoldDropConfig(lv, df,mud);
        
        return hero;
    }

    private CopyEventDropConfig GeneratorGoldDropConfig(int lv, double df, bool mud)
    {
        CopyEventDropConfig copyEventDropConfig = new CopyEventDropConfig();
        if (mud)
        {
            copyEventDropConfig.gold = 0.5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0,0,0,0,0};
        }
        else
        {
            copyEventDropConfig.gold = 5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0,0,0,0,0};
        }

        return copyEventDropConfig;
    }

    private CopyEventConfig GeneratorHero(int lv, double df,int posIndex,bool mud)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("monster(Lv.{0})",lv);
        hero.type = "monster";
        hero.icon = "monster";
        hero.eventType = "battle";
        hero.posIndex = posIndex;
        if (mud)
        {
            hero.costTime = 2;
        }
        else
        {
            hero.costTime = 1;
        }
        
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+16)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+2)*df;

        hero.copyEventDropConfig = GeneratorDropConfig(lv, df,mud);
        
        return hero;
    }

    private CopyEventDropConfig GeneratorDropConfig(int lv, double df,bool mud)
    {
        CopyEventDropConfig copyEventDropConfig = new CopyEventDropConfig();
        if (mud)
        {
            copyEventDropConfig.gold = 0.5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0.2 * df*0.5, 0.08 * df*0.5, 0.03 * df*0.5, 0 * df*0.5};
        }
        else
        {
            copyEventDropConfig.gold = 5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[] {0.2 * df, 0.08 * df, 0.03 * df, 0 * df};
        }

        return copyEventDropConfig;
    }
    
    
    private CopyEventConfig GeneratorBoss(int lv, double df,int posIndex,bool mud,bool lastBoss)
    {
        CopyEventConfig hero = new CopyEventConfig();
        hero.name = string.Format("boss(Lv.{0})",lv);
        hero.type = "boss";
        hero.icon = "boss";
        hero.isLastBoss = lastBoss;
        hero.eventType = "battle";
        hero.posIndex = posIndex;
        if (mud)
        {
            hero.costTime = 2;
        }
        else
        {
            hero.costTime = 1;
        }
        hero.baseAttribute[(int)DictAbilityPropEnum.MAX_HP] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*5+30)*df;
        hero.baseAttribute[(int)DictAbilityPropEnum.ATK] = Math.Pow(lv*lv ,1.1)*(randomUtil.value*1+3)*df;

        hero.copyEventDropConfig = GeneratorBossDropConfig(lv, df,mud);
        
        return hero;
    }
    
    private CopyEventDropConfig GeneratorBossDropConfig(int lv, double df,bool mud)
    {
        CopyEventDropConfig copyEventDropConfig = new CopyEventDropConfig();
        if (mud)
        {
            copyEventDropConfig.gold = 0.5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[]
                {0.25 - 0.05 * df*0.5, 0.55 - 0.15 * df*0.5, 0.15 + 0.15 * df*0.5, 0.05 + 0.05 * df*0.5};
        }
        else
        {
            copyEventDropConfig.gold = 5*Math.Pow(lv, 1.16) * (randomUtil.value * 5 + 11) * df;
            copyEventDropConfig.equipDropRadios = new double[]
                {0.25 - 0.05 * df, 0.55 - 0.15 * df, 0.15 + 0.15 * df, 0.05 + 0.05 * df};
        }

        return copyEventDropConfig;
    }


    /**
     *  返回一条随机属性
     * @param {number} lv  装备强化等级
     */
    void createRandomEntry(int lv, int qualityCoefficient)
    {

    }
  }
}