using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInGame : MonoBehaviour
{
    //Health items
    [Header("==== Health items ====")]
    [SerializeField] private ShopItem heartRegeneration = new ShopItem("Heart Regeneration", 1, false, "Gives back 1HP.");
    [SerializeField] public ShopItem bonusHeart = new ShopItem("Bonus Heart", 2, false, "Adds 1 HP max. (can only be purchased once)");
    [SerializeField] private ShopItem lifeRegeneration = new ShopItem("Life Regeneration", 4, true, "Gives back 1HP when you change floor.");

    //Movement items
    [Header("==== Movement items ====")]
    [SerializeField] private ShopItem wornSpeedBoots = new ShopItem("Worn Speed Boots", 1, true, "Give +1 movement each 3 turns");
    [SerializeField] private ShopItem trapProtector = new ShopItem("Trap Protector", 2, true, "Protect you from the first trap you set off. Recharges every 5 floors.");
    [SerializeField] private ShopItem speedBoots = new ShopItem("Speed Boots", 3, true, "Give +1 movement each turn.");

    //Counter items
    [Header("==== Counter items ====")]
    [SerializeField] private ShopItem counterRing = new ShopItem("Counter Ring", 1, true, "Your next attack after getting hit will be 2x more powerful.");
    [SerializeField] private ShopItem poisonFog = new ShopItem("Poison Fog", 2, true, "Fills the room with a poison fog that deal 1 HP dmamage to all monsters when you get hit.");
    [SerializeField] private ShopItem freezeTime = new ShopItem("Freeze Time", 3, true, "Freezes time for 2 turns when you get hit by a monster.");
    [SerializeField] private ShopItem revivor = new ShopItem("Revivor", 4, true, "When you die, revives with 1 HP.");
    [SerializeField] private ShopItem invincibility = new ShopItem("Invincibility", 5, true, "Makes you invincible for 3 turns when you get hit by a monster (you don’t take the hit from the monster).");

    //Damage items
    [Header("==== Damage items ====")]
    [SerializeField] private ShopItem bossSlayer = new ShopItem("Boss Slayer", 1, true, "You deal 1 more damage to the bosses.");
    [SerializeField] private ShopItem powerGloves = new ShopItem("Power Gloves", 2, true, "You deal 1 more HP damage to all enemies.");
    [SerializeField] private ShopItem sideSlash = new ShopItem("Side Slash", 3, true, "Your range is upgraded on your sides.");

    [Header("==== Others items ====")]
    [SerializeField] private ShopItem mysteryBox = new ShopItem("Mystery Box", 0, false, "When purchased, the mystery box is at a dangerousness level of 0. Each time you enter a shop, its dangerousness rises by 1. You can open the box whenever you want and get a random object of the dangerousness level of the box.");

    //List of items and shop
    public List<ShopItem> allItems = new List<ShopItem>();
    private List<ShopItem> availableItems = new List<ShopItem>();
    private List<ShopItem> Dplus1Items = new List<ShopItem>();
    private List<ShopItem> Dplus2Items = new List<ShopItem>();
    private ShopItem[] shop = new ShopItem[3];

    [SerializeField] private GameObject[] shopButtons;

    [SerializeField] private int Dmin;
    [SerializeField] private int Dmax;
    private int maxDangerousness = 5;
    private int minDangerousness = 1;
    private int d1Chance;
    private int d2Chance;

    // Start is called before the first frame update
    void Start()
    {
        //Add all item in the list
        allItems.Add(heartRegeneration);
        allItems.Add(bonusHeart);
        allItems.Add(lifeRegeneration);
        allItems.Add(wornSpeedBoots);
        allItems.Add(trapProtector);
        allItems.Add(speedBoots);
        allItems.Add(counterRing);
        allItems.Add(poisonFog);
        allItems.Add(freezeTime);
        allItems.Add(revivor);
        allItems.Add(invincibility);
        allItems.Add(bossSlayer);
        allItems.Add(powerGloves);
        allItems.Add(sideSlash);

        //Get all the GameObjects needed
        shopButtons[0] = UI.instanceUI.shopUI.transform.Find("Shop").transform.Find("Item1Button").gameObject;
        shopButtons[1] = UI.instanceUI.shopUI.transform.Find("Shop").transform.Find("Item2Button").gameObject;
        shopButtons[2] = UI.instanceUI.shopUI.transform.Find("Shop").transform.Find("Item3Button").gameObject;

        //Calculate Dmax and Dmin values
        Dmax = GameManager.instanceGM.actualDangerousness;
        Dmax = Mathf.Clamp(Dmax, minDangerousness, maxDangerousness);
        Dmin = Dmax - 2;
        Dmin = Mathf.Clamp(Dmin, minDangerousness, maxDangerousness - 2);

        //Remove unique items or in inventory items
        if (Inventory.instanceInventory.hasBonusHeart == true)
        {
            allItems.Remove(bonusHeart);
        }
        for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
        {
            foreach(ShopItem item in allItems.ToArray())
            {
                if (Inventory.instanceInventory.items[i].itemName == item.itemName)
                {
                    allItems.Remove(item);
                }
            }
        }

        //Add items that can be seen at your dangerousness level
        foreach (ShopItem item in allItems)
        {
            if (item.itemDangerousness <= Dmax && item.itemDangerousness >= Dmin)
            {
                availableItems.Add(item);
            }
            if (item.itemDangerousness == Dmax + 1 && Dmax <= maxDangerousness - 1)
            {
                Dplus1Items.Add(item);
            }
            if (item.itemDangerousness == Dmax + 2 && Dmax <= maxDangerousness - 2)
            {
                Dplus2Items.Add(item);
            }
        }
        if (Inventory.instanceInventory.mysteryBoxInShop == false && !Inventory.instanceInventory.mysteryBoxOpened && GameObject.FindWithTag("Player").GetComponent<Player>().hp > 1)
        {
            availableItems.Add(mysteryBox);
        }
        ChooseShopItems();
    }

    private void ChooseShopItems()
    {
        for (int i = 0; i < shopButtons.Length; i++)
        {
            switch (i)
            {
                case 0:
                    d1Chance = 4;
                    d2Chance = 1;
                    break;
                case 1:
                    d1Chance = 10;
                    d2Chance = 5;
                    break;
                case 2:
                    d1Chance = 20;
                    d2Chance = 10;
                    break;
            }
            
            if (Dmax <= maxDangerousness - 2)
            {
                int randomList = Random.Range(1, 101);
                //Debug.Log(randomList);
                if (randomList <= 100 && randomList > 100 - d2Chance && Dplus2Items.Count > 0)
                {
                    int randomItemIndex = Random.Range(0, Dplus2Items.Count);
                    shopButtons[i].GetComponent<ShopButton>().item = Dplus2Items[randomItemIndex];
                    shopButtons[i].GetComponent<Image>().sprite = Dplus2Items[randomItemIndex].itemSprite;
                    shopButtons[i].GetComponent<ShopButton>().costText.text = Dplus2Items[randomItemIndex].itemCost.ToString();
                    shop[i] = Dplus2Items[randomItemIndex];
                    Dplus2Items.RemoveAt(randomItemIndex);
                }
                else if (randomList <= 100 - d2Chance && randomList >= 100 - d2Chance - d1Chance && Dplus1Items.Count > 0)
                {
                    int randomItemIndex = Random.Range(0, Dplus1Items.Count);
                    shopButtons[i].GetComponent<ShopButton>().item = Dplus1Items[randomItemIndex];
                    shopButtons[i].GetComponent<Image>().sprite = Dplus1Items[randomItemIndex].itemSprite;
                    shopButtons[i].GetComponent<ShopButton>().costText.text = Dplus1Items[randomItemIndex].itemCost.ToString();
                    shop[i] = Dplus1Items[randomItemIndex];
                    Dplus1Items.RemoveAt(randomItemIndex);
                }
                else
                {
                    int randomItemIndex = Random.Range(0, availableItems.Count);
                    shopButtons[i].GetComponent<ShopButton>().item = availableItems[randomItemIndex];
                    shopButtons[i].GetComponent<Image>().sprite = availableItems[randomItemIndex].itemSprite;
                    shopButtons[i].GetComponent<ShopButton>().costText.text = availableItems[randomItemIndex].itemCost.ToString();
                    shop[i] = availableItems[randomItemIndex];
                    availableItems.RemoveAt(randomItemIndex);
                }
            }
            else if (Dmax == maxDangerousness - 1)
            {
                int randomList = Random.Range(0, 101);
                if (randomList >= 100 - d1Chance && Dplus1Items.Count > 0)
                {
                    int randomItemIndex = Random.Range(0, Dplus1Items.Count);
                    shopButtons[i].GetComponent<ShopButton>().item = Dplus1Items[randomItemIndex];
                    shopButtons[i].GetComponent<Image>().sprite = Dplus1Items[randomItemIndex].itemSprite;
                    shopButtons[i].GetComponent<ShopButton>().costText.text = Dplus1Items[randomItemIndex].itemCost.ToString();
                    shop[i] = Dplus1Items[randomItemIndex];
                    Dplus1Items.RemoveAt(randomItemIndex);
                }
                else
                {
                    int randomItemIndex = Random.Range(0, availableItems.Count);
                    shopButtons[i].GetComponent<ShopButton>().item = availableItems[randomItemIndex];
                    shopButtons[i].GetComponent<Image>().sprite = availableItems[randomItemIndex].itemSprite;
                    shopButtons[i].GetComponent<ShopButton>().costText.text = availableItems[randomItemIndex].itemCost.ToString();
                    shop[i] = availableItems[randomItemIndex];
                    availableItems.RemoveAt(randomItemIndex);
                }
            }
            else
            {
                int randomItemIndex = Random.Range(0, availableItems.Count);
                shopButtons[i].GetComponent<ShopButton>().item = availableItems[randomItemIndex];
                shopButtons[i].GetComponent<Image>().sprite = availableItems[randomItemIndex].itemSprite;
                shopButtons[i].GetComponent<ShopButton>().costText.text = availableItems[randomItemIndex].itemCost.ToString();
                shop[i] = availableItems[randomItemIndex];
                availableItems.RemoveAt(randomItemIndex);
            }

            if(shopButtons[i].GetComponent<ShopButton>().item.itemName == "Mystery Box")
            {
                shopButtons[i].GetComponent<ShopButton>().costText.text = "1HP";
            }
        }
        if (Inventory.instanceInventory.mysteryBoxInShop == true)
        {
            mysteryBox.itemDangerousness = Inventory.instanceInventory.mysteryBoxDangerousness;
            shopButtons[0].GetComponent<ShopButton>().item = mysteryBox;
            shopButtons[0].GetComponent<Image>().sprite = mysteryBox.itemSprite;
            shopButtons[0].GetComponent<ShopButton>().costText.text = Inventory.instanceInventory.mysteryBoxDangerousness.ToString();

        }
        /*foreach (ShopItem item in shop)
        {
            Debug.Log("item du shop : " + item.itemName + ", " + item.itemDangerousness);
        }*/
    }

}
