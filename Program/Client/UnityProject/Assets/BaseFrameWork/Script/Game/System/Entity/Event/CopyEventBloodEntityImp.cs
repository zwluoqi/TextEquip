namespace Script.Game.System.Entity
{
    public class CopyEventBloodEntityImp:CopyEventEntityImp
    {
        public override void _OnActionStart()
        {
            double recorverHpPercent = double.Parse(this.config.type);

            SystemlogCtrl.PostSystemLog("打开了血瓶，血量回复！"+ (int)(recorverHpPercent*100)+"%");
        }

        protected override void _OnActionTimeDone()
        {
            double recorverHpPercent = double.Parse(this.config.type);
            world.player.RecorverHpPercent(recorverHpPercent);
        }

        protected override void _OnOpenTimeDone()
        {
            base._OnOpenTimeDone();
            this.operationNear = true;
        }
    }
}