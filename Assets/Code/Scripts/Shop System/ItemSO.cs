using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon = 0,
    Ammo = 1,
    Health = 2
}

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public int ID => GetInstanceID();

    //ITEM BASIC INFORMATION
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string Description { get; set; }

    [field: SerializeField]
    public Sprite ItemImage { get; set; }
    
    //ITEM RARITY, PRICE AND TYPE 
    [field: SerializeField]
    public int rarity { get; set; } = 0;
    [field: SerializeField]
    public int price { get; set; }
    public float growthRate;
    public string weaponName;

    public int ammoTotal;

    public ItemType itemType;
}
