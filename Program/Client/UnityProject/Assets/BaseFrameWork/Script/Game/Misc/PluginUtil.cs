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
// * Filename:PluginUtil.cs
// * Created:2020/4/13
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

namespace XZXD
{
	public class PluginUtil
	{
		public static bool CheckModifuApp ()
		{
			// string info = NativeCaller.getRunningProcess ();
			// if (string.IsNullOrEmpty (info)) {
			// 	return false;
			// }
			// try {
			// 	var apps = Newtonsoft.Json.JsonConvert.DeserializeObject<AppInfo[]> (info);
			// 	foreach (var app in  apps) {
			// 		if (app == null) {
			// 			continue;
			// 		}
			// 		if (string.IsNullOrEmpty (app.pkgName)) {
			// 			continue;
			// 		}
			//
			// 		foreach (var dictapp in DictDataManager.Instance.dictSystemCheatApp.getList()) {
			// 			//				DictCheatApp config = allConfigs[j];
			// 			if (string.IsNullOrEmpty (dictapp.pkgname)) {
			// 				continue;
			// 			}
			// 			if (app.pkgName == dictapp.pkgname) {
			// 				return true;
			// 			}
			// 		}
			// 	}
			// } catch (Exception e) {
			//
			// }
			return false;
		}


		public class AppInfo
		{
			public string pkgName;
			public string pid;
			public string processName;
			public string uid;
		}
	}
}

