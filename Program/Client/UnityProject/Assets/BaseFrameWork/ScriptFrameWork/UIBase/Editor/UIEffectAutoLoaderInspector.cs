// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
//
// namespace XZXD.UI{
// 	[CanEditMultipleObjects]
// 	[CustomEditor (typeof(UIEffectAutoLoader), true)]
//
// 	public class UIEffectAutoLoaderInspector : Editor {
//
// 		private GameObject obj;
//
// 		public override void OnInspectorGUI (){
// 			base.OnInspectorGUI ();
// 			var loader = target as UIEffectAutoLoader;
//
//             if (obj == null)
//             {
// 				if (string.IsNullOrEmpty(loader.effectPath))
//                 {
//                     ;
//                 }
//                 else
//                 {
// 					obj = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/effect/" + loader.effectPath + ".prefab");
//                 }
//             }
//
// 			obj =(GameObject) EditorGUILayout.ObjectField ("目标(不会直接引用对象)", obj, typeof(GameObject), true);
//
// 			if (obj != null) {
// 				string path = AssetDatabase.GetAssetPath (obj);
// 				loader.guid = AssetDatabase.AssetPathToGUID (path);
//
// 				path = path.Replace ('\\', '/');
// 				var effectPath = path.Replace ("Assets/Resources/effect/", "");
// 				loader.effectPath = effectPath.Replace (".prefab", "");
// 			}
// 		}
// 	}
//
// }