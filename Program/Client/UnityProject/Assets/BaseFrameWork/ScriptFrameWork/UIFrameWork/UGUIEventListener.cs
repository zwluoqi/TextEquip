using System;
using UnityEngine;  
using UnityEngine.UI;  
using System.Collections;  
using UnityEngine.EventSystems;
using System.Collections.Generic;
//using LuaInterface;  


namespace XZXD.UI
{
	public class UGUIEventListener:EventTrigger  
	{  
		public delegate void VoidDelegate(GameObject go);  

		public VoidDelegate onClick ;

		public Void_GO_Bool onPress ;

		public VoidDelegate onLongClick;

		public bool pressing = false;
		public float checkLongClickSpace = 0.3f;
		public float startPressTimer;
		public bool hasCallLongClickOnce;

		public override void OnPointerClick(PointerEventData eventData)  
		{  
			XZXDDebug.Log ("OnPointerClick " + this.gameObject);
			// SoundManager.Instance.PlayClip ("audio/ui/ui_button");

			base.OnPointerClick (eventData);
			if (onClick != null)  
				onClick(gameObject);  
		} 
		#region IPointerDownHandler implementation


		public override void OnPointerDown (PointerEventData eventData)
		{
			startPressTimer = Time.time;
			pressing = true;
			hasCallLongClickOnce = false;
			if (this.onPress != null) {
				this.onPress (gameObject, true);
			}
		}


		#endregion

		#region IPointerUpHandler implementation

		public override void OnPointerUp (PointerEventData eventData)
		{
			pressing = false;
			hasCallLongClickOnce = true;
			if (this.onPress != null) {
				this.onPress (gameObject, false);
			}
		}

		#endregion


		private void Update()
		{
			if (pressing && !hasCallLongClickOnce)
			{
				if (Time.time - startPressTimer > checkLongClickSpace)
				{
					hasCallLongClickOnce = true;
					if (onLongClick != null)
					{
						onLongClick(gameObject);
					}
				}
			}
		}


		public static UGUIEventListener Get(GameObject go)  
		{  
			var grapic = go.GetComponent<Graphic> ();
			if (grapic != null) {
				grapic.raycastTarget = true;
			} else {
				Debug.LogError (go.name + " can not trigger ui event");
			}
			UGUIEventListener listener =go.GetComponent<UGUIEventListener>();  
			if(listener==null) listener=go.AddComponent<UGUIEventListener>();  
			return listener;  
		}  
	}  

}