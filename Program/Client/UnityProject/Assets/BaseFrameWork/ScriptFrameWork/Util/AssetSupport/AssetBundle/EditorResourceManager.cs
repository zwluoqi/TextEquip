//using System.Collections.Generic;
//using UnityEngine;
//using System.Collections;
//using System.IO;
//
//
//#if UNITY_EDITOR
//public class EditorResourceManager
//{
//
//
//	private string GetResourcePahtWithExtend(string resourcePath){
//		int lastSplitIndex = resourcePath.LastIndexOf ('/');
//		string dirName;
//		string assetName;
//		if (lastSplitIndex != -1)
//		{
//			dirName = resourcePath.Substring(0, lastSplitIndex);
//			assetName = resourcePath.Substring(lastSplitIndex + 1);
//		}
//		else
//		{
//			dirName = "";
//			assetName = resourcePath;
//		}
//		string[] filePaths = Directory.GetFiles ("Assets/GameResources/" + dirName);
//		foreach (var filePath in filePaths) {
//			if (filePath.EndsWith (".meta"))
//				continue;
//			string fileName = Path.GetFileName (filePath).ToLower();
//			if (fileName.StartsWith (assetName.ToLower() + ".")) {
//				return filePath;
//			}
//		}
//
//		return "Assets/GameResources/"+resourcePath;
//	}
//
//	/// <summary>
//	/// 阻塞加载资源,会增加引用计数器，注意：会阻塞主线程，谨慎使用！！！
//	/// </summary>
//	/// <param name="resourcePath">资源路径</param>
//	/// <returns>加载得到的资源</returns>
//	public Object Load(string resourcePath)
//	{
//		resourcePath = resourcePath.Replace (".ab","");
//		Object asset = null;
//		if (!resourcePath.Contains (".")) {
//			asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object> (GetResourcePahtWithExtend (resourcePath));
//		} else {
//			asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object> ("Assets/GameResources/" +resourcePath);
//		}
//
//		return asset;
//	}
//
//
//	/// <summary>
//	/// 阻塞加载资源,会增加引用计数器，注意：会阻塞主线程，谨慎使用！！！
//	/// </summary>
//	/// <param name="resourcePath">资源路径</param>
//	/// <returns>加载得到的资源</returns>
//	public Object[] LoadAll(string resourcePath)
//	{
//		resourcePath = resourcePath.Replace (".ab","");
//		Object[] assets = null;
//		if (Directory.Exists ("Assets/GameResources/" + resourcePath)) {
//			assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath ("Assets/GameResources/" + resourcePath);
//		} else {
//			assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath (GetResourcePahtWithExtend (resourcePath));
//		}
//
//		return assets;
//	}
//
//
//	/// <summary>
//	/// 异步加载资源,addRefCount： 是否缓存并增加引用计数器
//	/// </summary>
//	/// <param name="resourcePath"></param>
//	public void LoadAsync(string resourcePath, System.Action<string,Object> act)
//	{
//
//		act (resourcePath, Load (resourcePath));
//	}
//		
//
//
//
//}
//
//#endif