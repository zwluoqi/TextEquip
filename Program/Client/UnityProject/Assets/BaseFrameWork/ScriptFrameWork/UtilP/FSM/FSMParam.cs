using System;

namespace FSM
{
	public class FSMParam<T>
	{
		public T otherState;
		public object param;

		public FSMParam (T state, object param = null)
		{
			this.otherState = state;
			this.param = param;
		}

		public K GetParam<K> ()
		{
			return (K)param;
		}
	}
}

