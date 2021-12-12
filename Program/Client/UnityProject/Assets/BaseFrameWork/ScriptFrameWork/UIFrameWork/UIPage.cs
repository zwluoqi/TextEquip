using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using DG.Tweening;

namespace XZXD.UI
{
	public class UIPage : UIBase
	{
		/// <summary>
		/// 页面类型枚举
		/// </summary>
		public enum PageTypeEnum
		{
			FULL_SCREEN = 0,
			// 全屏窗口
			COVER_SCREEN,
			// 覆盖窗口
		}

		/// <summary>
		///  页面类型
		/// </summary>
		public PageTypeEnum m_pageType = PageTypeEnum.COVER_SCREEN;

     
		public bool useAnimOpe = true;
		public bool UseAnimOpen{
			get{
				return false;
			}
		}

		public Vector3 OpenWorldPos;


		/// <summary>
		/// 是否永久驻留在内存
		/// </summary>
		public bool m_alwaysInMemerys = true;

		UIAlphaCtrl _alphaCtrl;
		public UIAlphaCtrl alphaCtrl{
			get{
				if (_alphaCtrl == null) {
					_alphaCtrl = this.gameObject.AddComponent<UIAlphaCtrl> ();
				}
				return _alphaCtrl;
			}
		}



		/// <summary>
		/// 页面是否被打开
		/// </summary>
		protected bool m_isOpened = false;

		protected override void OnSaveOptString ()
		{
			//
			UIPageManager.Instance.SavePageOption (this, OptionString);
		}
     

		// 页面类型
		public PageTypeEnum PageType {
			get {
				return m_pageType;
			}
		}
			

		// 页面Panel
		public UICanvasOrderRoot canvasData{get;set;}

		// 页面名字
		public string Name {
			get {
				return gameObject.name;
			}

			protected set {
				gameObject.name = value;
			}
		}



		//
		public bool IsAlwaysInMemery ()
		{
			return m_alwaysInMemerys;
//			return true;
		}



		//
		public bool IsOpened ()
		{
			return m_isOpened;
		}
			
		/// <summary>
		/// 执行内存关闭
		/// 不实质性关闭，只是做一些隐藏
		/// </summary>
		public void PgaeToClose ()
		{
			CloseChilds ();
			this.m_isOpened = false;
			DoClose ();
		}

		// 显示
		public void Show ()
		{
			this.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
//			alphaCtrl.Show ();
//			gameObject.SetActive(true);

			DoShow ();
		}

		// 隐藏
		public void Hide ()
		{
			this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (outScreenPos, 0);

//			alphaCtrl.Hide ();
//			gameObject.SetActive(false);

			//
			DoHide ();
		}

		// 是否显示
		public virtual bool IsShow ()
		{
			return gameObject.activeSelf;
		}


		// 进行打开操作
		// 请勿主动调用此函数
		protected virtual void DoOpen ()
		{

		}

		// 进行重新打开操作
		// 请勿主动调用此函数
		protected virtual void DoReopen ()
		{

		}


		//销毁窗口
		public void Destroy ()
		{
			DestroyChilds ();
			NotificationCenter.Default.RemoveObserver(this);
			m_isOpened = false;
			this.DoDestroy ();
			Destroy (gameObject);
		}

		protected virtual void DoDestroy ()
		{

		}

		protected virtual void DoClose ()
		{

		}

		/// <summary>
		/// 显示的一些额外操作
		/// </summary>
		protected virtual void DoShow ()
		{
            
		}

		/// <summary>
		/// 隐藏的一些额外操作
		/// </summary>
		protected virtual void DoHide ()
		{

		}

		public void Load()
		{
			XZXDDebug.Log (this.name+" Load");
			this.outScreenPos = outScreenStartPos;
			outScreenStartPos += 1000;
			LoadChilds ();
			DoLoad ();
		}

		public virtual void DoLoad ()
		{
			
		}

		public bool IsAniming{
			get{
				return animing;
			}
		}
		bool animing = false;

		public TimeEventHandler animingCor;

		// 打开
		public void Open (string options,Vector3 openWorldPos)
		{
			//
			XZXDDebug.Log (this.name+" open");
			m_isOpened = true;

			OptionString = options;

			OpenWorldPos = openWorldPos;

			DoOpen ();

			OpenChilds ();

			if (UseAnimOpen) {
				AnimOpen ();
			}
		}

		void AnimOpen(){
			animing = true;
			this.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
			this.transform.DOScale (1, 0.4f);
			this.transform.position = OpenWorldPos;
			this.transform.DOMove (Vector3.zero, 0.4f);

			TimeEventManager.Delete (ref animingCor);
			animingCor = UIPageManager.Instance.timeEventManager.CreateEvent (AnimOpenDone, 0.4f);
		}

		void AnimOpenDone ()
		{
			this.transform.localScale = Vector3.one;
			this.transform.localPosition = Vector3.zero;
			animing = false;
		}


		// 通知PageManager关闭当前页面
		// 手动关闭页面的时候请调用此函数
		public void Close ()
		{
			if (animing) {
				return;
			}
			// SoundManager.Instance.PlayClip ("audio/ui/ui_button");
			if (UseAnimOpen) {
				AnimClose ();
			} else {
				UIPageManager.Instance.ClosePage (this);
			}
		}



		void AnimClose ()
		{
			animing = true;
			this.transform.DOScale (0.05f, 0.3f);
			this.transform.DOMove (OpenWorldPos, 0.3f);
//			RunCoroutine.Stop (animingCor);
			TimeEventManager.Delete(ref animingCor);
			animingCor = UIPageManager.Instance.timeEventManager.CreateEvent (RealClose, 0.3f);
		}

		void RealClose ()
		{
			animing = false;
			UIPageManager.Instance.ClosePage (this);
		}
			
		public void Reopen (string options,Vector3 openWorldPos)
		{
			OptionString = options;
			OpenWorldPos = openWorldPos;

			DoReopen ();
		}

		//
		public void OnCoverPageOpen (UIPage coverPage)
		{


			DoOnCoverPageOpen (coverPage);

			PauseChilds ();

		}

		protected virtual void DoOnCoverPageOpen (UIPage coverPage)
		{

		}

		public void OnCoverPageRemove (UIPage coverPage)
		{

			DoOnCoverPageRemove (coverPage);

			ResumeChilds (coverPage);
		}

		protected virtual void DoOnCoverPageRemove (UIPage coverPage)
		{

		}
			

		public void SetFormation(){
			UIPageManager.Instance.OpenPage ("SetFormationPage", "");
		}

		public void RechargeRuby(){
			UIPageManager.Instance.OpenPage ("RechargeRubyPage", "");
		}

	}
}
