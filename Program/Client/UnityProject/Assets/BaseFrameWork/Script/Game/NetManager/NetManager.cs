using UnityEngine;
using System.Collections;
using NetWork.Layer;
using Newtonsoft.Json.Linq;
using XZXD.UI;

public class NetManager
{
	private static NetManager instance;

	public static NetManager Instance {
		get {
			if (instance == null) {
				instance = new NetManager ();

			}
			return instance;
		}
	}

	public HTTPManager httpManager;

	public static void Release ()
	{
		instance = null;		
	}

	private bool m_bLastLockScreen;

	public NetManager ()
	{
		httpManager = new HTTPManager (false);
		httpManager.netErrorHandle = OnNetErrorHandle;
	}

	void OnNetErrorHandle (string code,string arg1)
	{
		NetErrorHandle.ErrorMessage (code,arg1);
	}

	public void InitHttp (string sPath, string relativePath)
	{
		httpManager.SetServerUrl (sPath, relativePath);
	}



	public void NetTick ()
	{
		HttpTick ();
	}


	private void HttpTick ()
	{
		if (httpManager != null) {
			httpManager.Tick ();
		}
	}



	public bool SendHttp (string nOpcode, string kMsg, HttpHandler dShow, bool bLockScreen = true)
	{
		bool success = httpManager.Send (nOpcode, kMsg, !bLockScreen, dShow);
		if (success) {
			if (bLockScreen) {
				m_bLastLockScreen = bLockScreen;
				XZXDDebug.LogWarning ("BoxManager.CreateNetMask()");
				BoxManager.CreateNetMask ();
			}
		}
		return success;
	}

  
	public void CheckErrorPacket (Packet kErrorMsg)
	{
		XZXDDebug.Error ("Error opcode: " + kErrorMsg.nOpCode);
		//异常处理
	}

	public static void ResetZoneUrl(string url){
		VersionTool.zoneUrl = url;
		string serverURL = VersionTool.zoneUrl+ "text_equip/";
		NetManager.Instance.InitHttp (serverURL,"");
		// SDKOrderTick.SetHttp(serverURL,"");
		// ChatOrderTick.SetHttp (serverURL, "");
	}

}
