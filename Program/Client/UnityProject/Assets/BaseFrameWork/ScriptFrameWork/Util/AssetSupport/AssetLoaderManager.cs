using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoaderManager
{
	public InnerResourceManager innerResourceManager{ private set; get; }


	public bool loadStream = false;
    public bool loadInnerResource = true;

	/// <summary>
	/// 正在加载的资源列表
	/// </summary>
	Dictionary<string, Void_STR_UnityEngineObject> loadingCallbackList = new Dictionary<string, Void_STR_UnityEngineObject> ();

	/// <summary>
	/// 直接获取资源，不会改动引用计数器
	/// </summary>
	/// <param name="resourcePath"></param>
	/// <returns>资源</returns>
	public Object GetResource (string resourcePath)
	{
		return innerResourceManager.GetResource(resourcePath);
	}

	/// <summary>
	/// 阻塞加载资源,会增加引用计数器，注意：会阻塞主线程，谨慎使用！！！
	/// </summary>
	/// <param name="resourcePath">资源路径</param>
	/// <returns>加载得到的资源</returns>
	public Object LoadResourceBlock (string resourcePath)
	{

		Object asset = this.innerResourceManager.Load (resourcePath);

		return asset;
	}


	/// <summary>
	/// 阻塞加载资源,会增加引用计数器，注意：会阻塞主线程，谨慎使用！！！
	/// </summary>
	/// <param name="resourcePath">资源路径</param>
	/// <returns>加载得到的资源</returns>
	public Object[] LoadAllResourceBlock (string resourcePath)
	{

		Object[] assets = this.innerResourceManager.LoadAll (resourcePath);

		return assets;
	}

	/// <summary>
	/// 异步加载资源,addRefCount： 是否缓存并增加引用计数器
	/// </summary>
	/// <param name="resourcePath"></param>
	public void LoadResourceAsync (string resourcePath, Void_STR_UnityEngineObject asyncCallBack = null)
	{

		if (loadingCallbackList.ContainsKey (resourcePath)) {
			loadingCallbackList [resourcePath] += asyncCallBack;
		} else {
			loadingCallbackList.Add (resourcePath, null);
			loadingCallbackList [resourcePath] += asyncCallBack;
			LoadResourceAsync0 (resourcePath);
		}
	}

	private void LoadResourceAsync0 (string resourcePath)
	{


		this.innerResourceManager.LoadAsync (resourcePath, (_resourcePath, _asset) => {
			HandlerAsyncCallBack (resourcePath, _asset);
		});



	}

	private void HandlerAsyncCallBack (string resourcePath, Object asset)
	{

		if (!loadingCallbackList.ContainsKey (resourcePath)) {
			Debug.LogError ("loadingCallbackList:" + resourcePath + " had be removed");
		}

		if (asset == null) {
			Debug.LogError ("equest.asset null:" + resourcePath);
		}


		Void_STR_UnityEngineObject asyncCallBack = loadingCallbackList [resourcePath];

		loadingCallbackList.Remove (resourcePath);

		if (asyncCallBack != null) {
			asyncCallBack (resourcePath, asset);
		}
	}

	/// <summary>
	/// 卸载资源, 引用计数为零将其从内存中清除
	/// </summary>
	/// <param name="resourcePath">资源路径</param>
	public void UnloadResource (string resourcePath)
	{
		innerResourceManager.UnloadResource (resourcePath);
	}

	public void Release(){
		innerResourceManager.Release ();
		Resources.UnloadUnusedAssets ();
	}

	private static AssetLoaderManager instance;

	public static AssetLoaderManager Instance {
		get {
			if (instance == null) {
				instance = new AssetLoaderManager ();
				instance.innerResourceManager = new InnerResourceManager ();
			}
			return instance;
		}
	}
}

