namespace Script.Game.Grow
{

    public class BaseGrowData
    {
        public long guid;
        public BaseGrowData()
        {
            this.guid = DataBaseSystem.GetUniqueId();
        }
    }
}