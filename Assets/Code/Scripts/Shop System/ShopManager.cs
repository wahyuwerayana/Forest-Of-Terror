using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject Shop;
    public GameObject itemUI;
    public GameObject desc;
    
    [Header("Shop Data")]
    public List<ItemSO> shopItem;
    public Transform itemsParent;
    public GameObject itemPrefab;

    public Image frameRarity;
    public Image itemImage;
    
    public TMP_Text itemName;
    public TMP_Text itemDescription;

    [Header("Item Rarity")]
    public Sprite rarity1;
    public Sprite rarity2;
    public Sprite rarity3;

    [Header("Total Price (DO NOT EDIT VALUES)")]
    public float cartCount = 0;

    [Header("UI")]
    public GameObject popUpPanel;
    public TMP_Text buyItemText;

    [Header("Player Attributes")]
    [SerializeField] private Health playerHealthScript;
    [SerializeField] private WeaponSystem weaponSystemScript;
    [SerializeField] private List<GameObject> weaponsList;
    [SerializeField] private Dictionary<ItemSO, Weapon> weaponsDictionary;

    private List<int> purchaseCount;

    private ItemSO currentSelectedItem;
    private int currentSelectedItemPrice;

    private void Start()
    {
        Initialize();
        InitializeDictionary();
    }

    public void Initialize()
    {
        purchaseCount = new List<int>(new int[shopItem.Count]);
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        SortItemByRarity();
        int index = 0;
        foreach (ItemSO item in shopItem)
        {
            GameObject newItem = Instantiate(itemPrefab, itemsParent);
            UIShopItem itemUI = newItem.GetComponent<UIShopItem>();
            itemUI.SetUp(item, this, purchaseCount[index]);
            index++;
        }
    }

    private void InitializeDictionary(){
        weaponsDictionary = new Dictionary<ItemSO, Weapon>();
        foreach(ItemSO itemSO in shopItem){
            if(itemSO.itemType == ItemType.Ammo){
                foreach(GameObject weapon in weaponsList){
                    if(itemSO.weaponName == weapon.name){
                        Weapon weaponScript = weapon.GetComponent<Weapon>();
                        weaponsDictionary.Add(itemSO, weaponScript);
                        break;
                    }
                }
            }
        }
    }

    public void PopulateShop(List<ItemSO> itemsToDisplay)
    {
        // Clear current items
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }
        SortItemByRarity();
        // Populate with the filtered items
        int index = 0;
        foreach (ItemSO item in itemsToDisplay)
        {
            GameObject newItem = Instantiate(itemPrefab, itemsParent);
            UIShopItem itemUI = newItem.GetComponent<UIShopItem>();
            itemUI.SetUp(item, this, purchaseCount[index]);
            index++;
        }
    }

    public void ShowItemDetails(ItemSO item)
    {
        SetItemFrameByRarity(item);
        itemImage.sprite = item.ItemImage;
        itemName.text = item.Name;
        itemDescription.text = item.Description;

        itemUI.SetActive(true);
        desc.SetActive(true);
    }

    private void SetItemFrameByRarity(ItemSO item)
    {
        if(item.rarity == 1)
        {
            frameRarity.sprite = rarity1;
        }else if (item.rarity == 2)
        {
            frameRarity.sprite = rarity2;
        }
        else
        {
            frameRarity.sprite = rarity3;
        }
    }

    private void SortItemByRarity()
    {
        //low to high
        shopItem.Sort((item1, item2) => item1.rarity.CompareTo(item2.rarity));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Shop.SetActive(!Shop.activeSelf);
            SetActiveShop(Shop.activeSelf);
            //always reset cart count when open or close the shop
            cartCount = 0;
        }
    }

    private void SetActiveShop(bool shopCondition){
        CardManager.Instance.GetCurrentWeapon();

        if(shopCondition){
            popUpPanel.SetActive(false);
            CardManager.Instance.currentActiveWeapon.showCrosshair = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        } else{
            CardManager.Instance.currentActiveWeapon.showCrosshair = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void PopUpCart(ItemSO item, int newPrice){
        if(CoinsManager.Instance.coinsValue - newPrice < 0)
        return;

        currentSelectedItem = item;
        currentSelectedItemPrice = newPrice;
        popUpPanel.SetActive(true);
        buyItemText.text = "Buy " + item.Name + "?";
    }

    public void YesButton(){
        CoinsManager.Instance.ChangeCoinsValue(-currentSelectedItemPrice);
        
        switch(currentSelectedItem.itemType){
            case ItemType.Health:
                BuyHealItem(currentSelectedItem.Name);
                break;
            case ItemType.Weapon:
                BuyWeaponItem(currentSelectedItem.Name);
                break;
            case ItemType.Ammo:
                BuyAmmoItem(currentSelectedItem);
                break;
        }
        
        IncrementPrice();
        PopulateShop(shopItem);
        popUpPanel.SetActive(false);
    }

    public void NoButton(){
        popUpPanel.SetActive(false);
    }

    private void BuyHealItem(string itemName){
        switch (itemName){
            case "Full Health":
                playerHealthScript.ChangeHealth(playerHealthScript.maxHealth);
                break;
            case "HP +20%":
                playerHealthScript.ChangeHealth(playerHealthScript.maxHealth * 0.2f);
                break;
            case "HP +50%":
                playerHealthScript.ChangeHealth(playerHealthScript.maxHealth * 0.5f);
                break;
            default:
                break;
        }
    }

    private void BuyWeaponItem(string weaponName){
        foreach(GameObject weapon in weaponsList){
            if(weapon.name == weaponName){
                weaponSystemScript.weapons.Add(weapon);
                weaponsList.Remove(weapon);
                break;
            }
        }
        
        int index = shopItem.IndexOf(currentSelectedItem);
        purchaseCount.RemoveAt(index);
        shopItem.Remove(currentSelectedItem);
    }

    private void BuyAmmoItem(ItemSO currentItemSO){
        if(weaponsDictionary[currentItemSO].type == WeaponType.Beam){
            weaponsDictionary[currentItemSO].currentAmmo += currentItemSO.ammoTotal;
        } else{
            weaponsDictionary[currentItemSO].reservedAmmo += currentItemSO.ammoTotal;
        }
        Debug.Log(currentItemSO.ammoTotal);

        WeaponSystem.Instance.UpdateAmmoText(weaponsDictionary[currentItemSO].currentAmmo,
            weaponsDictionary[currentItemSO].reservedAmmo, 
            weaponsDictionary[currentItemSO].unlimitedMagazine,
            weaponsDictionary[currentItemSO].type);
    }

    private void IncrementPrice(){
        if(currentSelectedItem.itemType != ItemType.Weapon){
            int index = shopItem.IndexOf(currentSelectedItem);
            purchaseCount[index] += 1;
        }
    }
}
