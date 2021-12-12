using System.Collections.Generic;

namespace TextEquip.System
{
    public class CopyEventConfig
    {
        public double[]  baseAttribute =  new double[WorldConfig.MaxAttribute];
        public CopyEventDropConfig copyEventDropConfig;
        public string name;
        public string type;
        public string eventType;
        public double costTime = 1;
        public double openCostTime = 1;
        /// <summary>
        /// 位置
        /// </summary>
        public int posIndex = 0;

        public string icon="";
        public string maskIcon="mask";
        
        public const int MaxCol = 5;

        public int curRow
        {
            get
            {
                return posIndex / MaxCol;
            }
        }

        public int curCol
        {
            get
            {
                return posIndex % MaxCol;
            }
        }
        public CopyEventConfig nextConfig;
        public bool isLastBoss;
    }

    public class CopyEventDropConfig
    {
        //E,D,C,B,A,S,Z,X
        public double[] equipDropRadios = new double[16];
        public double gold;
        public bool dropKey;
    }
}