using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	public class FSMBaseState<T, M> : IFSMState<T> where M : IFSMCtrl<T>
	{
		#region Fields

		protected readonly M _ctrl;
		protected float _runningBeforeTime;
		protected float _runningTime;

		#endregion

		public T StateType{ get; private set; }

		#region Constructors

		public FSMBaseState (T stateType, M controller)
		{
			this.StateType = stateType;
			this._ctrl = controller;
			_runningTime = 0;
		}

		#endregion

		#region Methods

		public void _Enter (FSMParam<T> enterParam)
		{
			ResetRunningTime ();
			this.Enter (enterParam);
		}

		protected void ResetRunningTime ()
		{
			_runningBeforeTime = -1;
			_runningTime = 0;
		}

		protected virtual void Enter (FSMParam<T> enterParam)
		{
		}

		public void _Tick (float delta)
		{
			_runningBeforeTime = _runningTime;
			_runningTime += delta;
			this.Tick (delta);
		}

		public void _FixedTick (float delta)
		{
			this.FixedTick (delta);
		}

		protected virtual void FixedTick (float delta)
		{
		}

		protected virtual void Tick (float delta)
		{
		}

		protected bool AtTimePoint (float time)
		{
			return _runningBeforeTime < time && time <= _runningTime;
		}

		public void _Leave (FSMParam<T> leaveParam)
		{
			this.Leave (leaveParam);
		}

		protected virtual void Leave (FSMParam<T> leaveParam)
		{
		}

		protected void Goto (T nextStateType, object param = null)
		{
			_ctrl.Goto (nextStateType, param, false);
		}

		#endregion
	}
}