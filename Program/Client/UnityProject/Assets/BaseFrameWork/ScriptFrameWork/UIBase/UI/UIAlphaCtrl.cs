using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XZXD.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIAlphaCtrl : MonoBehaviour {

	private CanvasGroup _group;
	private bool defaultinteractable = false;
	private bool defaultblocksRaycasts = false;
	public CanvasGroup group{
		get{
			if (_group == null) {
				_group = this.GetComponent<CanvasGroup> ();
				if (_group == null) {
					_group = this.gameObject.AddComponent<CanvasGroup> ();
				}
				defaultinteractable = _group.interactable;
				defaultblocksRaycasts = _group.blocksRaycasts;
			}
			return _group;
		}
	}


	public void Show(){
//		group.alpha = 1;
//		group.interactable = defaultinteractable;
//		group.blocksRaycasts = defaultblocksRaycasts;
//		this.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		this.GetComponent<RectTransform> ().localScale = Vector3.one;
	}

	public void Hide(){
//		group.alpha = 0;
//		group.interactable = false;
//		group.blocksRaycasts = false;
		this.GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

	public bool IsShowed ()
	{
		return group.alpha == 1;
	}
}
