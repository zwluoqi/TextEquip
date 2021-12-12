using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    /// <summary>
    /// 结束后直接获得奖励即可
    /// </summary>
    public class CopyEventGiftEntityImp:CopyEventEntityImp
    {
        protected override void _OnOpenTimeDone()
        {
            base._OnOpenTimeDone();
            operationNear = true;
        }

        public override bool CheckCanOperation()
        {
            if (this.config.type == "key")
            {
                return true;
            }
            else
            {
                return base.CheckCanOperation();
            }
        }

        protected override void _OnActionTimeDone()
        {
            if (this.config.type == "key")
            {
                SystemlogCtrl.PostSystemLog("获得钥匙，可以前往下一层了");
                this._copyEntity.copyEntityImp.getKey = true;
            }
            else
            {
                SystemlogCtrl.PostSystemLog("打开了礼包，获得了不菲的奖励！");
                var dropResult = CaculateTrophy();
            }
        }
    }
}