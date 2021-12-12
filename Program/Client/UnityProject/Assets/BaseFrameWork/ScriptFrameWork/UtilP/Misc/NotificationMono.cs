using System;
using UnityEngine;
using System.Collections.Generic;

namespace NotificationCenterDebug
{
	public class NotificationMono : MonoBehaviour
	{
		#if UNITY_EDITOR
//		float time;
		public Dictionary<object, List<int>> dic;

		void Awake ()
		{
			dic = new Dictionary<object, List<int>> ();
		}

		void Update ()
		{
//			time += Time.unscaledDeltaTime;
//			if (time > 1) {
				dic = NotificationCenter.Default.GetObserverDict ();
//				time = 0;
//			}
		}
		#endif
	}
}

