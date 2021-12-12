using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Selector (Notification notification);
	
/// <summary>
/// An NotificationCenter object (or simply, notification center) provides a mechanism 
/// for broadcasting information within a program. An NotificationCenter object is essentially 
/// a notification dispatch table.
/// </summary>
public class NotificationCenter
{
	#region Static Fields

	private static NotificationCenter _ins = null;

	#endregion

	#region Static Properties

	public static NotificationCenter Default {
		get {
			if (_ins == null) {
				_ins = GetNew ();
			}
			return _ins;
		}
	}

	public static void Release(){
		_ins = null;
	}

	#endregion

	#region Static Methods

	public static NotificationCenter GetNew ()
	{
		return new NotificationCenter ();
	}

	#endregion

	#region Fields

	List<BaseItem> baseDataList;
	Dictionary<object, List<BaseItem>> observerListDic;
	Dictionary<int, List<BaseItem>> keyListDic;
	bool dirty;

	#endregion

	#region Constructors

	private NotificationCenter ()
	{
		baseDataList = new List<BaseItem> ();
		observerListDic = new Dictionary<object, List<BaseItem>> ();
		keyListDic = new Dictionary<int, List<BaseItem>> ();
		dirty = false;
	}

	#endregion

	#region Methods

	/// <summary>
	/// Adds an entry to the receiver’s dispatch table with an observer, a notification selector and optional criteria: notification name and sender.
	/// </summary>
	/// <param name="observer">Object registering as an observer. This value must not be null.</param>
	/// <param name="selector">Selector that specifies the message the receiver sends observer to notify it of the notification posting. 
	/// The method specified by selector must have one and only one argument (an instance of Notification).</param>
	/// <param name="notificationId">The id of the notification for which to register the observer; that is, only notifications with this name are delivered to the observer.
	/// If you pass null, the notification center doesn’t use a notification’s name to decide whether to deliver it to the observer.</param>
	/// <param name="poster">The object whose notifications the observer wants to receive; that is, only notifications sent by this sender are delivered to the observer.
	/// If you pass null, the notification center doesn’t use a notification’s sender to decide whether to deliver it to the observer.</param>
	public void AddObserver (object observer, Selector selector, int notificationId)
	{
		if (observer == null) {
			throw new System.Exception ("The observer must not be null");
		}
		for (int i = 0, imax = baseDataList.Count; i < imax; i++) {
			if (baseDataList [i].observer == observer && baseDataList [i].key == notificationId) {
				baseDataList [i].valid = false;
				baseDataList [i].selector = selector;
				XZXDDebug.LogWarning ("An observer can only observe a similar message");
				Debug.Break ();
			}
		}
		baseDataList.Add (new BaseItem (observer, notificationId, selector));
		dirty = true;
	}

	/// <summary>
	/// Posts a given notification to the receiver.
	/// </summary>
	/// <param name="notification">The notification to post. This value must not be null.</param>
	public void PostNotification (Notification notification)
	{
		if (notification == null) {
			throw new System.Exception ("The notification must not be null");
		}
		if (dirty) {
			ExecuteDirty ();
			dirty = false;
		}
		if (keyListDic.ContainsKey (notification.id) && keyListDic [notification.id] != null) {
			List<BaseItem> res = new List<BaseItem> (keyListDic [notification.id]);
			foreach (var item in res) {
				if (item.valid) {
					item.selector (notification);
				}
			}
		}
	}

	private void ExecuteDirty ()
	{
		observerListDic.Clear ();
		keyListDic.Clear ();
		for (int i = baseDataList.Count - 1; i >= 0; i--) {
			if (!baseDataList [i].valid) {
				baseDataList.RemoveAt (i);
			} else {
				BaseItem data = baseDataList [i];
				if (!observerListDic.ContainsKey (data.observer)) {
					observerListDic [data.observer] = new List<BaseItem> ();
				}
				observerListDic [data.observer].Add (data);
				if (!keyListDic.ContainsKey (data.key)) {
					keyListDic [data.key] = new List<BaseItem> ();
				}
				keyListDic [data.key].Add (data);
			}
		}
	}

	/// <summary>
	/// Creates a notification with a given name and sender and posts it to the receiver.
	/// </summary>
	/// <param name="notificationName">The name of the notification.</param>
	/// <param name="poster">The object posting the notification.</param>
	public void PostNotification (int notificationId)
	{
		PostNotification (notificationId, null);
	}

	/// <summary>
	/// Creates a notification with a given name, sender, and information and posts it to the receiver.
	/// </summary>
	/// <param name="notificationName">The name of the notification.</param>
	/// <param name="poster">The object posting the notification.</param>
	/// <param name="userInfo">Information about the the notification. May be null.</param>
	public void PostNotification (int notificationId, object notificationInfo)
	{
		Notification notification = Notification.GetNotification (notificationId, notificationInfo);
		PostNotification (notification);
	}

	/// <summary>
	/// Removes all the entries specifying a given observer from the receiver’s dispatch table.
	/// </summary>
	/// <param name="observer">The observer to remove. Must not be null.</param>
	public void RemoveObserver (object observer)
	{
		if (observer == null) {
			throw new System.Exception ("The observer must not be null");
		}
		for (int i = 0, imax = baseDataList.Count; i < imax; i++) {
			if (baseDataList [i].observer == observer) {
				baseDataList [i].valid = false;
			}
		}
		dirty = true;
	}

	/// <summary>
	/// Removes matching entries from the receiver’s dispatch table.
	/// </summary>
	/// <param name="observer">Observer to remove from the dispatch table. Specify an observer to remove only entries for this observer. 
	/// Must not be null, or message will have no effect.</param>
	/// <param name="notificationId">Name of the notification to remove from dispatch table. Specify a notification name to remove only 
	/// entries that specify this notification name. When null or empty, the receiver does not use notification names as criteria for removal.</param>
	/// <param name="poster">Sender to remove from the dispatch table. Specify a notification sender to remove only entries that specify this sender. 
	/// When null, the receiver does not use notification senders as criteria for removal.</param>
	public void RemoveObserver (object observer, int notificationId)
	{
		if (observer == null) {
			throw new System.Exception ("The observer must not be null");
		}
		bool idInvalid = notificationId < 0;
		if (idInvalid) {
			RemoveObserver (observer);
		} else {
			for (int i = 0, imax = baseDataList.Count; i < imax; i++) {
				if (baseDataList [i].observer == observer && baseDataList [i].key == notificationId) {
					baseDataList [i].valid = false;
				}
			}
			dirty = true;
		}
	}
	#if UNITY_EDITOR
	public Dictionary<object, List<int>> GetObserverDict ()
	{
		Dictionary<object, List<int>> res = new Dictionary<object, List<int>> ();
		foreach (var item in observerListDic) {
			res [item.Key] = new List<int> ();
			foreach (var da in item.Value) {
				res [item.Key].Add (da.key);
			}
		}
		return res;
	}
	#endif

	#endregion

	#region Sub Class

	private class BaseItem
	{
		public object observer;
		public int key;
		public Selector selector;
		public bool valid;

		public BaseItem (object observer, int key, Selector selector)
		{
			this.observer = observer;
			this.key = key;
			this.selector = selector;
			this.valid = true;
		}
	}

	#endregion
}