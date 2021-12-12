using System;
using UnityEngine;
using System.Collections.Generic;

public class UnityTools
{
	public static void SetParent(Transform trans,Transform parent){
		trans.SetParent (parent);
		trans.localPosition = Vector3.zero;
		trans.localRotation = Quaternion.identity;
		trans.localScale = Vector3.one;
	}

	public static void SetFullScreenParent (Transform trans, Transform parent)
	{
		SetParent (trans, parent);

		RectTransform rtTr = trans.gameObject.GetComponent<UnityEngine.RectTransform> ();

		if (rtTr == null) {
			rtTr = trans.gameObject.AddComponent<UnityEngine.RectTransform> ();
		}
		rtTr.anchoredPosition = Vector2.zero;
		rtTr.sizeDelta = Vector2.zero;

		rtTr.anchorMax = Vector2.one;
		rtTr.anchorMin = Vector2.zero;
	}


	public static void SetCenterParent (Transform trans, Transform parent)
	{
		SetParent (trans, parent);

		RectTransform rtTr = trans.gameObject.GetComponent<UnityEngine.RectTransform> ();

		if (rtTr == null) {
			rtTr = trans.gameObject.AddComponent<UnityEngine.RectTransform> ();
		}


		rtTr.anchorMax = new Vector2 (0.5f, 0.5f);
		rtTr.anchorMin = new Vector2 (0.5f, 0.5f);
		rtTr.pivot = new Vector2 (0.5f, 0.5f);
		rtTr.anchoredPosition = Vector2.zero;
		rtTr.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal,Mathf.Abs( rtTr.sizeDelta.x));
		rtTr.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,Mathf.Abs( rtTr.sizeDelta.y));
	}


	/// <summary>
	/// 相机3D坐标转换2D
	/// </summary>
	public static Vector3 WorldToScreenPosition(Vector3 position,Camera threeCamera,Camera uiCamera)
	{
		Vector3 targetVet = uiCamera.ScreenToWorldPoint(threeCamera.WorldToScreenPoint(position));
		targetVet.z = 0;
		return targetVet;
	}

	public static void SetLayer(int layer,Transform trans){
		trans.gameObject.layer = layer;

		foreach (Transform tran in trans) {
			SetLayer (layer, tran);
		}

	}

	public static string GetTransformPath (Transform transform)
	{
		List<string> nodes = new List<string> ();
		GetTransformPath (nodes, transform);
		string path = "";
		for(int i=1;i<nodes.Count;i++) {
			path += nodes[i] + "/";
		}
		path = path.Remove (path.Length - 1);
		return path;
	}

	static void GetTransformPath(List<string> nodes,Transform transform){
		if (transform != null) {
			nodes.Insert (0, transform.name);
			GetTransformPath (nodes, transform.parent);
		} else {
			return;
		}
	}
}

