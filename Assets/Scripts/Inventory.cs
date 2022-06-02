using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instanceInventory;

    public ShopItem[] items = new ShopItem[3];

    public int maxItemNumber = 3;

    private void Awake()
    {
        if (instanceInventory != null)
        {
            Destroy(instanceInventory);
        }
        instanceInventory = this;
    }
}
