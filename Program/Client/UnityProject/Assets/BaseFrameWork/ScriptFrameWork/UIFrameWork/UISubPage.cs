using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

namespace XZXD.UI
{
	public class UISubPage : UIBase
	{
		public bool moveOutWithHide = true;

		public MonoBehaviour owner;


		public bool InScreen;

		public bool IsAniming{
			get{
				return animing;
			}
		}
		bool animing = false;
	
		public void Load()
		{
			InScreen = false;
			this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (outScreenPos, 0);

			LoadChilds();
			DoLoad ();
		}

		protected virtual void DoLoad ()
		{
		}

		public void Destroy(){
			DestroyChilds ();
			DoDestroy ();
		}

		protected virtual void DoDestroy ()
		{
		}

		public void Open ()
		{
			XZXDDebug.Log (this.name+" sub open");
			DoOpen ();
			OpenChilds ();
			XZXDDebug.Log (this.name+" sub open done");
		}

		protected virtual void DoOpen ()
		{
		}

		public  void Close ()
		{
			TimeEventManager.Delete (ref animCoroutine);
			CloseChilds ();
			DoClose ();
		}

		protected virtual void DoClose ()
		{
		}

		public  void Pause ()
		{
			if (InScreen) {
				PauseChilds ();
				DoPause ();
			}
		}

		protected virtual void DoPause ()
		{
		}
		//被其他局部页面覆盖


		public void Resume (UIPage coverPage)
		{
			if (InScreen) {
				DoResume (coverPage);
				ResumeChilds (coverPage);
			}
		}

		protected virtual void DoResume (UIPage coverPage)
		{
			
		}
		//回到最前端

		public void MoveOutScreen(bool force){

			if (InScreen) {
				AnimHide ();
				TimeEventManager.Delete (ref animCoroutine);
				animCoroutine = UIPageManager.Instance.timeEventManager.CreateEvent (Hide, 0.3f);
			} else {
				Hide ();
			}

			DoMoveOutScreen ();
			InScreen = false;
		}
			
		TimeEventHandler animCoroutine;

		void AnimHide(){
			animing = true;
			var width = UIManager.Instance.GetUIWidth ()*1.5f;
			this.GetComponent<RectTransform> ().DOAnchorPos (new Vector2 (-width, 0), 0.3f);
			this.alphaCtrl.group.DOFade (0, 0.3f);
		}

		protected virtual void DoMoveOutScreen(){

		}

		public void MoveInScreen(bool force,string options){

			XZXDDebug.Log (this.name+" MoveInScreen");
			OptionString = options;

			if (!InScreen) {
				AnimShow ();
				TimeEventManager.Delete (ref animCoroutine);
				animCoroutine = UIPageManager.Instance.timeEventManager.CreateEvent (Show, 0.3f);
			} else {
				Show ();
			}

			DoMoveInScreen ();
			InScreen = true;
			XZXDDebug.Log (this.name+" MoveInScreen Done");
		}

		void AnimShow ()
		{
			animing = true;
			// SoundManager.Instance.PlayClip ("audio/ui/battle_switch");
			var width = UIManager.Instance.GetUIWidth ()*1.5f;
			this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (width, 0);
			this.GetComponent<RectTransform> ().DOAnchorPos (Vector2.zero, 0.3f);
			this.alphaCtrl.group.DOFade (1, 0.3f);
		}


		protected virtual void DoMoveInScreen(){

		}

		UIAlphaCtrl _alphaCtrl;
		public UIAlphaCtrl alphaCtrl{
			get{
				if (_alphaCtrl == null) {
					_alphaCtrl = this.gameObject.AddComponent<UIAlphaCtrl> ();
				}
				return _alphaCtrl;
			}
		}


		public void Show(){
			animing = false;
			if (this.owner.GetComponent<UICanvasOrderRoot> () != null) {
				this.owner.GetComponent<UICanvasOrderRoot> ().Reset ();
			}
			this.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		}

		public void Hide(){
			animing = false;
			this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (outScreenPos, 0);
		}

	}
}