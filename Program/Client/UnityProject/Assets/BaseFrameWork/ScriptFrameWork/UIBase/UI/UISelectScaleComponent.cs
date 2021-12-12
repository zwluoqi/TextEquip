using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectScaleComponent  : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler {

	public Transform target;
	public Vector3 normal = Vector3.one;
	public Vector3 press = Vector3.one*0.9f;

	void Awake(){
		if (target == null) {
			target = this.transform;
		}
	}

	public void OnPointerDown (PointerEventData eventData){
		target.localScale = press;

	}

	public void OnPointerUp (PointerEventData eventData){
		target.localScale = normal;

	}

	public void OnPointerClick (PointerEventData eventData){

		target.localScale = normal;
	}
}
