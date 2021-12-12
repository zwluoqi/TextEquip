using System.Collections.Generic;

namespace TextEquip.System
{
    public class WorldConfig
    {
        public const int MaxAttribute = 128;
        public const int MaxEquip = 4;
        public const int MaxBagCount = 40;
        
        public List<CopyConfig> copyList = new List<CopyConfig>();
    }
}