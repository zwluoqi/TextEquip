using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target3Dto2DPos : MonoBehaviour {

	public GameObject threedObj;
	public GameObject twoObj;
	public CanvasScaler canvas;
	public Camera uiCamera;
	public Camera threeCamera;

	public Vector3 pos;
	public Vector3 pos1;
	public Vector3 pos2;
	public Vector3 pos3;
	public Vector3 globalMousePos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Camera rightCam = threeCamera;
		pos = rightCam.WorldToScreenPoint (threedObj.transform.position);
//		pos.z = 0;
//		Vector2 pos = RectTransformUtility.WorldToScreenPoint(rightCam, threedObj.transform.position);
		pos1 = uiCamera.ScreenToWorldPoint(pos);
		pos2 = new Vector3 (canvas.referenceResolution.x * (pos1.x - 0.0f), canvas.referenceResolution.y * (pos1.y - 0.0f), 0);
		pos3 = new Vector3 (pos.x-Screen.width/2+0, pos.y-Screen.height/2);
		//        this.transform.GetComponent<RectTransform>().anchoredPosition = pos2;

		if (RectTransformUtility.ScreenPointToWorldPointInRectangle (twoObj.GetComponent<RectTransform>(), pos, uiCamera, out globalMousePos)) {
			twoObj.transform.position = globalMousePos;
		}

		this.twoObj.GetComponent<Transform>().position = pos1;


	}
}
