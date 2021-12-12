// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
//
// namespace XZXD.UI{
// public class UIEffectAutoLoader : UITimer {
// 	
// 	//[HideInInspector]
// 	public string effectPath;
// 	//[HideInInspector]
//     public string guid;
// 	public Action<GameObject> onEffectFinish;
//
// 	private GameObject effectPrefab;
//
// //    GameObjectGetter getter;
//
// 	public override void OnEnable0 ()
// 	{
// 		if (effectPrefab == null) {
// 			effectPrefab = GameObjectPoolManager.Instance.GetGameObjectDirect("effect/" + effectPath);
// //            effectPrefab = getter.GetInstantiateGO();
//             if (effectPrefab != null)
//             {
//                 UnityTools.SetParent(effectPrefab.transform, this.transform);
//                 UnityTools.SetLayer(this.gameObject.layer, effectPrefab.transform);
//             }
//             else
//             {
//                 Debug.LogError("effect auto loader error:" + effectPath);
//             }
// 		}
//
// 		FreshState ();
// 	}
//
// 	public override void OnDisable0 ()
// 	{
// 		if (effectPrefab != null) {
// 			effectPrefab.SetActive (false);
//             if (onEffectFinish != null)
//             {
//                 onEffectFinish(this.gameObject);
//             }
// 		}
// 	}
//
// 	public override void OnShowable ()
// 	{
// 		FreshState ();
// 	}
// 	public override void OnDisShowable ()
// 	{
// 		FreshState ();
// 	}
//
//     void Destory() {
// //        getter.ReleaseInstantiateGO(effectPrefab);
//     }
//
// 	void FreshState(){
// 		if (effectPrefab != null) {
// 			if (show) {
// 				effectPrefab.SetActive (true);	
// 			} else {
// 				effectPrefab.SetActive (false);	
// 			}
// 		}
// 	}
//
// }
//
// }