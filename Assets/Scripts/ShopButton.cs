using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if(player.numEssence > item.itemCost)
        {
            if(item.goesInInventory == false)
            {
                switch (item.itemName)
                {
                    case "Heart Regeneration":
                        if(player.hp < player.maxHP)
                        {
                            player.hp++;
                            this.GetComponent<Button>().interactable = false;
                        }
                        break;
                    case "Bonus Heart":
                        player.maxHP++;
                        this.GetComponent<Button>().interactable = false;
                        break;
                }
            }
            else
            {
                for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
                {
                    if (Inventory.instanceInventory.items[i].itemName == "" && item.goesInInventory)
                    {
                        player.numEssence -= item.itemCost;
                        Inventory.instanceInventory.items[i] = item;
                        this.GetComponent<Button>().interactable = false;
                        break;
                    }
                }
            }
        }
    }
}
