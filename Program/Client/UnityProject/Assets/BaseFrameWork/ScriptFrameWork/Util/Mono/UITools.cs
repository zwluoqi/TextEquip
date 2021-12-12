using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using XZXD;

public static class UITools
{
	public static GameObject LoadUIObject (string path, Transform parent)
	{
		UnityEngine.Object uiPrefab = AssetLoaderManager.Instance.LoadResourceBlock (path);
		GameObject uiLayer = GameObject.Instantiate (uiPrefab) as GameObject;
		uiLayer.name = uiPrefab.name;
		UnityTools.SetLayer (LayerMask.NameToLayer ("UI"), uiLayer.transform);

		UnityTools.SetCenterParent (uiLayer.transform, parent);
		uiLayer.SetActive (true);
		return uiLayer;
		
	}

//	public static void SetParent (Transform transform, Transform grid)
//	{
//		throw new NotImplementedException ();
//	}

	//获得孩子-gameobject版本
	public static GameObject GetChild (GameObject go, string subnode)
	{
		if (go == null)
			return null;
		Transform trans = GetChild (go.transform, subnode);

		return trans == null ? null : trans.gameObject;
	}
	//获得孩子-transform版本
	public static Transform GetChild (Transform trans, string subnode)
	{
		if (trans == null)
			return null;
		return trans.Find (subnode);
	}

	public static T AddMissingComponent<T> (this GameObject obj) where T : Component
	{
		T res = obj.GetComponent<T> ();
		if (res == null) {
			res = obj.AddComponent<T> ();
		}
		return res;
	}


	public static T GetComponentOrInParent<T>(this Component obj) where T:Component{
		var uiBase = obj.GetComponent<T> ();
		if (uiBase == null && obj.transform.parent != null) {
			uiBase = obj.transform.parent.GetComponentOrInParent<T> ();
		}
		return uiBase;
	}


	public static Texture2D CreateTexture(int width_val,int height_val,byte[] bytes)
	{
		var formatlevel = 0;// CacheData.GetTextureLevel ();
		TextureFormat tf = TextureFormat.RGBA32;
//		//TODO
//		#if UNITY_IOS
		if(formatlevel ==0){
			tf = TextureFormat.RGBA32;
		}else if(formatlevel == 1){
			tf = TextureFormat.RGBAHalf;
		}else {
			tf = TextureFormat.RGBA4444;
		}
//
//		Texture2D texture = new Texture2D(width_val, height_val,tf,false);
//		#else
//		if(formatlevel ==0){
//		tf = TextureFormat.RGBA32;
//		}else if(formatlevel == 1){
//		tf = TextureFormat.RGBAHalf;
//		}else {
//		tf = TextureFormat.ETC2_RGBA8;
//		}
//		Texture2D texture = new Texture2D(width_val, height_val,tf,false);
//		#endif
		Texture2D texture = new Texture2D(width_val, height_val,tf,false);
		#if UNITY_ANDROID
//		texture.alphaIsTransparency = true;
		#else

		#endif
		texture.LoadImage (bytes);
		return texture;
	}

	public static void SetQulityLev (int arg0)
	{
		string[] names = QualitySettings.names;
		int level = 0;
		if(arg0==0){
			level = QualitySettings.names.Length-1;
		}else if(arg0 == 1){
			level = QualitySettings.names.Length/2;
		}else{
			level = 0;
		}
		QualitySettings.SetQualityLevel(level,true);
	}


	public static void SetSpriteName(this Image image, string atlas, string spriteName)
	{
		image.sprite = SpritePackerManager.Instance.GetSprite(atlas, spriteName);
	}

	public static GameObject AddSimpleButton(Transform transform,UnityEngine.TextAnchor anchor,string btn){
		var go = UITools.AddButton (transform, anchor);
		var image = UITools.AddImage (go.transform, TextAnchor.MiddleCenter);
		image.SetSpriteName ("CommonUI", "common_btn");
		var text = UITools.AddText (go.transform, TextAnchor.MiddleCenter);
		text.text = btn;
		return go;
	}

