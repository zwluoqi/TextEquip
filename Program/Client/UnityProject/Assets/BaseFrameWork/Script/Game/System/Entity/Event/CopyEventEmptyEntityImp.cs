using NotImplementedException = System.NotImplementedException;

namespace Script.Game.System.Entity
{
    /// <summary>
    /// 空白事件，打开完毕即可移动
    /// </summary>
    public class CopyEventEmptyEntityImp:CopyEventEntityImp
    {

        protected override void _OnOpenTimeDone()
        {
            base._OnOpenTimeDone();
            operationNear = true;
        }
    }
}