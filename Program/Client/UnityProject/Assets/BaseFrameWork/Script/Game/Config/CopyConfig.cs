using System.Collections.Generic;

namespace TextEquip.System
{
    public class CopyConfig
    {
        public List<CopyEventConfig> eventConfigs = new List<CopyEventConfig>();
        public CopyEventConfig[] gameEventConfigs;
        public string id;
        public int battleTime;
        public string name;
        public int eventNum;
        public int lv;
        public double needDPS;
        public int difficulty;
        public string difficultyName;
        public double top;
        public double left;
        public int maxIndex = -1;
        public int bossIndex = -1;
        public int bornIndex = -1;
        public int maxLayer = 9;
    }
}