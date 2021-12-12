// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:UIToggleGroupComponent.cs
// * Created:2017/12/11
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using XZXD.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleGroupComponent:MonoBehaviour
{

	public ToggleGroup tg;
	protected List<ToggleHelper> toggles = new List<ToggleHelper>();
	protected ToggleHelper currentToggle;
	Action<int> freshCurrentToggleUI;

	public void DoLoad (Action<int> callBack)
	{
		toggles = new List<ToggleHelper> ();
		this.tg.GetComponentsInChildren<ToggleHelper> (true, toggles);
		for (int i = 0; i < toggles.Count; i++) {
			// var redTipBtn = toggles [i].GetComponent<RedTipBtn> ();
			// if (redTipBtn != null) {
			// 	redTipBtn.SetTip (false);
			// }
			toggles [i].SetOff ();
			toggles [i].toggleCallBack = ((go, arg0) => OnValueChanged (go, arg0));
			toggles [i].GetComponent<Toggle> ().group = tg;
		}
		currentToggle = toggles [0];
		currentToggle.SetOn ();
		freshCurrentToggleUI = callBack;
	}

	public void DoClears ()
	{
		currentToggle = null;
		freshCurrentToggleUI = null;
		foreach (var item in toggles) {
			GameObject.DestroyImmediate (item.gameObject);
		}
		toggles.Clear ();
	}

	public void OpenWithToggleIndex (int openToggle)
	{
		for (int i = 0; i < toggles.Count; i++) {
			if (openToggle != i) {
				toggles [i].SetOff ();
			} else {
				toggles [i].SetOn ();
			}
		}

		currentToggle = toggles [openToggle];
		FreshToggleUI ();
	}

	public void FreshToggleUI ()
	{
		OnValueChanged (currentToggle, true);
	}

	void OnValueChanged (ToggleHelper th, bool isOn)
	{
		if (isOn) {
			XZXDDebug.LogWarning ("index:" + th.name + " ison:" + isOn);
			int index = toggles.IndexOf (th);

			currentToggle = th;

			OnFreshCurrenToggleUI (index);
		}
	}



	void OnFreshCurrenToggleUI (int index)
	{
		if (freshCurrentToggleUI != null) {
			freshCurrentToggleUI (index);
		}
	}

	internal int GetCurrentToggleIndex ()
	{
		int index = toggles.IndexOf (currentToggle);
		return index;
	}
	internal string GetCurrentToggleTag ()
	{
		return currentToggle.toggleTag;
	}

	// public RedTipBtn GetRedTipByIndex(int index){
	// 	return toggles [index].GetComponent <RedTipBtn>();
	// }

	public int GetToggleCount(){
		return toggles.Count;
	}
}
