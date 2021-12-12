using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GridAutoApet : MonoBehaviour
{
    private UnityEngine.UI.GridLayoutGroup _group;
    private RectTransform _rect;
    // Start is called before the first frame update
    void Start()
    {
        _group = this.GetComponent<GridLayoutGroup>();
        _rect  = this.GetComponent<RectTransform>();
        if (delayTransChanged)
        {
            UpdateContentHeight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int childCount = 1;
    private bool delayTransChanged = false;
    public void OnTransformChildrenChanged()
    {
        if (_group == null)
        {
            delayTransChanged = true;
            return;
        }
        if (this.childCount != this.transform.childCount)
        {
            UpdateContentHeight();
        }
    }

    private void UpdateContentHeight()
    {
        delayTransChanged = false;
        this.childCount = this.transform.childCount;
        var heiCount = (this.childCount / _group.constraintCount)+1;
        _rect.sizeDelta = new Vector2(_rect.sizeDelta.x,(_group.spacing.y+_group.cellSize.y)*heiCount);
        // Debug.LogWarning("grid count:"+this.transform.childCount);

    }
}
