using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XZXD.UI
{
//	[ExecuteInEditMode]
	public class UIGridCellAdapt : MonoBehaviour
	{
		public UnityEngine.UI.GridLayoutGroup grid;
		public float defaultWidth;
		public float defaultHeight;


		public float widthRadioFactor = 1;
		public float heightRadioFactor = (9.0f / 16.0f) / (9.0f / 19.5f);

		void Awake(){
			grid = GetComponent<UnityEngine.UI.GridLayoutGroup> ();
			if (defaultWidth == 0) {
				return;
			}
			if (defaultHeight == 0) {
				return;
			}
			var width = Screen.width;
			var height = Screen.height;
			var radio = width * 1.0f / height;
			var standRadio = 9.0f / 16.0f;
			if (radio > standRadio) {
				grid.cellSize = new Vector2 (defaultWidth,defaultHeight * 1);
			} else {
				var scaleY = standRadio / radio;
				grid.cellSize = new Vector2 (defaultWidth,defaultHeight * scaleY);
			}
		}
	}
}