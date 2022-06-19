using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    [SerializeField] public string itemName;

    public int itemDangerousness;
    public int itemCost;
    public int itemCooldown;

    public string itemDescription;

    public Sprite itemSprite;
    public Sprite itemHudSprite1;
    public Sprite itemHudSprite2;
    public Sprite itemHudSprite3;

    public bool goesInInventory;

    public ShopItem(string _itemName, int _itemDangerousness, bool _inInventory, string _itemDescription)
    {
        itemName = _itemName;
        itemDangerousness = _itemDangerousness;
        goesInInventory = _inInventory;
        itemDescription = _itemDescription;
    }
}
