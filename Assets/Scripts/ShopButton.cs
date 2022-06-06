using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{

    public ShopItem item;

    private Player player;

    public Text costText;

    private bool hasBeenClicked = false;

    [SerializeField] Button[] shopButtons;
    [SerializeField] Button[] inventoryButtons;
    [SerializeField] Button closeButton;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        if(hasBeenClicked)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void ChooseItem()
    {
        if (player.numEssence > item.itemCost)
        {
            if (item.goesInInventory == false)
            {
                switch (item.itemName)
                {
                    case "Heart Regeneration":
                        if (player.hp < player.maxHP)
                        {
                            player.hp++;
                            hasBeenClicked = true;
                        }
                        break;
                    case "Bonus Heart":
                        player.maxHP++;
                        hasBeenClicked = true;
                        break;
                    case "Mystery Box":
                        if (Inventory.instanceInventory.mysteryBoxInShop == false)
                        {
                            Inventory.instanceInventory.mysteryBoxInShop = true;
                            Inventory.instanceInventory.mysteryBoxDangerousness = 1;
                        }
                        else
                        {
                            OpenMysteryBox();
                            if (Inventory.instanceInventory.itemInInventory >= Inventory.instanceInventory.maxItemNumber)
                            {
                                for (int i = 0; i < shopButtons.Length; i++)
                                {
                                    inventoryButtons[i].GetComponent<ShopInventoryButton>().itemToAdd = item;
                                    shopButtons[i].interactable = false;
                                    closeButton.interactable = false;
                                    inventoryButtons[i].interactable = true;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
                                {
                                    if (Inventory.instanceInventory.items[i].itemName == "" && item.goesInInventory)
                                    {
                                        Inventory.instanceInventory.items[i] = item;
                                        Inventory.instanceInventory.itemInInventory++;
                                        this.GetComponent<Button>().interactable = false;
                                        break;
                                    }
                                }
                            }
                            Inventory.instanceInventory.mysteryBoxInShop = false;
                        }
                        hasBeenClicked = true;
                        break;
                }
                }
                else if (Inventory.instanceInventory.itemInInventory >= Inventory.instanceInventory.maxItemNumber)
                {
                    for(int i = 0; i < shopButtons.Length; i++)
                    {
                        inventoryButtons[i].GetComponent<ShopInventoryButton>().itemToAdd = item;
                        shopButtons[i].interactable = false;
                        closeButton.interactable = false;
                        inventoryButtons[i].interactable = true;
                        hasBeenClicked = true;
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
                            Inventory.instanceInventory.itemInInventory++;
                            this.GetComponent<Button>().interactable = false;
                            hasBeenClicked = true;
                            break;
                        }
                    }
                }
        }
    }

    void OpenMysteryBox()
    {
        bool hasFindItem = false;
        while (hasFindItem == false)
        {
            int random = Random.Range(0, GameObject.FindWithTag("ShopInGame").GetComponent<ShopInGame>().allItems.Count);
            if (GameObject.FindWithTag("ShopInGame").GetComponent<ShopInGame>().allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness)
            {
                hasFindItem = true;
                this.item = GameObject.FindWithTag("ShopInGame").GetComponent<ShopInGame>().allItems[random];
            }
        }
    }

}