using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instanceInventory;

    //Inventory management
    public int maxItemNumber = 3;
    public int itemInInventory = 0;

    //Mystery Box
    public bool mysteryBoxInShop = false;
    public int mysteryBoxDangerousness = 0;

    public ShopItem[] items = new ShopItem[3];

    private void Awake()
    {
        if (instanceInventory != null)
        {
            Destroy(instanceInventory);
        }
        instanceInventory = this;
    }
}
