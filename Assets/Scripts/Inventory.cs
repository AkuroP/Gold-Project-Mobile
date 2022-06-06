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

    public bool HasItem(string itemName)
    {
        for(int i = 0; i < maxItemNumber; i++)
        {
            if(itemName == items[i].itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0; i < maxItemNumber; i++)
        {
            if (itemName == items[i].itemName)
            {
                items[i].itemName = "";
                items[i].itemSprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            }
        }
    }
}
