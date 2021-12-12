namespace Script.Game.System.Entity
{
    public class CopyEventDoorEntityImp:CopyEventEntityImp
    {
        
        protected override void _OnInit()
        {
            this.operationNear = true;
        }

        public override bool CheckCanOperation()
        {
            if (_copyEntity.copyEntityImp.getKey)
            {
                return true;
            }
            return false;
        }

        public override bool CheckLocked()
        {
            return false;
        }

        protected override void _OnActionTimeDone()
        {
            result.enterNextLayer = true;
        }
    }
}