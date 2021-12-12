using UnityEngine;
using System.Collections;
using XZXD.UI;
using XZXD;
using System;

public class NetErrorHandle
{
	public static void ErrorMessage (string sErrorCode,  string sErrorText)
	{
		if (sErrorCode.Equals ("update_new_dict_version")) {
			BoxManager.CreatOneButtonBox (sErrorText, SessionOut);
		} 
		else if (sErrorCode.Equals("Info_StopServer")) {
			BoxManager.CreatOneButtonBox(sErrorText, SessionOut);
		}
        else if (sErrorCode.Equals("jiance_zuobi_exit"))
        {
//			ADTouTiao.Instance.RequestRewardVideo ();
	        BoxManager.CreatOneButtonBox( sErrorText, OnExitApp);
        }
		else if (string.IsNullOrEmpty (sErrorCode)) {
			BoxManager.CreatOneButtonBox ( sErrorText, SessionOut);
		}
		else {
			BoxManager.CreatOneButtonBox ( sErrorText, null);
		}
	}

    private static void OnExitApp(bool bo)
    {
		Application.OpenURL(VersionTool.zuobiExitUrl);
		Application.Quit ();
    }

    static void SessionOut (bool ok)
	{
		// StageMgr.Inst().ReturnLogin ();
	}
}
