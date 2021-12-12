using UnityEngine;
using System.Collections;

namespace FSM
{
	public class FSMtate<T> : FSMBaseState<T, FSMMachine<T>>, IFSMState<T>
	{
		/// <summary>
		/// Get the running time.
		/// </summary>
		/// <value>Current state running time(s).</value>
		public float runningTime{ get { return _runningTime; } }

		public FSMtate (T stateType, FSMMachine<T> controller) : base (stateType, controller)
		{
		}

		protected sealed override void Enter (FSMParam<T> enterParam)
		{
			this.AEnter (enterParam.otherState, enterParam.param);
		}

		protected virtual void AEnter (T beforeState, object param)
		{
		}

		protected sealed override void Leave (FSMParam<T> enterParam)
		{
			this.ALeave (enterParam.otherState);
		}

		protected virtual void ALeave (T nextState)
		{
		}

		public virtual T GetNextStateType (ref object nextStateEnterParamData)
		{
			return StateType;
		}
	}
}