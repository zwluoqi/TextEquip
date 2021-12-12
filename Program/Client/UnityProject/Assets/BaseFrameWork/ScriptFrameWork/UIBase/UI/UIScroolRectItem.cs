using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  abstract class UIScroolRectItem : UIItemPool,IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerClickHandler,IPointerDownHandler {

	public bool draged = false;
	public void OnDrag(PointerEventData eventData)
	{
		ScrollRect scrollRect = this.GetComponentInParent<ScrollRect> ();;
		if (scrollRect == null) {
			return;
		}
		scrollRect.OnDrag (eventData);
		draged = true;
	}

	public void OnBeginDrag(PointerEventData eventData){
		ScrollRect scrollRect = this.GetComponentInParent<ScrollRect> ();;
		if (scrollRect == null) {
			return;
		}
		scrollRect.OnBeginDrag (eventData);
	}

	public void OnEndDrag(PointerEventData eventData){
		ScrollRect scrollRect = this.GetComponentInParent<ScrollRect> ();;
		if (scrollRect == null) {
			return;
		}
		scrollRect.OnEndDrag (eventData);
	}

	#region IPointerDownHandler implementation

	public void OnPointerDown (PointerEventData eventData)
	{
		draged = false;
	}

	#endregion

	#region IPointerClickHandler implementation
	protected  abstract void  OnClick ();

	public void OnPointerClick (PointerEventData eventData)
	{
		if (draged) {
			return;
		}
		OnClick ();
	}

	#endregion
}
