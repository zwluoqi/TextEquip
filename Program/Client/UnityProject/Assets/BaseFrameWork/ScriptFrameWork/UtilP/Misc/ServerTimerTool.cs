using UnityEngine;
using System.Collections;
using System;



public class ServerTimerTool
{
	private static long distance = 0;

	public const long begin = 621355968000000000;
	//1970,1,1到公元元年的时间差




	// 机器的UTC时间(毫秒)
	public static long UtcNow {
		get {
			var cur = CurrentTime;
			return (DateTime.UtcNow.Ticks - begin) / 10000;
		}
	}

	// 服务器的UTC时间(毫秒)
	public static long ServerUtcNowMillSecond {
		get {
			var cur = CurrentTime;
			return UtcNow + distance / 10000;
		}
	}


	// 服务器的UTC时间(秒)
	public static long ServerUtcNowSecond {
		get {
			var cur = CurrentTime;
			return ServerUtcNowMillSecond / 1000;
		}
	}
		
	/// <summary>
	/// 校正时刻毫秒
	/// </summary>
	private static long correctServerTime=correctServerDate.Ticks/10000;
	/// <summary>
	/// 校正时刻时间
	/// </summary>
	private static DateTime correctServerDate = DateTime.Now;

	public static void CorrectTime (long _serverTime)
	{
		correctServerTime = _serverTime;
		distance = correctServerTime * 10000 - (DateTime.UtcNow.Ticks - begin);
		correctServerDate = CurrentTime;
	}

	// 本地时间(与服务器校正完毕后的本地时间)
	public static DateTime CurrentTime {
		get {
			var cur = new DateTime (DateTime.Now.Ticks + distance);
			var span = (cur - correctServerDate);
			return cur;
		}
	}
		
	/// <summary>
	/// JAVA毫秒转C#时间
	/// </summary>
	/// <returns>The CS time.</returns>
	/// <param name="time">Time.</param>
	public static DateTime Java2CSTime (long time)
	{
		var cur = CurrentTime;
		return new DateTime (time * 10000 + begin - DateTime.UtcNow.Ticks + DateTime.Now.Ticks);
	}

	/// <summary>
	/// C#转JAVA时间
	/// </summary>
	/// <returns>The java time.</returns>
	/// <param name="time">Time.</param>
	public static long Cs2JavaTime (DateTime time)
	{
		var cur = CurrentTime;
		return (time.Ticks - DateTime.Now.Ticks + DateTime.UtcNow.Ticks - begin) / 10000;
	}
}

