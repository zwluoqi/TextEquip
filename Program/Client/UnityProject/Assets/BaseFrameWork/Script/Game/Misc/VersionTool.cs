using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XZXD;

public sealed class VersionTool {

	public static bool useTest = false;
	public static string url = "";
	public static string zoneUrl = "";
	public static string cdnDownloadUrl {
		get{
			return DictConfigUtil.GetString ("cdn_download_url","http://114.67.88.112/cdn");
		}
	}

	public static int appVersion = 38;
	public static int funVersion = 8;
    internal static string qqUrl{
        get
        {
	        return  DictConfigUtil.GetString ("qq_url","https://jq.qq.com/?_wv=1027&k=5p7uL1A");
        }
    }

	internal static string zuobiExitUrl{
		get
		{
			return  DictConfigUtil.GetString ("exit_url","https://jq.qq.com/?_wv=1027&k=5p7uL1A");
		}
	}
	
	public static string beginGameCode ="xxpcl_i";
}
