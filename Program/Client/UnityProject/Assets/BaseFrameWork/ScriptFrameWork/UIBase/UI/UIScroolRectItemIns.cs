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
// * Filename:UIScroolRectItemIns.cs
// * Created:2018/4/30
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScroolRectItemIns:UIScroolRectItem
{

	public Action<GameObject> onClick;
	#region implemented abstract members of UIScroolRectItem

	protected override void OnClick ()
	{
		if (onClick != null) {
			onClick (this.gameObject);
		}
	}

	#endregion

	public static UIScroolRectItemIns Get(GameObject go)  
	{  
		var grapic = go.GetComponent<Graphic> ();
		if (grapic != null) {
			grapic.raycastTarget = true;
		} else {
			Debug.LogError (go.name + " can not trigger ui event");
		}
		UIScroolRectItemIns listener =go.GetComponent<UIScroolRectItemIns>();  
		if(listener==null) listener=go.AddComponent<UIScroolRectItemIns>();  
		return listener;  
	}  

}
