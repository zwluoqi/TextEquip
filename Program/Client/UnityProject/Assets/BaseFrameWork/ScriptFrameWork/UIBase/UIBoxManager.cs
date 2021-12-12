// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using System.Text;
//
//
// namespace XZXD.UI
// {
// 	public class UIBoxManager
// 	{
// 		private long boxCount;
//
// 		private static UIBoxManager m_Instance;
//
// 		public static UIBoxManager Instance {
// 			get {
// 				if (m_Instance == null) {
// 					m_Instance = new UIBoxManager ();
// 					m_Instance.Init ();
// 				}
//
// 				return m_Instance;
// 			}
// 		}
//
// 		public static void Release ()
// 		{
// 			//TODO
// 			if (m_Instance != null) {
// 				GameObject.Destroy (m_Instance.multipopMessage.gameObject);
// 				m_Instance = null;
// 			} else {
//
// 			}
// 		}
//
// 		MulityPopMessage multipopMessage;
// 		private void Init ()
// 		{
// 			boxCount = 0;
// 			var go = UITools.LoadUIObject ("ui/pages/uibox/MulityPopMessage", UIManager.Instance.GetLayerRoot (UILayerEnum.ATTENTION).transform);
// 			multipopMessage = go.GetComponent<MulityPopMessage> ();
// 		}
//
// 		GameObject netError;
// 		public void CreatNetError (string btnName, string content, Void_Bool callBack)
// 		{
// 			if (netError == null) {
// 				netError = UITools.LoadUIObject ("ui/pages/uibox/NetErrorBox", UIManager.Instance.GetLayerRoot (UILayerEnum.NETWORK).transform);
// 				NetErrorBox netErrorBox = netError.GetComponent<NetErrorBox> ();
// 				netErrorBox.Init (btnName, content, callBack);
// 			}
// 		}
//
//
//
// 		/// <summary>
// 		/// 创建 只有一个按钮的box
// 		/// </summary>
// 		/// <param name="btnName"></param>
// 		/// <param name="content"></param>
// 		/// <param name="callBack"></param>
// 		public void CreatOneButtonBox (string btnName, string content, Void_Bool callBack)
// 		{
// 			boxCount++;
// 			OneButtonBox page = UIPageManager.Instance.OpenPage ("UIBox/OneButtonBox@@" + boxCount, "") as OneButtonBox;
// 			page.Init (btnName, content, callBack);
// 		}
//
// 		public void CreatOneButtonBox (string title, string btnName, string content, Void_Bool callBack)
// 		{
// 			boxCount++;
// 			OneButtonBox page = UIPageManager.Instance.OpenPage ("UIBox/OneButtonBox@@" + boxCount, "") as OneButtonBox;
// 			page.Init (title,btnName, content, callBack);
// 		}
//
// 		public void CreatCanCloseOneButtonBox (string title, string btnName, string content, Void_Bool callBack)
// 		{
// 			boxCount++;
// 			OneButtonBox page = UIPageManager.Instance.OpenPage ("UIBox/OneCloseButtonBox@@" + boxCount, "") as OneButtonBox;
// 			page.Init (title,btnName, content, callBack);
// 		}
//
// 		/// <summary>
// 		/// 创建  yes no Box  默认左边  yes 右边no
// 		/// </summary>
// 		/// <param name="yes"></param>  确定
// 		/// <param name="no"></param>   取消
// 		/// <param name="content"></param>   内容
// 		/// <param name="isChange"></param>   是否改变位置
// 		public YesAndNoBox CreatYesAndNoBox (string yes, string no, string content, Void_Bool callBack, bool isChange = false)
// 		{
// 			boxCount++;
// 			YesAndNoBox page = UIPageManager.Instance.OpenPage ("UIBox/YesAndNoBox@@" + boxCount, "") as YesAndNoBox;
// 			page.Init (yes, no, content, callBack, isChange);
// 			return page;
// 		}
//
// 		public YesAndNoBox CreatYesAndNoBox (string content, Void_Bool callBack)
// 		{
// 			return CreatYesAndNoBox ("确定", "取消", content, callBack);
// 		}
//
//         public ThreeBox CreatThreeBox(string one, string two,string three,string content, Void_Int callBack)
//         {
//             boxCount++;
//             ThreeBox page = UIPageManager.Instance.OpenPage("UIBox/ThreeBox@@" + boxCount, "") as ThreeBox;
//             page.Init(one, two,three, content, callBack);
//             return page;
//         }
//
// 		public YesAndNoBox CreatYesAndNoBoxBuyRuby (string content)
// 		{
// 			return CreatYesAndNoBox ("确定", "取消", content, delegate(bool bo) {
// 				if(bo){
// 					UIPageManager.Instance.OpenPage("RechargeRubyPage","");
// 				}
// 			});
// 		}
//
// 		public CostNumPage CreatCostNumPage (string yes, string no, string content, System.Action<bool,int> callBack, bool isChange = false)
// 		{
// 			CostNumPage page = UIPageManager.Instance.OpenPage ("CostNumPage", "") as CostNumPage;
// 			page.InitCost (yes, no, content, callBack, isChange);
// 			return page;
// 		}
//
// 		public void CreatePopMessage (string val)
// 		{
// 			multipopMessage.ShowMessage(val);
// 		}
//
// 		public void CreateMulityPopMessage (StringBuilder val)
// 		{
// 			multipopMessage.ShowMessage(val);
// 		}
//
// 		public void CreateMulityPopMessage (string[] val)
// 		{
// 			multipopMessage.ShowMessage(val);
// 		}
//
// 		public void ShowTips (string info)
// 		{
// 			multipopMessage.ShowTip(info);
// 		}
//
// 		public bool IsShowTip ()
// 		{
// 			return multipopMessage.IsShowTip ();
// 		}
//
// 		/// <summary>
// 		/// 多重奖励界面
// 		/// </summary>
// 		/// <param name="btnName"></param>
// 		/// <param name="content"></param>
// 		/// <param name="callBack"></param>
// 		public RewardPage CreatRewardPage (List<Drop> drops,Vector3 pos)
// 		{
// 			SoundManager.Instance.PlayClip ("audio/ui/Get_reward");
// 			boxCount++;
// 			RewardPage page = UIPageManager.Instance.OpenPage ("RewardPage@@" + boxCount, "",pos) as RewardPage;
// 			page.ShowRewards (drops);
// 			return page;
// 		}
//
// 		public void CreatRewardPage (List<Drop> drops)
// 		{
// 			CreatRewardPage ( drops,Vector3.zero);
// 		}
//
// 		public RewardPrePage CreatPreRewardPage (List<Drop> drops)
// 		{
// //			CreatRewardPage ( drops,Vector3.zero);
// //			SoundManager.Instance.PlayClip ("audio/ui/Get_reward");
// 			boxCount++;
// 			RewardPrePage page = UIPageManager.Instance.OpenPage ("RewardPrePage@@" + boxCount, "",Vector3.zero) as RewardPrePage;
// 			page.ShowRewards (drops);
// 			return page;
// 		}
//
//
// 		public void CreatRewardPage (Drop drop)
// 		{
// 			List<Drop> drops = new List<Drop>();
// 			drops.Add (drop);
// 			CreatRewardPage ( drops,Vector3.zero);
// 		}
//
//
// 		public void ShowRewardPage (Vector3 position,List<Drop> list)
// 		{
// 			CreatRewardPage ( list,position);
// 		}
//
// 		public void CreatePopUIParticle (string url)
// 		{
// 			var go = GameObjectPoolManager.Instance.GetGameObjectDirect (url);
// 			if (go == null) {
// 				return;
// 			}
// 			UnityTools.SetParent (go.transform, UIManager.Instance.GetLayerRoot (UILayerEnum.ATTENTION).transform);
// 			var sortEffect = go.AddMissingComponent<UISortEffectComponent> ();
// 			sortEffect.sortLayerName = UIManager.Instance.GetCanvasLayerName (UILayerEnum.ATTENTION);
// 			sortEffect.orderSort = 1;
// 			sortEffect.page = null;
// 			sortEffect.FreshRenderSorrOrder ();
// 				
// 			var move = go.AddMissingComponent<MoveToTop> ();
// 			move.Move ();
// 			GameObjectPoolManager.Instance.Unspawn (go, 3);
// 		}
//
// 		public void CreateUIParticle (string url, Transform parent)
// 		{
// 			var go = GameObjectPoolManager.Instance.GetGameObjectDirect (url);
// 			UnityTools.SetParent (go.transform, parent);
// 			var sortEffect = go.AddMissingComponent<UISortEffectComponent> ();
// 			var canvas = parent.GetComponentInParent<Canvas> ();
// 			sortEffect.sortLayerName = canvas.sortingLayerName;
// 			sortEffect.orderSort =  10;
// 			sortEffect.page = null;
// 			sortEffect.FreshRenderSorrOrder ();
//
// 			GameObjectPoolManager.Instance.Unspawn (go, 3);
// 		}
//
//
// 		public GameObject netMask = null;
// 		public int netMaskCount = 0;
//
// 		public void CreateNetMask ()
// 		{
// 			netMaskCount++;
// 			if (netMask == null) {
// 				netMask = UITools.LoadUIObject ("ui/pages/uibox/NetMask", UIManager.Instance.GetLayerRoot (UILayerEnum.NETWORK).transform);
// 				UnityTools.SetFullScreenParent (netMask.transform, netMask.transform.parent);
// 			}
// 		}
//
// 		public void ClearNetMask ()
// 		{
// 			netMaskCount--;
// 			if (netMask != null) {
// 				GameObject.Destroy (netMask);
// 			}
// 			netMask = null;
// 		}
//
// 		private LoginWeb loginWeb;
// 		public void OpenGongGao (string url)
// 		{
// 			Debug.LogWarning ("OpenGongGao:" + url);
// 			CloseGonggao ();
// 			var go = UITools.LoadUIObject ("ui/component/WebPage", UIManager.Instance.GetLayerRoot (UILayerEnum.PLATFORM).transform);
// 			UnityTools.SetFullScreenParent (go.transform, UIManager.Instance.GetLayerRoot (UILayerEnum.PLATFORM).transform);
//             loginWeb = go.GetComponent<LoginWeb>();
//             loginWeb.Show(url);
// 		}
//
// 		public void CloseGonggao(){
// 			if (loginWeb != null) {
// 				loginWeb.OnCloseWeb ();
// 			}
// 		}
//
// 		public void OpenSimpleGongGao ()
// 		{
// //			Debug.LogWarning ("OpenSimpleGongGao:" + url);
// //			CloseGonggao ();
// //			var go = UITools.LoadUIObject ("ui/component/SimpleWebPage", UIManager.Instance.GetLayerRoot (UILayerEnum.PLATFORM).transform);
// //			UnityTools.SetFullScreenParent (go.transform, UIManager.Instance.GetLayerRoot (UILayerEnum.PLATFORM).transform);
// 			var go = UIPageManager.Instance.OpenPage("SimpleWebPage","");
// 			var simple = go.GetComponent<LoginSimpleWeb>();
// 			simple.Init();
// 		}
//
// 		private UIMaskPanel m_MaskAni = null;
// 		public void CreatMaskPanel (int type, float time)
// 		{
// 			if (m_MaskAni == null || m_MaskAni.gameObject == null)
// 			{
// 				m_MaskAni = UITools.LoadUIObject("ui/component/UIMaskPanel",UIManager.Instance.GetLayerRoot( UILayerEnum.GUIDER)).GetComponent<UIMaskPanel>();
// 			}
//
// 			m_MaskAni.Init(type, time);
// 		}
//
// 		public void CreatGudeNextPage (bool b)
// 		{
// 			//TODO
// 		}
//
// 		public GameObject OpenRedBagCtrl ()
// 		{
// //			Debug.LogWarning ("OpenGongGao:" + url);
// //			CloseGonggao ();
// 			var go = UITools.LoadUIObject ("ui/component/RedBagCtrl", UIManager.Instance.GetLayerRoot (UILayerEnum.ATTENTION).transform);
// 			UnityTools.SetFullScreenParent (go.transform, UIManager.Instance.GetLayerRoot (UILayerEnum.ATTENTION).transform);
// 			return go;
// //			loginWeb = go.GetComponent<LoginWeb>();
// //			loginWeb.Show(url);
// 		}
// 	}
// }
