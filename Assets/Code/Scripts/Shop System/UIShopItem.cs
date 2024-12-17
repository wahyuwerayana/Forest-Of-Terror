using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    public Image frame;
    public Image icon;
    public Button viewDescriptionButton;
    public Button addToCart;
    public TextMeshProUGUI price;

    private ItemSO itemData;
    private ShopManager shopManager;

    [Header("Item Rarity")]
    public Sprite rarity1;
    public Sprite rarity2;
    public Sprite rarity3;
    // Initialize the item
    public void SetUp(ItemSO item, ShopManager manager, int purchaseCount)
    {
        SetItemFrameByRarity(item);
        itemData = item;
        shopManager = manager;
        int value = GetPrice(item.price, item.growthRate, purchaseCount);
        price.text = value.ToString();

        icon.sprite = item.ItemImage;
        viewDescriptionButton.onClick.AddListener(() => shopManager.ShowItemDetails(itemData));
        addToCart.onClick.AddListener(() => shopManager.PopUpCart(itemData));
    }

    private void SetItemFrameByRarity(ItemSO item)
    {
        if (item.rarity == 1)
        {
            frame.sprite = rarity1;
        }
        else if (item.rarity == 2)
        {
            frame.sprite = rarity2;
        }
        else
        {
            frame.sprite = rarity3;
        }
    }

    private int GetPrice(int basePrice, float growthRate, int purchaseCount){
        return (int)(basePrice * Mathf.Pow(1 + growthRate, purchaseCount));
    }
}
