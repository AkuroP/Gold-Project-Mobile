using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    public string itemName;

    public int itemDangerousness;

    [SerializeField] Sprite itemSprite;

    private bool goesInInventory;

    public Item(string _itemName, int _itemDangerousness, bool _inInventory)
    {
        itemName = _itemName;
        itemDangerousness = _itemDangerousness;
        goesInInventory = _inInventory;
    }
}
