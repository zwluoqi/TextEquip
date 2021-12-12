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
// * Filename:UIItemPool.cs
// * Created:2018/6/7
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;

public class UIItemPool: MonoBehaviour 
{
	public bool hideMode = false;
	public void Show(){
		if (hideMode) {
			gameObject.SetActive (true);
		} else {
			//		group.alpha = 1;
			//		group.interactable = defaultinteractable;
			//		group.blocksRaycasts = defaultblocksRaycasts;
			//		this.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
			this.GetComponent<RectTransform> ().localScale = Vector3.one;
		}
	}

	public void Hide(){
		if (hideMode) {
			gameObject.SetActive (false);
		} else {
			//		group.alpha = 0;
			//		group.interactable = false;
			//		group.blocksRaycasts = false;
			this.GetComponent<RectTransform> ().localScale = Vector3.zero;
		}
	}


}