	public static GameObject AddButton (Transform transform,UnityEngine.TextAnchor anchor)
	{
		var go = new GameObject ();
		UnityTools.SetParent(go.transform,transform);
		RectTransform rtTr = go.AddMissingComponent<UnityEngine.RectTransform> ();
		UpdateAnchor (rtTr, anchor);
		rtTr.sizeDelta = new Vector2 (200, 65);
		rtTr.anchoredPosition = Vector2.zero;
		go.AddComponent<UIBoxRayCast> ();
		return go;
	}

	public static UnityEngine.UI.Image AddImage (Transform transform,UnityEngine.TextAnchor anchor)
	{
		var go = new GameObject ();
		UnityTools.SetParent(go.transform,transform);
		RectTransform rtTr = go.AddMissingComponent<UnityEngine.RectTransform> ();
		UpdateAnchor (rtTr, anchor);
		rtTr.sizeDelta = new Vector2 (200, 65);
		rtTr.anchoredPosition = Vector2.zero;
		var image = go.AddComponent<UnityEngine.UI.Image> ();

		return image;
	}

	public static UnityEngine.UI.Text AddText (Transform transform,UnityEngine.TextAnchor anchor)
	{
		var go = new GameObject ();
		UnityTools.SetParent(go.transform,transform);
		RectTransform rtTr = go.AddMissingComponent<UnityEngine.RectTransform> ();
		UpdateAnchor (rtTr, anchor);
		rtTr.sizeDelta = new Vector2 (200, 65);
		rtTr.anchoredPosition = Vector2.zero;
		var text = go.AddComponent<UnityEngine.UI.Text> ();
		text.fontSize = 24;
		text.fontStyle = FontStyle.Italic;
		text.alignment = TextAnchor.MiddleCenter;
		// text.font = Main.Instance.defaultFont;
		return text;
	}

	public static UnityEngine.UI.Text AddIconText (Transform transform,UnityEngine.TextAnchor anchor,float w)
	{
		var go = new GameObject ();
		UnityTools.SetParent(go.transform,transform);
		RectTransform rtTr = go.AddMissingComponent<UnityEngine.RectTransform> ();
		UpdateAnchor (rtTr, anchor);
		rtTr.sizeDelta = new Vector2 (w, w);
		rtTr.anchoredPosition = Vector2.zero;
		var text = go.AddComponent<UnityEngine.UI.Text> ();
		if (w > 100) {
			text.fontSize = 24;
		} else {
			text.fontSize = 20;
		}
		text.fontStyle = FontStyle.Italic;
		text.alignment = TextAnchor.MiddleCenter;
		// text.font = Main.Instance.defaultFont;
		return text;
	}


	private static void UpdateAnchor(RectTransform rectTransform,UnityEngine.TextAnchor anchor)
	{
//		rectTransform.sizeDelta = Vector2.zero;
//		rectTransform.anchoredPosition = pixelOffset;

		if (anchor == TextAnchor.UpperLeft)
		{
//			alignment = TextAnchor.UpperLeft;
			rectTransform.anchorMin = Vector2.up;
			rectTransform.anchorMax = Vector2.up;
			rectTransform.pivot =Vector2.up;
		}
		else if (anchor == TextAnchor.UpperRight)	
		{
//			alignment = TextAnchor.UpperRight;
			rectTransform.anchorMin = Vector2.one;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot =Vector2.one;
		}
		else if (anchor == TextAnchor.LowerLeft)
		{
//			alignment = TextAnchor.LowerLeft;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.zero;
			rectTransform.pivot = Vector2.zero;
		}
		else if (anchor == TextAnchor.LowerRight)
		{
//			alignment = TextAnchor.LowerRight;
			rectTransform.anchorMin = Vector2.right;
			rectTransform.anchorMax = Vector2.right;
			rectTransform.pivot = Vector2.right;
		}
		else if (anchor == TextAnchor.UpperCenter)
		{
//			alignment = TextAnchor.UpperCenter;
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
		}
		else if (anchor == TextAnchor.LowerCenter)
		{
//			alignment = TextAnchor.LowerCenter;
			rectTransform.anchorMin = new Vector2(0.5f, 0f);
			rectTransform.anchorMax = new Vector2(0.5f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
		}
		else
		{
			rectTransform.anchorMax = new Vector2 (0.5f, 0.5f);
			rectTransform.anchorMin = new Vector2 (0.5f, 0.5f);
			rectTransform.pivot = new Vector2 (0.5f, 0.5f);
		}
	}
}

