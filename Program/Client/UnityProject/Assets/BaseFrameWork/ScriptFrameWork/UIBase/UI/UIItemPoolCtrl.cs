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
// * Filename:UIAlphaCtrlPool.cs
// * Created:2018/6/6
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPoolCtrl<S> where S : UIItemPool
{

	public int index = 0;
	public bool hideMode = false;
	public List<S> items = new List<S>();

	public S GetTaskItem(GridLayoutGroup grid,S itemPrefab)
	{
		return GetTaskItem(grid.transform,itemPrefab);
	}

	public S GetTaskItem(Transform grid,S itemPrefab)
	{
		S teskItem = null;
		if (index >= items.Count) {
			var go = GameObject.Instantiate (itemPrefab);
			UnityTools.SetCenterParent (go.transform, grid.transform);
			go.gameObject.SetActive (true);
			go.hideMode = hideMode;
			items.Add (go);
		}
		teskItem = items [index];
		teskItem.Show ();
		index++;
		return teskItem;
	}

	public void Hide(){
		foreach (var val in items) {
			val.Hide();
		}
		index = 0;
	}
}

