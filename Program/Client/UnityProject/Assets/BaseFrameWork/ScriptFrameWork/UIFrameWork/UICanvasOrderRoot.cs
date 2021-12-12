using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace XZXD.UI
{
	
	public class UICanvasOrderRoot : MonoBehaviour
	{
		Dictionary<Canvas, int> allCanvasBaseOrder;
		int additionalOrder;
		public int  baseOrder{
			get{
				return additionalOrder;
			}
		}
		public int minOrder{ get; private set; }

		public int maxOrder{ get; private set; }

		public int minAddOrder{ get { return additionalOrder + minOrder; } }

		public int MaxAddOrder{ get { return additionalOrder + maxOrder; } }

		public int deltaOrder{ get; private set; }

		void CalculateCanvas(){
			

			Canvas[] allCanvas = GetComponentsInChildren<Canvas> (true);


			for (int i = 0, imax = allCanvas.Length; i < imax; i++) {
				var canvas = allCanvas [i];
				if (canvas == rootCanvas) {
					continue;
				}
				if (allCanvasBaseOrder.ContainsKey (canvas)) {
					continue;
				}
				int baseOrder = 0;

				CanvasHelper canvasHelper = canvas.GetComponent<CanvasHelper> ();
				if (canvasHelper != null) {
					baseOrder = canvasHelper.sourceOrder;
				} else {
					var ch = canvas.gameObject.AddComponent<CanvasHelper> ();
					baseOrder = canvas.sortingOrder;
					ch.sourceOrder = baseOrder; 
				}

				allCanvasBaseOrder [canvas] = baseOrder;
				minOrder = System.Math.Min (minOrder, baseOrder);
				maxOrder = System.Math.Max (maxOrder, baseOrder);
			}
			deltaOrder = maxOrder - minOrder;

		}
		private Canvas rootCanvas;

		public void Init ()
		{
			rootCanvas = GetComponent<Canvas> ();
			allCanvasBaseOrder = new Dictionary<Canvas, int> ();
			minOrder = int.MaxValue;
			maxOrder = int.MinValue;

			CalculateCanvas ();
			if (rootCanvas != null) {
				int baseOrder = rootCanvas.sortingOrder;//minOrder - 5;
				allCanvasBaseOrder [rootCanvas] = baseOrder;
				minOrder = System.Math.Min (minOrder, baseOrder);
				maxOrder = System.Math.Max (maxOrder, baseOrder);
			}
			deltaOrder = maxOrder - minOrder;

			additionalOrder = 0;
		}

		public void ResetAddtionalOrder (int newAdditionalOrder)
		{
			additionalOrder = newAdditionalOrder;
			foreach (var pair in allCanvasBaseOrder) {
				var canvas = pair.Key;
				if (canvas == null) {
					continue;
				}
				int baseOrder = pair.Value;
				canvas.sortingOrder = baseOrder + additionalOrder;
			}
		}

		public void Reset(){
			CalculateCanvas ();
			ResetAddtionalOrder (additionalOrder);
		}
	}
}