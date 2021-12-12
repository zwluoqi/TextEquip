using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class XZXDDebug
{
	public static bool m_showLog = false;
	public static bool m_debugNormalEnable = false;
	public static bool m_debugWarning = false;



//	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void Log (object message)
	{
		#if UNITY_EDITOR
		// if (!m_showLog)
		// 	return;
		message = System.DateTime.Now.ToString ("yyyy-MM-dd HH-mm-ss-ffff") + "---" + "Normal-------" + " " + message;


		// if (m_debugNormalEnable == false) {
		// 	return;
		// }
		Log0 (message);
		#endif
	}

	private static void Log0 (object message)
	{
		UnityEngine.Debug.Log("<color=cyan>" + message + "</color>");

	}

//	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogWarning (object message)
	{
		#if UNITY_EDITOR
		// if (!m_showLog)
		// 	return;
		message = System.DateTime.Now.ToString ("yyyy-MM-dd HH-mm-ss-ffff") + "---" + "Warning------" + " " + message;


		// if (m_debugWarning == false) {
		// 	return;
		// }
		LogWarning0 (message);
		#endif
	}

	private static void LogWarning0 (object message)
	{
		UnityEngine.Debug.LogWarning("<color=orange>" + message + "</color>");

	}

//	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogGuideExe (string desc)
	{
		#if UNITY_EDITOR
		LogWarning (desc);
		#endif
	}

//	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public static void LogUnitExe (string desc)
	{
		LogWarning (desc);
	}

	public static void Error (object message)
	{
		message = System.DateTime.Now.ToString ("yyyy-MM-dd HH-mm-ss-ffff") + "---" + "Error--------" + " " + message;
		UnityEngine.Debug.LogError (message);
	}
}
