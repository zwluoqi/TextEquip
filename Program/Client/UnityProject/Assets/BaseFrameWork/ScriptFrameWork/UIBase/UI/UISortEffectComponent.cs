using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XZXD.UI
{
	[ExecuteInEditMode]
	public class UISortEffectComponent:MonoBehaviour
	{
		public UIPage page;
        [HideInInspector]
		public int orderSort;
		[HideInInspector]
		public string sortLayerName;


#if UNITY_EDITOR
        private void OnEnable()
        {
            FreshRenderSorrOrder();
        }
#endif

        void Start(){
			FreshRenderSorrOrder ();
		}

		void OnTransformChildrenChanged(){
			FreshRenderSorrOrder ();
		}

		public void FreshRenderSorrOrder(){
			int baseOrder = 0;
			if (page == null) {
				page = this.transform.GetComponentOrInParent <UIPage>();
			}
			if (page != null) {
				var canvans = page.GetComponent<UICanvasOrderRoot> ();
				if (canvans != null) {
					baseOrder = canvans.baseOrder;
				}
			}
			List<Renderer> renders = new List<Renderer> ();
			this.gameObject.GetComponentsInChildren<Renderer> (true,renders);
			foreach (var render in renders) {
				render.sortingOrder = baseOrder + orderSort;
				render.sortingLayerName = sortLayerName;
			}
			List<SkinnedMeshRenderer> skinnedRenders = new List<SkinnedMeshRenderer> ();
			this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> (true,skinnedRenders);
			foreach (var render in skinnedRenders) {
				render.sortingOrder = baseOrder + orderSort;
				render.sortingLayerName = sortLayerName;
			}
		}
	}
}

