using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XZXD.UI
{
	public class YesAndNoBox :UIPage
	{
		public Text m_LeftLb;
		public Text m_RightLb;
		public Button m_LeftBtn;
		public Button m_RightBtn;
		public GameObject m_close;

		public Text m_Content;
		public Text m_TitleLb;
		public GameObject m_TextTipObj;
		public RectTransform bk;

		private Void_Bool m_del;

        

		/// <summary>
		/// 默认左边  yes 右边no
		/// </summary>
		/// <param name="yes"></param>  确定
		/// <param name="no"></param>    取消
		/// <param name="content"></param>   内容
		/// change  是否改变左右的位置
		public void Init (string yes, string no, string content, Void_Bool call, bool change = false)
		{
			BaseInit ();

			if (change) {
				//右边yes  左边no
				m_RightLb.text = yes;
				m_LeftLb.text = no;

				UGUIEventListener.Get (m_RightBtn.gameObject).onClick = YesClick;
				UGUIEventListener.Get (m_LeftBtn.gameObject).onClick = NoClick;
			} else {
				//默认左边  yes 右边no
				m_LeftLb.text = yes;
				m_RightLb.text = no;

				UGUIEventListener.Get (m_RightBtn.gameObject).onClick = NoClick;
				UGUIEventListener.Get (m_LeftBtn.gameObject).onClick = YesClick;

			}
			m_del = call;
			m_Content.text = content;

			if (m_Content.preferredHeight > 100) {
				bk.sizeDelta = new Vector2 (bk.sizeDelta.x, m_Content.preferredHeight + 200>1100?1100:m_Content.preferredHeight + 200);
			}
			RectTransform rect = m_Content.GetComponent<RectTransform> ();
			rect.sizeDelta = new Vector2 (rect.sizeDelta.x, m_Content.preferredHeight);
		}

		private void BaseInit ()
		{
			m_close.gameObject.SetActive (false);
			m_LeftBtn.gameObject.SetActive (true);
			m_RightBtn.gameObject.SetActive (true);
		}


		public void Init (string yes, string no, string title, string content, Void_Bool call, bool change = false)
		{
			BaseInit ();

			Init (yes, no, content, call, change);

			m_TitleLb.text = title;
			m_TextTipObj.SetActive (true);
		}

		public void YesClick (GameObject go)
		{
			Close ();

			if (m_del != null) {
				m_del (true);
			}

		}

        

		public void NoClick (GameObject go)
		{
			Close ();
			if (m_del != null) {
				m_del (false);
			}

		}

	
	}

    

}

