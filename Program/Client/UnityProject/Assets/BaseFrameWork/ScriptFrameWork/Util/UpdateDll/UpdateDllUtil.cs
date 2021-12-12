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
// * Filename:UpdateDllUtil.cs
// * Created:2019/4/25
// * Author:  zhouwei
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
// using XZXD.UI;
using System.IO;
using UnityEngine;
using AssetPlugin;

public class UpdateDllUtil
{
	public static void SaveDllConfigInfo()
	{
		///有远程文件就更新，没有就删除已有的
		if (AssetFileToolUtilManager.Instance.dlls.recordNeedUpdate) {
			string dllpaht = GetDllPath () + "/csdlls/fuck.bin";
			AssetFileToolUtilManager.Instance.dlls.remoteSettingTxtAssetFileTool.SaveAssetHashCodeToFile (dllpaht);
			if (AssetFileToolUtilManager.Instance.dlls.recordNeedUpdate) {
				// UIBoxManager.Instance.CreatOneButtonBox ("确定", 
				// 	"脚本已更新，请重新启动\r\n" +
				// 	"1.修复重生之后部分属性显示异常bug\r\n" +
				// 	"2.修复一键碎炼的bug\r\n" +
				// 	"3.修复门派转职打不开的bug\r\n" +
				// 	"4.修复天机胜利后面板显示过长的bug\r\n" , OnRestart);
			}
		}
	}

	static void OnRestart (bool restart)
	{
		//XZXD.NativeCaller.restartApp ();
	}

//	public static void RemoveDllConfigInfo(){
//		string dllpaht = GetDllPath () + "/csdlls/fuck.bin";
//		if (File.Exists(dllpaht))
//		{
//			File.Delete(dllpaht);
//		}
//	}

	public static string GetDllPath(){
		string path = "";
		if (Application.platform == RuntimePlatform.Android)
		{
			string datapath = Application.dataPath;
			int start = datapath.IndexOf("com.");
			int end = datapath.IndexOf("-");
			string packagename = datapath.Substring(start, end - start);
			path = "/data/data/" + packagename + "/files/";
		}
		else
		{
			path = PathTool.DllsSavePath;
		}
		Debug.LogError("dll path:" + path);
		return path;
	}

	public static string GetLibPath(){
		string datapath = Application.dataPath;
		int start = datapath.IndexOf("com.");
		int end = datapath.IndexOf("-");
		string packagename = datapath.Substring(start, end - start);
		var path = "/data/data/" + packagename + "/lib/";
		return path;
	}


	public static string GetLibMonoMd5(){
		return "";
	}
	
}

