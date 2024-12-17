using System.Collections.Generic;
using UnityEngine;

public class FilterManager : MonoBehaviour
{
    public ShopManager shopManager;

    private void FilterItemsByType(ItemType itemType)
    {
        List<ItemSO> filteredItems = shopManager.shopItem.FindAll(item => item.itemType == itemType);

        shopManager.PopulateShop(filteredItems);
    }

    public void ShowItemsByTypeInt(int itemTypeIndex)
    {
        ItemType itemType = (ItemType)itemTypeIndex;
        FilterItemsByType(itemType);
    }
}
