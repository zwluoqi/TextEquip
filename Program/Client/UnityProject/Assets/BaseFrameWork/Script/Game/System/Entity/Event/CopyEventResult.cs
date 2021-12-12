using System.Collections.Generic;
using Script.Game.Grow;

namespace Script.Game.System.Entity
{
    public class CopyEventResult
    {
        public BattleAPI.BattleResult battleResult;
        public bool enterNextLayer;
    }

    public class CopyEventDropResult
    {
        public List<GrowEquip> drops = new List<GrowEquip>();
        public int gold;
    }
}