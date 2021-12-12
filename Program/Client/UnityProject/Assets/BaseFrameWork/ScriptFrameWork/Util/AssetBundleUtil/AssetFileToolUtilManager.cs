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
// * Filename:AssetFileToolUtilManager.cs
// * Created:2018/4/4
// * Author:  lucy.yijian
// * Purpose:
// * ==============================================================================
// */
//

using System;
using AssetPlugin;

public class AssetFileToolUtilManager
{
	public static AssetFileToolUtilManager _instance;
	public static AssetFileToolUtilManager Instance{
		get{
			if (_instance == null) {
				_instance = new AssetFileToolUtilManager ();
			}
			return _instance;
		}
	}


	public AssetFileToolUtil txt = new AssetFileToolUtil();
	public AssetFileToolUtil texture = new AssetFileToolUtil();
	public AssetFileToolUtil atlas = new AssetFileToolUtil();
    public AssetFileToolUtil dlls = new AssetFileToolUtil();

}