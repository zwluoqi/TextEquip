using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace XZXD.UI
{
	public class UIManager : MonoBehaviour
	{
		#region Static Fields

		private static UIManager _ins;

		#endregion

		#region Static Properties

		public static UIManager Instance {
			get {
				if (_ins == null) {
//					throw new System.Exception ("Current scene not have UIRootManager");
					var go = UITools.LoadUIObject("ui/UIRoot",null);
					_ins = go.GetComponent<UIManager> ();
				}
				return _ins;
			}
		}

		#endregion

		#region Fields

		public Camera uiCamera;

		public Transform rootScene;
		public Transform rootMain;
		public Transform rootGuider;
		public Transform rootAttention;
		public Transform rootNetwork;
		public Transform rootPlatform;
		public GameObject m_cacheLayer; // 缓存层
		#endregion

		#region Methods
		public float width;
		public float height;

		void Awake ()
		{
			if (_ins == null) {
				_ins = this;
				DontDestroyOnLoad (gameObject);
			}
			width = Screen.width;
			height = Screen.height;
			var radio = width * 1.0f / height;
			var standRadio = 9.0f / 16.0f;
			if (radio > standRadio) {
				GetComponent<CanvasScaler> ().matchWidthOrHeight = 1;
			} else {
				GetComponent<CanvasScaler> ().matchWidthOrHeight = 0;
			}
		}

		/// <summary>
		/// 获取某层的跟节点
		/// </summary>
		public Transform GetLayerRoot (UILayerEnum layer)
		{
			switch (layer) {
			case UILayerEnum.SCENE:
				return rootScene;
			case UILayerEnum.MAIN:
				return rootMain;
			case UILayerEnum.GUIDER:
				return rootGuider;
			case UILayerEnum.ATTENTION:
				return rootAttention;
			case UILayerEnum.NETWORK:
				return rootNetwork;
			case UILayerEnum.PLATFORM:
				return rootPlatform;
			default:
				return null;
			}
		}

		public string GetCanvasLayerName (UILayerEnum layer)
		{
			switch (layer) {
			case UILayerEnum.SCENE:
				return "Scene";
			case UILayerEnum.MAIN:
				return "Default";
			case UILayerEnum.GUIDER:
				return "Guider";
			case UILayerEnum.ATTENTION:
				return "Attention";
			case UILayerEnum.NETWORK:
				return "Network";
			case UILayerEnum.PLATFORM:
				return "Platform";
			}
			return "";
		}

		public void SetCanvasLayer (Canvas canvas, UILayerEnum layer, int baseOrder)
		{
			string layerName = GetCanvasLayerName (layer);
			if (layerName.Length > 0) {
				canvas.overrideSorting = true;
				canvas.sortingLayerName = layerName;
				canvas.sortingOrder = baseOrder;
			} else {
				canvas.overrideSorting = false;
			}
		}

		public float GetUIWidth(){
			return this.rootMain.parent.GetComponent<RectTransform> ().sizeDelta.x;
		}

		public float GetUIHeight(){
			return this.rootMain.parent.GetComponent<RectTransform> ().sizeDelta.y;
		}

		/// <summary>
		/// 增加到缓存层
		/// </summary>
		void AddCacheLayer(GameObject panel)
		{
			if(panel != null)
			{
				panel.transform.SetParent (m_cacheLayer.transform);
			}
		}

//		public Transform GetCacheLayer(){
//			return m_cacheLayer.transform;
//		}

		public Dictionary<string,GameObjectPool> cacheGamePools = new Dictionary<string, GameObjectPool>();



		public GameObjectPool GetOrNewPool (string poolName,GameObject itemPrefab)
		{
			if (!cacheGamePools.ContainsKey (poolName)) {
				var go = GameObject.Instantiate (itemPrefab) as GameObject;
				go.SetActive (true);
				UIManager.Instance.AddCacheLayer (go);
				GameObjectPool pool = new GameObjectPool (go, 10, go.GetInstanceID ());
				pool.HideInHierarchy (this.m_cacheLayer.transform);
				cacheGamePools.Add (poolName, pool);
			}
			return cacheGamePools [poolName];

		}

		public void UnSpwan (GameObjectPool taskPool, GameObject item)
		{
			taskPool.Unspawn (item);
			AddCacheLayer (item);
		}

		public void Release ()
		{
			foreach (var kv in cacheGamePools) {
				kv.Value.UnspawnAllAndDestroy ();
				GameObject.Destroy (kv.Value.GetPrefab ());
			}
			cacheGamePools.Clear ();
		}
		#endregion

	}
}

