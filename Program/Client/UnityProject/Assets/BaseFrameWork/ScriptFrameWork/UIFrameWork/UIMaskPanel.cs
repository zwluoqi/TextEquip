using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;
using System.Text;
using UnityEngine.UI;


namespace XZXD.UI
{
	class UIMaskPanel:MonoBehaviour
	{
		public enum MaskType
		{
			BlackToWhite = 0,//黑变白
			WhiteToBlack = 1,//白变黑
		}

		public CanvasGroup m_mask;

		private int currentState;
		public void Init(int t,float time)
		{
			switch (t)
			{
			case  0:
				m_mask.alpha = 1;
				// m_mask.DOFade(0,time);
				break;
			case  1:
				m_mask.alpha = 0;
				// m_mask.DOFade(1,time);
				break;
			}
			currentState = t;
			m_mask.gameObject.SetActive (true);

			RunCoroutine.Stop (_run);
			_run = RunCoroutine.Run(CloseMask(time));
		}

		RunCoroutine _run;

		IEnumerator CloseMask(float time)
		{
			yield return new WaitForSeconds(time);
			if (currentState == 1) {
				m_mask.gameObject.SetActive (true);
			} else {
				m_mask.gameObject.SetActive (false);
			}
		}
	}

}
