using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{

    public ShopItem item;

    private Player player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void ChooseItem()
    {
        
        for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
        {
            if (Inventory.instanceInventory.items[i].itemName == "" && player.numEssence > item.itemCost && item.goesInInventory)
            {
                player.numEssence -= item.itemCost;
                Inventory.instanceInventory.items[i] = item;
                break;
            }
        }
    }
}
