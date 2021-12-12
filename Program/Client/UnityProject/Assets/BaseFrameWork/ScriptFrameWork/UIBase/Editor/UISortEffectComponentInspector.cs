using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace XZXD.UI{
[CanEditMultipleObjects]
[CustomEditor (typeof(UISortEffectComponent), true)]
public class UISortEffectComponentInspector : Editor {

	public string[] layers = new string[6];


	protected override void OnHeaderGUI ()
	{
		base.OnHeaderGUI ();

	}

	public override void OnPreviewGUI (Rect r, GUIStyle background)
	{
		base.OnPreviewGUI (r, background);

	}

	public int currentIndex = 0;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		int initIndex = 0;
		layers [initIndex++] = "Default";
		layers [initIndex++] = "Scene";
		layers [initIndex++] = "Guider";
		layers [initIndex++] = "Attention";
		layers [initIndex++] = "Network";
		layers [initIndex++] = "Platform";
		UISortEffectComponent effect = target as UISortEffectComponent;

		currentIndex = 0;
		for (int i = 0; i < layers.Length; i++) {
			if (layers [i] == effect.sortLayerName) {
				currentIndex = i;
			}
		}
		currentIndex = EditorGUILayout.Popup (currentIndex,layers);

        int newOrderIndex = EditorGUILayout.IntField("Order:", effect.orderSort);
        string newLayer = layers[currentIndex];
        if (newOrderIndex != effect.orderSort || newLayer != effect.sortLayerName)
        {
            effect.sortLayerName = newLayer;
            effect.orderSort = newOrderIndex;
            effect.FreshRenderSorrOrder();
        }
	}
}

}