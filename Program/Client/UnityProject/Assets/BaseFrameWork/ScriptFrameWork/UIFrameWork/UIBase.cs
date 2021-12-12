using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace XZXD.UI
{
	public class UIBase : UIPart
	{

		public static float outScreenStartPos = 10000f;
		public float outScreenPos = 10000f;

		//dynamic
		protected Dictionary<Type, UISubPage> uiSubPages = new Dictionary<Type, UISubPage> ();
		public UISubPage CurSubPage{ protected set; get; }
		/// <summary>
		/// 必须在Load时候调用
		/// </summary>
		/// <param name="res">Res.</param>
		/// <param name="parentGo">Parent go.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected T AddSubChild<T> (string res, GameObject parentGo) where T : UISubPage
		{
			GameObject handler = UITools.LoadUIObject (res, parentGo.transform);
			UnityTools.SetFullScreenParent (handler.transform, parentGo.transform);
			T uisubPage = handler.GetComponent<T> ();
			uiSubPages.Add (typeof(T), uisubPage);
			if (null != uisubPage) {
				uisubPage.owner = this;
				uisubPage.outScreenPos = outScreenStartPos;
				outScreenStartPos += 1000;
				uisubPage.Load ();
			}
			return uisubPage;
		}

		/// <summary>
		/// 由名字获得SubPage
		/// </summary>
		public T GetSubPageByKay <T>()where T : UISubPage
		{
			if (uiSubPages.ContainsKey (typeof(T))) {
				return uiSubPages [typeof(T)] as T;
			}
			return null;
		}


		protected void ResumeChilds(UIPage coverPage){

			//子控件OnResume()
			foreach (UISubPage ctrl in uiSubPages.Values) {
				if (ctrl == null)
					continue;
				ctrl.Resume (coverPage);
			}
		}

		protected void PauseChilds(){
			//子控件OnPause()
			foreach (UISubPage ctrl in uiSubPages.Values) {
				if (ctrl == null)
					continue;
				ctrl.Pause ();
			}

		}



		protected void OpenChilds(){

			foreach (UISubPage ctrl in uiSubPages.Values) {
				if (ctrl == null)
					continue;
				ctrl.Open ();
			}
		}

		protected void LoadChilds(){

		}

		protected void DestroyChilds(){
			//
			foreach (UISubPage ctrl in uiSubPages.Values) {
				if (ctrl == null)
					continue;
				ctrl.Destroy ();
			}
			uiSubPages.Clear ();

		}

		protected void CloseChilds(){
			// 移出屏幕
			foreach (UISubPage ctrl in uiSubPages.Values) {
				if (ctrl == null)
					continue;
				ctrl.Close ();
			}
		}


		protected void MoveInSubPage<T>(bool force = false,string options = "")where T : UISubPage
		{
			var type = typeof(T);
			if (!uiSubPages.ContainsKey (type)) {
				var subPage = CheckOpenedSub <T>();
				subPage.Open ();
			}
			if (uiSubPages [type].InScreen && !force) {
				return;
			}

			foreach (var kv in uiSubPages) {
					if (kv.Key == type) {
					CurSubPage = kv.Value;
					kv.Value.MoveInScreen (force,options);
				} else {
					kv.Value.MoveOutScreen (force);
				}
			}
		}

		protected virtual T CheckOpenedSub<T> ()where T : UISubPage
		{
			return null;
		}

		/// <summary>
		/// optionString分隔符
		/// </summary>
		private readonly string m_optionsSpliteStr = "&";

		/// <summary>
		/// 操作字符串
		/// </summary>
		protected string m_optionString;

		/// <summary>
		/// 存放解析字符串
		/// </summary>
		protected Dictionary<string, string> m_options = new Dictionary<string, string> ();

		//
		public string OptionString {
			get {
				return m_optionString;
			}

			set {
				m_optionString = value;
				ParseOptString ();
			}
		}


		// 解析操作字符串
		protected void ParseOptString ()
		{
			m_options.Clear ();
			string[] strArr = Regex.Split (m_optionString, m_optionsSpliteStr, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			foreach (string s in strArr) {
				if (s.Contains ("=")) {
					string[] strV = s.Split (new char[1] { '=' });

					if (m_options.ContainsKey (strV [0])) {
						m_options [strV [0]] = strV [1];
					} else {
						m_options.Add (strV [0], strV [1]);
					}
				}
			}
		}

		public long GetLongOptionValue (string key)
		{
			string resu = "";
			if (m_options.ContainsKey (key)) {
				resu = m_options [key];
			} else {
				return 0;
			}
			return long.Parse(resu);
		}

		public int GetIntOptionValue (string key)
		{
			string resu = "";
			if (m_options.ContainsKey (key)) {
				resu = m_options [key];
			} else {
				return 0;
			}
			return int.Parse(resu);
		}

		public virtual string GetOptionValue (string key)
		{
			string resu = "";
			if (m_options.ContainsKey (key))
				resu = m_options [key];
			return resu;
		}

		public bool GetBoolOptionValue (string key, bool defaultVal = false)
		{
			string boolStr = GetOptionValue (key);
			bool ret = defaultVal;
			bool.TryParse (boolStr, out ret);
			return ret;
		}

		// 设置页面某一参数的值
		public virtual void SetOptionValue (string key, string value)
		{
			if (!m_options.ContainsKey (key)) {
				m_options.Add (key, value);
			} else {
				m_options [key] = value;
			}

			//
			SaveOptString ();
		}


		public void SetBoolOptionValue (string key, bool val)
		{
			SetOptionValue (key, val ? "true" : "false");
		}

		//
		protected void SaveOptString ()
		{
			m_optionString = "";
			foreach (KeyValuePair<string, string> opt in m_options) {
				m_optionString += opt.Key + "=" + opt.Value + "&";
			}


		}

		protected virtual void OnSaveOptString(){

		}

		public void CurrentPageTick(float deltaTime){
			OnCurrentPageTick (deltaTime);
			if (CurSubPage != null) {
				CurSubPage.CurrentPageTick (deltaTime);
			}
		}

		protected virtual void OnCurrentPageTick (float deltaTime)
		{
			
		}

		public void HelpPageInfo(){
			UIPageManager.Instance.OpenPage ("HelpPage", "title="+this.name+"_title&content="+this.name+"_content");
		}
	}
}

