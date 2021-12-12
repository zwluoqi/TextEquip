using System;

namespace FSM
{
	public interface IFSMState<T>
	{
		T StateType{ get; }

		void _Enter (FSMParam<T> enterParam);

		void _Tick (float delta);

		void _FixedTick (float delta);

		void _Leave (FSMParam<T> leaveParam);
	}
}