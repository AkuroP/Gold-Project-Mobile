using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    [SerializeField] public string itemName;

    public int itemDangerousness;
    public int itemCost;

    public Sprite itemSprite;

    public bool goesInInventory;

    public ShopItem(string _itemName, int _itemDangerousness, bool _inInventory)
    {
        itemName = _itemName;
        itemDangerousness = _itemDangerousness;
        goesInInventory = _inInventory;
    }
}
