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
// * Filename:DynamicDllDownload.cs
// * Created:2019/6/10
// * Author:  lucy.yijian
// * Alert:
// * 代码千万行
// * 注释第一行
// * 命名不规范
// * 同事两行泪
// * Purpose:
// * ==============================================================================
// */
//
using System;
using UnityEngine;

public class DynamicDllDownload:DynamicResDownload
{
	int localDllVersion = 0;
	protected override void DoInit ()
	{
		var versionFile = Resources.Load<TextAsset> ("dlls/version");
		if (versionFile != null) {
			int.TryParse (versionFile.text,out localDllVersion);
		}
	}

	protected override bool CheckNeedUpdateAsset ()
	{
		if(remoteAssetFileTool == null){
			Debug.LogError("remote txt config null");
			return false;
		}
		if (remoteAssetFileTool.version <= localDllVersion) {
			return false;
		}
		return base.CheckNeedUpdateAsset ();
	}
}