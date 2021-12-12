using System.Collections.Generic;
using Script.Game;
using Script.Game.Grow;
using Script.Game.Grow.NetData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XZXD.UI;

public class ShopPage:UIPage
{
    public ShopItem shopItem;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject modeBtn;
    public GameObject freshBtn;
    public TMP_Text shopModeBtn;
    public GameObject prePage;
    public GameObject nextPage;
    public TMP_Text curPage;
    public GameObject none;
    int page;

    private List<ShopItem> items = new List<ShopItem>();
    
    
    void Awake()
    {
        UGUIEventListener.Get(none).onClick = delegate(GameObject go)
        {
            UIPageManager.Instance.OpenPage("BagPage", "");
        };
        
        shopItem.gameObject.SetActive(false);
        UGUIEventListener.Get(freshBtn).onClick = delegate(GameObject go)
        {
            Clear();
            ShopUtil.RequestShopItems(page,true);
        };
        UGUIEventListener.Get(prePage).onClick = delegate(GameObject go)
        {
            --page;
            if (page < 0)
            {
                page = 10;
            }
            Clear();
            ShopUtil.RequestShopItems(page,false);
        };
        UGUIEventListener.Get(nextPage).onClick = delegate(GameObject go)
        {
            ++page;
            if (page > 10)
            {
                page = 0;
            }
            Clear();
            ShopUtil.RequestShopItems(page,false);
        };
        UGUIEventListener.Get(modeBtn).onClick = delegate(GameObject go)
        {
            if (GrowFun.Instance.shopItemFromNet)
            {
                BoxManager.OpenYesAndNoPage("是否切换到本地商店,本地商店的商品来源于开发者", delegate(bool b)
                {
                    if (b)
                    {
                        
                        shopModeBtn.text = "网络模式";
                        GrowFun.Instance.shopItemFromNet = false;
                        Clear();
                        ShopUtil.RequestShopItems(page,true);
                    }
                }); 
            }
            else
            {
                if (GrowFun.Instance.remote_id > 0)
                {
                    BoxManager.OpenYesAndNoPage("是否切换到网络商店,网络商店的商品来源于其他玩家", delegate(bool b)
                    {
                        if (b)
                        {
                            shopModeBtn.text = "本地模式";
                            GrowFun.Instance.shopItemFromNet = true;
                            Clear();
                            ShopUtil.RequestShopItems(page,true);
                        }
                    }); 
                }
                else
                {
                    if (string.IsNullOrEmpty( GrowFun.Instance.growData.growPlayer.playerName))
                    {
                        BoxManager.OpenYesAndNoPage("切换到网络商店需要网络进行登陆,是否进行登陆？", delegate(bool b)
                        {
                            if (b)
                            {
                                UIPageManager.Instance.OpenPage("RegisterPage", "");
                            }
                        });
                    }
                    else
                    {
                        AccountUtil.RequestLogin(false, GrowFun.Instance.growData.growPlayer.playerName);
                    }
                }
            }
        };
        NotificationCenter.Default.AddObserver(this,OnFreshShopItems,(int)GameMessageId.SCGetShopItems);
        NotificationCenter.Default.AddObserver(this,OnLogin,(int)GameMessageId.SCLoginDone);
        NotificationCenter.Default.AddObserver(this,OnBuyDone,(int)GameMessageId.SCBuyShopEquip);

    }

    private void OnBuyDone(Notification notification)
    {
        FrehsUI();
    }

    private void OnLogin(Notification notification)
    {
        var register = (bool)notification.info;

        if (register)
        {
            BoxManager.OpenYesAndNoPage("是否切换到网络商店,网络商店的商品来源于其他玩家", delegate(bool b)
            {
                if (b)
                {
                    
                    shopModeBtn.text = "本地模式";
                    GrowFun.Instance.shopItemFromNet = true;
                    Clear();
                    ShopUtil.RequestShopItems(page, true);
                }
            });
        }
        else
        {
            shopModeBtn.text = "本地模式";
            GrowFun.Instance.shopItemFromNet = true;
            Clear();
            ShopUtil.RequestShopItems(page, true);
        }
    }

    void OnFreshShopItems(Notification n){
        FrehsUI();
    }

    // protected override void DoOnCoverPageRemove(UIPage coverPage)
    // {
    //     ShopUtil.RequestShopItems(page,true);
    // }

    protected override void DoOpen()
    {
        shopModeBtn.text = "网络模式";
        Clear();
        ShopUtil.RequestShopItems(page,true);
    }

    void SimpleFreshUI()
    {
        nextPage.SetActive(GrowFun.Instance.shopItemFromNet);
        prePage.SetActive(GrowFun.Instance.shopItemFromNet);
        curPage.gameObject.SetActive(GrowFun.Instance.shopItemFromNet);
        if (GrowFun.Instance.shopItemFromNet)
        {
            shopModeBtn.text = "本地模式";
        }
        else
        {
            shopModeBtn.text = "网络模式";
        }
    }
    
    
    void FrehsUI()
    {
        SimpleFreshUI();
        curPage.text = "第" + page + "页";
        var shopData = ShopUtil.GetShopData(page);
        if (shopData != null)
        {
            Clear();
            foreach (var growEquip in shopData.equips)
            {
                var item = GameObject.Instantiate(shopItem);
                UnityTools.SetCenterParent(item.transform,gridLayoutGroup.transform);
                item.gameObject.SetActive(true);
                item.Init(growEquip);
                items.Add(item);
            }
            foreach (var growEquip in shopData.drops)
            {
                var item = GameObject.Instantiate(shopItem);
                UnityTools.SetCenterParent(item.transform,gridLayoutGroup.transform);
                item.gameObject.SetActive(true);
                item.Init(growEquip);
                items.Add(item);
            }

            if (shopData.equips.Count == 0)
            {
                none.SetActive(true);
            }
            else
            {
                none.SetActive(false);

            }

        }
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