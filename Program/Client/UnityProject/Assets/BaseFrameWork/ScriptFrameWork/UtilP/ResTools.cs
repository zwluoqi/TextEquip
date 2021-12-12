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
// * Filename:ResTools.cs
// * Created:2018/4/4
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;

public class ResTools
{

	public static byte[] ReadBytes (string filePath)
	{
		if (filePath.LastIndexOf ('.') != -1)
			filePath = filePath.Remove (filePath.LastIndexOf ('.'));
		var textAsset = Resources.Load (filePath) as TextAsset;
		if (null == textAsset) {
			Debug.LogError ("error! Resources.Load(" + filePath + ") failed!");
			return null;
		}
		byte[] bytes = textAsset.bytes;
		Resources.UnloadAsset (textAsset);
		return bytes;
	}

}
