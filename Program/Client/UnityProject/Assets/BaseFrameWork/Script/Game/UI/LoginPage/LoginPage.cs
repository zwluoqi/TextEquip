using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Script.Game.Grow;

namespace XZXD.UI
{
	public class LoginPage : UIPage
	{
		public LoginController controller;
		public GameObject childLogin;
		public GameObject childUpdateRes;
		public GameObject childCheckAppVersion;


		public Text childUpdateResText;
		public Slider childUpdateResSilder;
		public Text childUpdateResSpeed;

		public GameObject exchangeGate;


		public InputField loginID = null;
		public InputField loginPass = null;

		public GameObject passTip;


		public Text childUpdateTitleText;

		public UnityEngine.UI.Dropdown gameEasy;

		public GameObject selectServerItem;
		public Text curServerItem;
		public GameObject normalInputArea;
		public GameObject sdkInputArea;
		public GameObject noAccountInputArea;
		
		public Camera postCamera;

		public override void DoLoad ()
		{
#if UNITY_EDITOR
			exchangeGate.SetActive(true);
			#else
			exchangeGate.SetActive(false);
			#endif
			if (childUpdateTitleText == null) {
				var tmp_go = childUpdateRes.transform.Find ("Text (1)");
				if (tmp_go != null) {
					childUpdateTitleText = tmp_go.GetComponent<Text> ();
				}
			}
		}

		protected override void DoOpen ()
		{

			// controller = new LoginController (this);
			// Debug.LogError (System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss"));
			//

			postCamera.gameObject.SetActive(true);
		}

		void ShowNormalInputArea ()
		{
			sdkInputArea.SetActive(false);
			//if (VersionTool.rechargeMode == "none")
			//{
			//	noAccountInputArea.SetActive(true);
			//	normalInputArea.SetActive(false);
			//}
			//else
			{
				normalInputArea.SetActive(true);
				noAccountInputArea.SetActive(false);
			}
		}

		void ShowSDKInputArea ()
		{
			sdkInputArea.SetActive(true);
			normalInputArea.SetActive (false);
			noAccountInputArea.SetActive(false);
		}

		public void HideAll ()
		{
			childLogin.SetActive (false);
			childUpdateRes.SetActive (false);
			childCheckAppVersion.SetActive (false);
		}

		public void ShowWati ()
		{
			HideAll ();
			childLogin.SetActive (true);
			ShowNormalInputArea ();

		}

		protected override void DoOnCoverPageRemove (UIPage coverPage)
		{
			InitServer ();
		}

		public void InitServer(){

		}


		public void ShowCheckUpdate ()
		{
			HideAll ();
			childUpdateRes.SetActive (true);
			this.childUpdateResSilder.value = 0;
			this.childUpdateResSpeed.text = "";
		}

		public void ShowCheckAppVersion ()
		{
			HideAll ();
			childCheckAppVersion.SetActive (true);
		}

		public void ShowCheckAgree ()
		{
			HideAll ();
		}

		protected override void OnCurrentPageTick (float deltaTime)
		{
			controller.Tick (Time.deltaTime);
			if (loginID.text == loginPass.text) {
				passTip.SetActive (true);
			} else {
				passTip.SetActive (false);
			}
		}

		protected override void DoClose ()
		{
			postCamera.gameObject.SetActive(false);
		}

		public static int gongaoCount = 0;
		public void OnGongGao ()
		{
			gongaoCount++;
			if(gongaoCount>10){
				exchangeGate.SetActive (true);
			}
			BoxManager.OpenSimpleGongGao ();
		}

		
	}
}
