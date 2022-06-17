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
    [SerializeField] Button buyButton;
    [SerializeField] GameObject descriptionPanel;
    [SerializeField] Text descriptionText;

    private ShopInGame shopInGame;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        shopInGame = GameObject.FindWithTag("ShopInGame").GetComponent<ShopInGame>();
    }

    public void Update()
    {
        if(hasBeenClicked)
        {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void ShowDescription()
    {
        if(descriptionPanel.activeSelf == false)
        {
            descriptionPanel.SetActive(true);
            descriptionText.text = item.itemDescription;
        }
        else
        {
            descriptionText.text = item.itemDescription;
        }
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate{ChooseItem(item.itemName);});
    }

    public void ChooseItem(string name)
    {
        if (player.numEssence > item.itemCost)
        {
            if (item.goesInInventory == false)
            {
                switch (name)
                {
                    case "Heart Regeneration":
                        if (player.hp < player.maxHP)
                        {
                            player.hp++;
                            player.numEssence -= item.itemCost;
                            hasBeenClicked = true;
                            AchievementManager.instanceAM.UpdateItemsPurchased();
                        }
                        break;
                    case "Bonus Heart":
                        player.maxHP++;
                        player.hp++;
                        player.numEssence -= item.itemCost;
                        hasBeenClicked = true;
                        AchievementManager.instanceAM.UpdateItemsPurchased();
                        Inventory.instanceInventory.hasBonusHeart = true;
                        break;
                    case "Mystery Box":
                        if (Inventory.instanceInventory.mysteryBoxInShop == false && player.hp > 1)
                        {
                            Inventory.instanceInventory.mysteryBoxInShop = true;
                            Inventory.instanceInventory.mysteryBoxDangerousness = 0;
                            player.hp--;
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
                switch (name)
                {
                    case "Speed Boots":
                        player.mobility++;
                        break;
                    case "Worn Speed Boots":
                        player.mobility++;
                        break;
                }
                for (int i = 0; i < shopButtons.Length; i++)
                {
                    inventoryButtons[i].GetComponent<ShopInventoryButton>().itemToAdd = item;
                    shopButtons[i].interactable = false;
                    closeButton.interactable = false;
                    inventoryButtons[i].interactable = true;
                    hasBeenClicked = true;
                    AchievementManager.instanceAM.UpdateItemsPurchased();
                }
            }
            else
            {
                switch (name)
                {
                    case "Speed Boots":
                        player.mobility++;
                        break;
                }
                for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
                {
                    if (Inventory.instanceInventory.items[i].itemName == "" && item.goesInInventory)
                    {
                        player.numEssence -= item.itemCost;
                        Inventory.instanceInventory.items[i] = item;
                        Inventory.instanceInventory.itemInInventory++;
                        this.GetComponent<Button>().interactable = false;
                        hasBeenClicked = true;
                        AchievementManager.instanceAM.UpdateItemsPurchased();
                        break;
                    }
                }
            }
        }
        descriptionPanel.SetActive(false);
    }

    void OpenMysteryBox()
    {
        bool hasFindItem = false;
        while (hasFindItem == false)
        {
            int random = Random.Range(0, shopInGame.allItems.Count);
            if (Inventory.instanceInventory.mysteryBoxDangerousness == 5 && Inventory.instanceInventory.HasItem("Invincibility") && (!Inventory.instanceInventory.HasItem("Life Regeneration") || !Inventory.instanceInventory.HasItem("Revivor")) &&  shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness - 1)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 5 && Inventory.instanceInventory.HasItem("Invincibility") && Inventory.instanceInventory.HasItem("Life Regeneration") && Inventory.instanceInventory.HasItem("Revivor") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness - 2)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 4 && Inventory.instanceInventory.HasItem("Life Regeneration") && Inventory.instanceInventory.HasItem("Revivor") && Inventory.instanceInventory.HasItem("Invincibility") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness - 1)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 4 && Inventory.instanceInventory.HasItem("Life Regeneration") && Inventory.instanceInventory.HasItem("Revivor") && !Inventory.instanceInventory.HasItem("Invincibility") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness + 1)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 3 && Inventory.instanceInventory.HasItem("Speed Boots") && Inventory.instanceInventory.HasItem("Freeze TIme") && Inventory.instanceInventory.HasItem("Side Slash") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness + 1)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 2 && Inventory.instanceInventory.HasItem("Trap Protector") && Inventory.instanceInventory.HasItem("Poison Fog") && Inventory.instanceInventory.HasItem("Power Gloves") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness + 1)
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (Inventory.instanceInventory.mysteryBoxDangerousness == 1 && Inventory.instanceInventory.HasItem("Counter Ring") && Inventory.instanceInventory.HasItem("Worn Speed Boots") && Inventory.instanceInventory.HasItem("Boss Slayer") && shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness + 1 && shopInGame.allItems[random].itemName != "Bonus Heart" && shopInGame.allItems[random].itemName != "Heart Regeneration")
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
            else if (shopInGame.allItems[random].itemDangerousness == Inventory.instanceInventory.mysteryBoxDangerousness && shopInGame.allItems[random].itemName != "Bonus Heart" && shopInGame.allItems[random].itemName != "Heart Regeneration")
            {
                hasFindItem = true;
                this.item = shopInGame.allItems[random];
            }
        }
        Inventory.instanceInventory.mysteryBoxInShop = false;
        Inventory.instanceInventory.mysteryBoxOpened = true;
        AchievementManager.instanceAM.UpdateItemsPurchased();
    }
}