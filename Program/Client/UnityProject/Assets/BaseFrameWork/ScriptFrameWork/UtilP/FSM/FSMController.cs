using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
	public class FSMMachine<T> : FSMBaseCtrl<T, FSMtate<T>>, IFSMCtrl<T>
	{
		public const string Key_BeforeStateNAN = "FSM_CUS_KEY_BSNAN";
		
		private bool hadDefaultState = false;
		private T defaultState;
		private object defaultStateEnterData = null;

		public FSMMachine ()
		{
		}

		public new void SetDefaultState (T defaultState)
		{
			this.SetDefaultState (defaultState, null);
		}

		public void SetDefaultState (T defaultState, object defaultStateEnterData)
		{
			this.defaultState = defaultState;
			this.hadDefaultState = true;
			this.defaultStateEnterData = defaultStateEnterData;
		}

		public override void Tick (float deltaTime)
		{
			if (currState == null) {
				if (hadDefaultState) {
					currState = statePool [defaultState];
					currState._Enter (new FSMParam<T> (defaultState, defaultStateEnterData));
				}
			} else {
				object enterParamData = new object ();
				T beforeStateType = currState.StateType;
				T nextStateType = currState.GetNextStateType (ref enterParamData);
				if (!nextStateType.Equals (beforeStateType)) {
					currState._Leave (new FSMParam<T> (nextStateType));
					currState = statePool [nextStateType];
					currState._Enter (new FSMParam<T> (beforeStateType, enterParamData));
				}
				currState._Tick (deltaTime);
			}
		}

		public override void Goto (T nextStateType, object enterParam, bool allowSameState = false)
		{
			T beforeStateType = default(T);
//			OCDictionary enterParamData = enterParam as OCDictionary;
//			if (enterParamData == null) {
//				enterParamData = new OCDictionary ();
//			}
			if (currState != null) {
				beforeStateType = currState.StateType;
				if (!allowSameState && nextStateType.Equals (beforeStateType)) {
					return;
				}
				currState._Leave (new FSMParam<T> (nextStateType));
//				enterParamData.Insert (Key_BeforeStateNAN, false);
			} else {
//				enterParamData.Insert (Key_BeforeStateNAN, true);
			}
			if (statePool.ContainsKey (nextStateType)) {
				currState = statePool [nextStateType];
				currState._Enter (new FSMParam<T> (beforeStateType, enterParam));
			}
		}
		
	}
}