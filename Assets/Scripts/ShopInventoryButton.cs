using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventoryButton : MonoBehaviour
{

    public ShopItem item;
    public ShopItem itemToAdd;

    [SerializeField] Button[] shopButtons;
    [SerializeField] Button[] inventoryButtons;
    [SerializeField] Button closeButton;

    public void ChooseItemToDestroy()
    {
        for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
        {
            if (Inventory.instanceInventory.items[i].itemName == item.itemName)
            {
                Inventory.instanceInventory.items[i] = itemToAdd;
            }
        }
        for (int j = 0; j < shopButtons.Length; j++)
        {
            shopButtons[j].interactable = true;
            closeButton.interactable = true;
            inventoryButtons[j].interactable = false;
        }
    }
}
