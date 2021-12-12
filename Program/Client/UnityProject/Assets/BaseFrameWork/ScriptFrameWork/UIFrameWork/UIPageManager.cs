using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace XZXD.UI
{
	public class UIPageManager:UIBase
	{
		//
		private static UIPageManager m_instance;

		public static UIPageManager Instance {
			get {
				if (m_instance == null) {
					GameObject go = new GameObject ("UIPageManager");
					GameObject.DontDestroyOnLoad (go);
					m_instance = go.AddComponent<UIPageManager> ();

					m_instance.Init ();
				}

				return m_instance;
			}
		}

		public static void Release ()
		{
			if (m_instance != null) {
				m_instance.Destroy ();
				GameObject.Destroy (m_instance.gameObject);

			}
			m_instance = null;
		}

		public TimeEventManager timeEventManager = new TimeEventManager();

		/// <summary>
		/// 页面完整信息
		/// </summary>
		[System.Serializable]
		public class PageStackData
		{
			public string pageName;
			public string pageOption;
			public UIPage.PageTypeEnum type;
			public Vector3 openWorldPos;
			//
			public PageStackData (string pageName, string pageOption, UIPage.PageTypeEnum type, Vector3 openWorldPos)
			{
				this.pageName = pageName;
				this.pageOption = pageOption;
				this.type = type;
				this.openWorldPos = openWorldPos;
			}

			//			public PageStackData (string pageName, string pageOption, UIPage.PageTypeEnum type)
			//			{
			//				this.pageName = pageName;
			//				this.pageOption = pageOption;
			//				this.type = type;
			//				this.openWorldPos = Vector3.zero;
			//			}

			//
			public override string ToString ()
			{
				return pageName + UIPageManager.m_spliteStr + pageOption + UIPageManager.m_spliteStr + (int)type + UIPageManager.m_spliteStr;
			}
		}

		/// <summary>
		/// // 永驻内存中的页面
		/// </summary>
		private Dictionary<string, UIPage> m_memoryPages = new Dictionary<string, UIPage> ();

		public List<PageStackData> m_pageStack = new List<PageStackData> ();
		// 操作栈
		private List<PageStackData> m_backupStack;
		// 备份堆栈

		public static readonly string m_spliteStr = "##";
		// 分隔符

		private readonly string m_samePageSuffix = "@@";
		// "pageName" "pageName@@任意字符" 使用同一个预设体的不同对象


		//
		private void Init ()
		{
			timeEventManager = new TimeEventManager();;
			timeEventManager.Start();
		}

		//
		private void Destroy ()
		{
			//
			CloseAllPage ();
			DestoryMemoryPages ();
	
		}

		private void PreloadAlwaysInMemoryPage ()
		{

		}

		//
		public void CloseAllPage ()
		{
			for (int i = m_pageStack.Count - 1; i >= 0; i--) {
				//
				UIPage page = GetPageFromKey (m_pageStack [i]);
				if (page != null && page.IsOpened ()) {
					DoClosePage (page);
				}
			}

			//
			m_pageStack.Clear ();
//			m_pageDictonary.Clear ();

          
		}

		public void DestoryMemoryPages ()
		{
			foreach (UIPage page in m_memoryPages.Values) {
				if (page == null) {
					Debug.LogError ("memory page be destory in error mode");
				} else {
					page.Destroy ();
				}
			}

			//
			m_memoryPages.Clear ();
		}


		/// <summary>
		/// 打开堆栈页面
		/// </summary>
		private void OpenStackPages ()
		{
			//
			Stack<PageStackData> openStack = new Stack<PageStackData> ();

			//
			for (int i = m_pageStack.Count - 1; i >= 0; i--) {
				//
				PageStackData key = m_pageStack [i];
				openStack.Push (key);
				m_pageStack.RemoveAt (i);

				//
				UIPage.PageTypeEnum pageType = key.type;
				if (pageType == UIPage.PageTypeEnum.FULL_SCREEN) {
					break;
				}
			}

			// 打开窗口
			while (openStack.Count > 0) {
				PageStackData key = openStack.Pop ();

				OpenPage (key.pageName, key.pageOption, key.openWorldPos);
			}
		}

		// 打开index之前的页面
		private void OpenStackPages (int index)
		{
			//
			Stack<PageStackData> openStack = new Stack<PageStackData> ();

			//
			for (int i = index - 1; i >= 0; i--) {
				//
				PageStackData key = m_pageStack [i];
				openStack.Push (key);
				m_pageStack.RemoveAt (i);

				//
				UIPage.PageTypeEnum pageType = key.type;
				if (pageType == UIPage.PageTypeEnum.FULL_SCREEN) {
					break;
				}
			}

			int startIndex = index - openStack.Count;

			// 打开窗口
			while (openStack.Count > 0) {
				PageStackData key = openStack.Pop ();

				OpenPageForInnerStack (key.pageName, key.pageOption, key.openWorldPos, startIndex);
				startIndex++;
			}
		}

		/// <summary>
		/// 当前页（栈顶页）
		/// </summary>
		public UIPage CurPage {
			get {
				// 栈顶页
				return GetPageFromKey (StackTopKey ());
			}
		}

		// 加载一个页面
		private UIPage LoadPage (string pageName)
		{
			if (pageName.Length == 0) {
				return null;
			}

			UIPage page = GetPage (pageName);
			if (page != null) {
				return page;
			}

			//
			string path = "UI/Pages/" + GetPrefabName (pageName);
			GameObject obj = UITools.LoadUIObject (path, null);
			if (obj != null) {
				obj.name = pageName;
				obj.transform.localScale = Vector3.one;

				//
				page = obj.GetComponent<UIPage> ();
				if (page == null) {
					Debug.LogError ("Page " + pageName + " Not Add UIPage Script");
					return null;
				}
				Canvas canvas = obj.AddMissingComponent<Canvas> ();
				UnityTools.SetFullScreenParent (obj.transform, UIManager.Instance.GetLayerRoot (UILayerEnum.MAIN));
				UIManager.Instance.SetCanvasLayer (canvas, UILayerEnum.MAIN, 0);


				obj.AddMissingComponent<GraphicRaycaster> ();

				page.Load ();

				page.canvasData = obj.AddMissingComponent<UICanvasOrderRoot> ();
				page.canvasData.Init ();

				// 常驻内存的界面
				AddMemeryPage (pageName, page);
//
//				if (page.IsAlwaysInMemery ()) {
//					
//				} else {
//					AddPage (pageName, page);
//				}
			} else {
				Debug.LogError ("Cannot find the page " + pageName);
			}

			return page;
		}

		/// <summary>
		/// 获取预设体的名字
		/// </summary>
		/// <param name="pageName"></param>
		/// <returns></returns>
		private string GetPrefabName (string pageName)
		{
			string[] subStrings = Regex.Split (pageName, m_samePageSuffix, RegexOptions.IgnoreCase);
			if (subStrings.Length > 0) {
				return subStrings [0];
			} else {
				return pageName;
			}
		}


		// 获取一个页面
		public UIPage GetPage (string pageName)
		{
			UIPage page = null;

			//
//			if (m_pageDictonary.ContainsKey (pageName)) {
//				page = m_pageDictonary [pageName];
//			}

			if (m_memoryPages.ContainsKey (pageName)) {
				page = m_memoryPages [pageName];
			}

			return page;
		}

		public bool GetStackPage (string pageName) 
		{
			
			foreach (PageStackData page in m_pageStack) {
				if (page.pageName == pageName) {
					return true;
				}
			}

			return false;
		}

		//
		public UIPage GetPageFromKey (PageStackData pageKey)
		{
			if (pageKey == null)
				return null;
			return GetPage (pageKey.pageName);
		}

		//
		public T GetPage<T> () where T : UIPage
		{
			T retPage = null;

			//
			foreach (UIPage page in m_memoryPages.Values) {
				retPage = page.gameObject.GetComponent<T> ();
				if (retPage != null) {
					return retPage;
				}
			}

			return retPage;
		}

		public UIPage OpenPage (string pageName, string option)
		{
			return OpenPage (pageName, option, Vector3.one);
		}

		/// <summary>
		/// 打开某个页面
		/// page页面必须放入Page文件夹下
		/// </summary>
		/// <param name="pageName"></param>
		/// <param name="option"></param>
		/// <returns></returns>
		public UIPage OpenPage (string pageName, string option, Vector3 openWorldPos)
		{
			//

			UIPage currentPage = CurPage;
			if (currentPage != null && currentPage.Name == pageName) {
				currentOrder += 100;
				currentPage.canvasData.ResetAddtionalOrder (currentOrder);

				currentPage.Show ();
				currentPage.Reopen (option, openWorldPos);
				return currentPage;
			}


			//
			UIPage pageToOpen = LoadPage (pageName);
			if (pageToOpen == null) {
				return null;
			}

			//
			if (currentPage != null) {
				// 不同类型窗口的处理
				switch (pageToOpen.PageType) {
				// 关闭所有打开的界面
				case UIPage.PageTypeEnum.FULL_SCREEN:
					{
						for (int i = m_pageStack.Count - 1; i >= 0; i--) {
							PageStackData curPageKey = m_pageStack [i];

							//
							UIPage curPage = GetPageFromKey (curPageKey);
							if (curPage == null) {
								continue;
							}

							//
							if (curPage.IsOpened ()) {
								DoClosePage (curPage);
							} else {
								break;
							}
						}
					}
					break;

				//
				case UIPage.PageTypeEnum.COVER_SCREEN:
					{
						currentPage.OnCoverPageOpen (pageToOpen);
					}
					break;

				default:
					break;
				}
			}


			// 当前页入栈，并且打开
			StackPush (new PageStackData (pageName, option, pageToOpen.PageType, openWorldPos));

//			SoundManager.Instance.PlayClip ("audio/ui/Open_jiemian");
			DoOpenPage (pageToOpen, option, openWorldPos);

 
			//
			return pageToOpen;
		}

		/// <summary>
		///  内部插入打开：!!!:特殊用途
		/// </summary>
		/// <param name="pageName"></param>
		/// <param name="option"></param>
		/// <returns></returns>
		private UIPage OpenPageForInnerStack (string pageName, string option, Vector3 openWorldPos, int index)
		{
			//
			UIPage pageToOpen = LoadPage (pageName);
			if (pageToOpen == null) {
				return null;
			}

			// 不同类型窗口的处理
			switch (pageToOpen.PageType) {
			// 关闭所有打开的界面
			case UIPage.PageTypeEnum.FULL_SCREEN:
				{
					// 不做任何处理，因为内部界面已经处于关闭状态
				}
				break;

			//
			case UIPage.PageTypeEnum.COVER_SCREEN:
				{
					// 底部界面
					PageStackData stackData = StackIndex (index - 1);
					if (stackData != null) {
						UIPage currentPage = GetPageFromKey (stackData);
						if (currentPage != null) {
							currentPage.OnCoverPageOpen (pageToOpen);
						}
					}
				}
				break;

			default:
				break;
			}

			// 当前页入栈，并且打开
			StackPushIndex (new PageStackData (pageName, option, pageToOpen.PageType, openWorldPos), index);
			DoOpenPage (pageToOpen, option, openWorldPos);

			//
			return pageToOpen;
		}

		public int currentOrder = 0;
		//
		private void DoOpenPage (UIPage page, string option, Vector3 openWorldPos)
		{
			// 增加到UI层级中
			currentOrder += 100;
			page.canvasData.ResetAddtionalOrder (currentOrder);

			if (page.IsOpened ()) {
				page.Show ();

				page.Reopen (option, openWorldPos);

			} else {
				page.Show ();

				page.Open (option, openWorldPos);
				//

			}
		}


		//
		private void DoClosePage (UIPage page)
		{
			currentOrder -= 100;
			currentOrder = Mathf.Max (0, currentOrder);
			//
			if (page.IsAlwaysInMemery ()) {
				page.Hide ();
				page.PgaeToClose ();
			} else {
				RemoveMemeryPage (page.Name);

				//
				page.Hide ();
				page.PgaeToClose ();
				page.Destroy ();

			}
		}


		// 关闭一个页面，暂且不支持关闭非顶层页面
		public void ClosePage (UIPage page)
		{
			//
			if (page == null) {
				return;
			}

			string pageName = page.name;




			//
			if (CurPage == page) {
				//当前页面是最下面一个页面时，不能用普通手段关闭
				if (m_pageStack.Count == 1) {
					return;
				}

				// 当前页出栈
				StackPop ();

				// 当前页面类型
				switch (page.PageType) {
				// 覆盖窗口，告知窗口移出即可
				case UIPage.PageTypeEnum.COVER_SCREEN:
					{
						UIPage curPage = CurPage;
						//
						if (curPage != null) {
							curPage.OnCoverPageRemove (page);
						}
					}
					break;

				// 全屏，则找到下一个全屏窗口，一一打开
				case UIPage.PageTypeEnum.FULL_SCREEN:
					{
						OpenStackPages ();
					}
					break;

				default:
					break;
				}

				//
				DoClosePage (page);
			} else {

				// 界面是否存活
//				if (m_pageDictonary.ContainsKey (page.Name)) {
				//
//					PageStackData stackData = new PageStackData (page.name, page.OptionString, page.PageType,page.OpenWorldPos);

				//
				int index = StackRemove (page.Name);
				if (index == -1) {
					Debug.LogError ("删除的页面，堆栈中不存在 :" + page.Name);
					return;
				}

				// 非底层的页面才需要处理
				if (index > 0) {
					// 当前页面类型
					switch (page.PageType) {
					// 覆盖窗口，告知窗口移出即可
					case UIPage.PageTypeEnum.COVER_SCREEN:
						{
							//
							PageStackData curStackData = StackIndex (index);
							UIPage curPage = GetPageFromKey (curStackData);

							//
							if (curPage != null) {
								curPage.OnCoverPageRemove (page);
							}
						}
						break;

					// 全屏，则找到下一个全屏窗口，一一打开
					case UIPage.PageTypeEnum.FULL_SCREEN:
						{
							OpenStackPages (index);
						}
						break;

					default:
						break;
					}
				}


				//
				DoClosePage (page);

//				} else {
//					// 暂且不准许删除非活着的界面
//					Debug.LogError ("不能关闭非复活的页面 :" + page.Name);
//				}
			}
   
		}

		// 关闭所有page，只保留栈底的page
		public void ClosePagesRetainBottom ()
		{
			// 如果已经是栈底了，不处理
			if (m_pageStack.Count == 1) {
				return;
			}

			while (m_pageStack.Count > 1) {
				UIPage page = GetPageFromKey (m_pageStack [m_pageStack.Count - 1]);
				ClosePage (page);
			}
		}

		//关闭所有page，直到遇到需要的page
		public void ClosePagesRetainPage (string pageName)
		{
			// 如果已经是栈底了，不处理
			if (m_pageStack.Count == 1) {
				return;
			}

			while (m_pageStack.Count > 1) {
				UIPage page = GetPageFromKey (m_pageStack [m_pageStack.Count - 1]);
				if (page.Name == pageName) {
					break;
				}
				ClosePage (page);
			}
		}


		// 保存page的options,暂不支持非顶层页面
		public void SavePageOption (UIPage page, string option)
		{
			//
			if (page == null) {
				return;
			}

			//
			if (CurPage == page) {
				PageStackData key = StackTopKey ();

				//
				PageStackData newKey = new PageStackData (key.pageName, option, page.PageType, page.OpenWorldPos);

				// 更换新的key值
				StackPop ();
				StackPush (newKey);
			} else {
				// 暂且不准许跨层保存option
				// LogTools.LogError("Can't Save Page Option That Is Not At Top:" + page.Name);
			}
		}

		//
		private string GetPageKey (string pageName, string pageOption, UIPage.PageTypeEnum type)
		{
			return pageName + m_spliteStr + pageOption + m_spliteStr + (int)type;
			;
		}


		//
//		private void AddPage (string pageName, UIPage page)
//		{
//			if (m_pageDictonary.ContainsKey (pageName)) {
//				Debug.LogError (pageName + "has already exist");
//			}
//
//			//
//			m_pageDictonary [pageName] = page;
//		}
//
		private void RemoveMemeryPage (string pageName)
		{
			if (!m_memoryPages.ContainsKey (pageName)) {
				Debug.LogError (pageName + "has not exist");
			} else {
				m_memoryPages.Remove (pageName);
			}
		}

		private void AddMemeryPage (string pageName, UIPage page)
		{
			if (m_memoryPages.ContainsKey (pageName)) {
				Debug.LogError (pageName + "has already exist");
			}
			//
			m_memoryPages [pageName] = page;
		}

 

		//
		private PageStackData StackTopKey ()
		{
			if (m_pageStack.Count > 0) {
				return m_pageStack [m_pageStack.Count - 1];
			} else {
				return null;
			}
		}

		private PageStackData StackBottomKey ()
		{
			if (m_pageStack.Count > 0) {
				return m_pageStack [0];
			} else {
				return null;
			}
		}

		private PageStackData StackPop ()
		{
			if (m_pageStack.Count > 0) {
				PageStackData top = m_pageStack [m_pageStack.Count - 1];
				m_pageStack.RemoveAt (m_pageStack.Count - 1);

				return top;
			} else {
				return null;
			}
		}


		private void StackPush (PageStackData key)
		{
			m_pageStack.Add (key);
		}

		private void StackPushIndex (PageStackData key, int index)
		{
			m_pageStack.Insert (index, key);
		}

		// 弹出某个key
		private int StackRemove (string pageName)
		{
			//
			for (int i = m_pageStack.Count - 1; i >= 0; i--) {
				if (pageName == m_pageStack [i].pageName) {
					m_pageStack.RemoveAt (i);
					return i;
				}
			}

			return -1;
		}

		//
		private PageStackData StackIndex (int index)
		{
			if (index >= 0 && index < m_pageStack.Count) {
				return m_pageStack [index];
			} else {
				return null;
			}
		}



		void Update(){
			timeEventManager.OrderUpdate(Time.deltaTime);
			if (CurPage != null) {
				CurPage.CurrentPageTick (Time.deltaTime);
			}
		}
	}
}
