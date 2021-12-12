using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using XZXD.UI;

public class PopMessage : UIPart {

	public Text label;

	void Start(){

		StartCoroutine (_DelayMove(1f));
	}

	IEnumerator _DelayMove(float delay){
		

		yield return new WaitForSeconds (delay);
		GameObject.Destroy (this.gameObject, 1);
		var dl = gameObject.GetComponent<RectTransform> ().DOLocalMoveY (668, 1);
		dl.SetEase (Ease.Linear);
	}
}
