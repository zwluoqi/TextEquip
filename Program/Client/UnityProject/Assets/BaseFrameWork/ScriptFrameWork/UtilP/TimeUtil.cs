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
// * Filename:TimeUtil.cs
// * Created:2018/2/7
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// 自动记录时间变更堆栈信息，删除任意一个记录，则取栈顶的时间缩放。如果没有信息，恢复默认1
/// </summary>
public class TimeUtil
{
	#region TimeScale
	public class RecordTimeScale
	{
		public float timeScale = 1;
	}
	private static List<RecordTimeScale> recordTimeScales = new List<RecordTimeScale> ();
	public static RecordTimeScale SetTimeScale (float newTimeScale)
	{
		RecordTimeScale rts = new RecordTimeScale ();
		rts.timeScale = newTimeScale;

		recordTimeScales.Add (rts);

		Time.timeScale = newTimeScale;

		return rts;
	}

	public static void DeleteTimeScale (RecordTimeScale rts)
	{
		recordTimeScales.Remove(rts);
		if (recordTimeScales.Count > 0) {
			Time.timeScale = recordTimeScales [recordTimeScales.Count - 1].timeScale;
		} else {
			Time.timeScale = 1;
		}
	}

	public static void ResetTimeScale(){
		recordTimeScales.Clear ();
		Time.timeScale = 1;
	}

	public static string GetTimeReccords ()
	{
		StringBuilder sb = new StringBuilder();
		foreach (var r in recordTimeScales) {
			sb.Append (r.timeScale + ",");
		}
		return sb.ToString ();
	}
	#endregion
}

