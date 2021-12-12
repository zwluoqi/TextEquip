using System;
using System.Collections.Generic;
using Script.Game.Grow;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class BagPage:UIPage
{
        public GridLayoutGroup gridLayoutGroup;
        public BagItemUI bagItemUiPrefab;

        public GameObject autoCostBtn;
        public GameObject onekeyCostBtn;
        public GameObject onekeySortBtn;
        
        
        private List<BagItemUI> items = new List<BagItemUI>();

        private void Awake()
        {
                bagItemUiPrefab.gameObject.SetActive(false);
                UGUIEventListener.Get(autoCostBtn).onClick = delegate(GameObject go)
                {
                        UIPageManager.Instance.OpenPage("SetAutoCostPage", "");
                };
                UGUIEventListener.Get(onekeyCostBtn).onClick = delegate(GameObject go)
                {
                        GrowFun.Instance.growData.CostEquips();
                        Fresh();
                };
                UGUIEventListener.Get(onekeySortBtn).onClick = delegate(GameObject go)
                {
                        GrowFun.Instance.growData.SortEquips();
                        Fresh();
                };
        }


        protected override void DoOpen()
        {
                Fresh();
        }

        void Fresh()
        {
                Clear();
                foreach (var growEquip in GrowFun.Instance.growData.growEquips)
                {
                        var item = GameObject.Instantiate(bagItemUiPrefab);
                        UnityTools.SetCenterParent(item.transform,gridLayoutGroup.transform);
                        item.gameObject.SetActive(true);
                        item.Init(growEquip);
                        items.Add(item);
                }
        }

        protected override void DoOnCoverPageRemove(UIPage coverPage)
        {
                Fresh();
        }

        void Clear()
        {
                foreach (var item in items)
                {
                        GameObject.Destroy(item.gameObject);
                }
                items.Clear();
        }

        protected override void DoClose()
        {
                Clear();
        }
}