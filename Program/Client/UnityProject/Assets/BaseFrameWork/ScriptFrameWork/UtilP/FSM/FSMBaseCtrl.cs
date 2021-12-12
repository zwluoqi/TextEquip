using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	public class FSMBaseCtrl<T, S> : IFSMCtrl<T> where S : IFSMState<T>
	{
		#region Fields

		protected Dictionary<T, S> statePool = new Dictionary<T, S> ();
		protected S currState;

		protected bool hadDefaultState = false;
		protected T defaultState;

		#endregion

		#region Properties

		public bool Running { get { return currState != null; } }

		public T CurrStateType { 
			get {
				if (currState != null) {
					return currState.StateType;
				} else {
					return default(T);
				}
			}
		}

		#endregion

		#region Constructors

		public FSMBaseCtrl ()
		{
		}

		#endregion

		#region Methods

		public void AddState (S newState)
		{
			this.statePool [newState.StateType] = newState;
		}

		public void SetDefaultState (T defaultState)
		{
			this.hadDefaultState = true;
			this.defaultState = defaultState;
		}

		public bool CurrentStateIs (T state)
		{
			if (this.currState != null) {
				return state.Equals (this.currState.StateType);
			} else {
				return false;
			}
		}

		public S this [T stateType] {
			get {
				return this.statePool [stateType];
			}
		}

		public V GetState<V> (T stateType) where V : S
		{
			return (V)this.statePool [stateType];
		}

		public virtual void Tick (float deltaTime)
		{
			if (this.currState == null && hadDefaultState) {
				this.currState = this.statePool [defaultState];
				this.currState._Enter (new FSMParam<T> (default(T)));
			}
			if (this.currState != null) {
				this.currState._Tick (deltaTime);
			}
		}

		public virtual void FixedTick (float deltaTime)
		{
			if (this.currState != null) {
				this.currState._FixedTick (deltaTime);
			}
		}

		public virtual void Goto (T nextStateType, object enterParam = null, bool allowSameState = false)
		{
			T lastState = default(T);
			if (this.currState != null) {
				if (!allowSameState && nextStateType.Equals (this.currState.StateType)) {
					return;
				} else {
					lastState = this.currState.StateType;
					this.currState._Leave (new FSMParam<T> (nextStateType));
				}
			}
			if (this.statePool.ContainsKey (nextStateType)) {
				this.currState = this.statePool [nextStateType];
				this.currState._Enter (new FSMParam<T> (lastState, enterParam));
				Debug.LogWarning("goto state "+nextStateType.ToString());
			}
		}

		#endregion
	}
}