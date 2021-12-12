using UnityEngine;
using System.Collections;

public class Notification
{
	public int id;
	public object info;

	private Notification (int notificationId, object notificationInfo)
	{
		this.id = notificationId;
		this.info = notificationInfo;
	}

	public static Notification GetNotification (int notificationId)
	{
		return new Notification (notificationId, null);
	}

	public static Notification GetNotification (int notificationId, object notificationInfo)
	{
		return new Notification (notificationId, notificationInfo);
	}
}