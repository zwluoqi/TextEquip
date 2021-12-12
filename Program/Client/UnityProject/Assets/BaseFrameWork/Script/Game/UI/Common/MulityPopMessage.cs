using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;
using TMPro;
using XZXD.UI;

public class MulityPopMessage : UIPart {

	public Transform grid;
	public GameObject prefab;
	public GameObject tip;

	void Awake(){
		tip.SetActive (false);
		prefab.SetActive (false);
		GameObjectPoolManager.Instance.Regist ("MulityPopMessageItem", 10, prefab);

	}


	public void ShowTip(string info){
		var text = tip.GetComponentInChildren<TMP_Text> ();
		text.text = info;
		text.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
		float duration = text.preferredWidth / 600f * 5 + 10 ;
		tip.SetActive (true);
		var targetPos = Mathf.Min (-text.preferredWidth - 80, -680);
		text.GetComponent<RectTransform> ().DOLocalMoveX (targetPos, duration);
		RunCoroutine.Run(_EnumHideTip(duration));
	}

	private IEnumerator _EnumHideTip(float duration)
	{
		yield return new WaitForSeconds(duration);
		_HideTip();
	}

	void _HideTip ()
	{
		tip.SetActive (false);
	}

	public bool IsShowTip ()
	{
		return tip.activeSelf;
	}

	public void ShowMessage(string str){
		InnerShowMessage (str);
	}

	public void ShowMessage(StringBuilder sb){
		
		string[] newInfos = sb.ToString ().Split (new char[]{ '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

		foreach (var info in newInfos) {
			InnerShowMessage (info);
		}
	}

	public void ShowMessage(string[] newInfos){

		foreach (var info in newInfos) {
			InnerShowMessage (info);
		}
	}

	void InnerShowMessage(string info){


		var item = GameObjectPoolManager.Instance.Spawn ("MulityPopMessageItem");
		UnityTools.SetCenterParent (item.transform, grid.transform);
		var text = item.GetComponentInChildren<TMP_Text> ();

		text.text = info;

		item.SetActive (true);
		text.GetComponent<RectTransform>().DOLocalMoveY(600, 3f);
		GameObjectPoolManager.Instance.Unspawn (item, 4);
	}

}
