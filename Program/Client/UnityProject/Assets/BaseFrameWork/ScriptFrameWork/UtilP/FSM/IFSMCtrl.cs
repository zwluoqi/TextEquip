using System;

namespace FSM
{
	public interface IFSMCtrl<T>
	{
		void Tick (float deltaTime);

		void Goto (T nextStateType, object enterParam, bool allowSameState);
	}
}