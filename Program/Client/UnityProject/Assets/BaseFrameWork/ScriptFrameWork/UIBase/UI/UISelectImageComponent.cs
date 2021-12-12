using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISelectImageComponent : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler {


	public Image target;

	public Sprite normal;
	public Sprite pressed;

	void Start(){
		target.sprite = normal;
	}


	public void OnPointerDown (PointerEventData eventData){
		target.sprite = pressed;

	}

	public void OnPointerUp (PointerEventData eventData){
		target.sprite = normal;

	}

	public void OnPointerClick (PointerEventData eventData){

		target.sprite = normal;
	}
}
