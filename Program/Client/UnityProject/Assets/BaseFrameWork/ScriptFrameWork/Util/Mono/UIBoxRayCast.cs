using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Box Collider")]
public class UIBoxRayCast:Graphic
{
        public override void SetMaterialDirty()
        {
                
        }

        public override void SetVerticesDirty()
        {
                
        }

        protected override void OnFillVBO(List<UIVertex> vbo)
        {
                vbo.Clear();
        }
}