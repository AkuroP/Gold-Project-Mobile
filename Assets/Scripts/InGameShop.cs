using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShop : MonoBehaviour
{
    //Health items
    private Item heartRegeneration = new Item("Heart Regeneration", 1, false);
    private Item bonusHeart = new Item("Bonus Heart", 2, false);
    private Item lifeRegeneration = new Item("Life Regeneration", 4, true);

    //Movement items
    private Item wornSpeedBoots = new Item("Worn Speed Boots", 1, true);
    private Item trapProtector = new Item("Trap Protector", 2, true);
    private Item speedBoots = new Item("Speed Boots", 3, true);

    //Counter items
    private Item counterRing = new Item("Counter Ring", 1, true);
    private Item poisonFog = new Item("Poison Fog", 2, true);
    private Item freezeTime = new Item("Freeze Time", 3, true);
    private Item revivor = new Item("Revivor", 4, true);
    private Item invicibility = new Item("Invicibility", 5, true);

    //Damage items
    private Item bossSlayer = new Item("Boss Slayer", 1, true);
    private Item powerGloves = new Item("Power Gloves", 2, true);
    private Item sideSlash = new Item("Side Slash", 3, true);

    private List<Item> allItems = new List<Item>();
    private List<Item> availableItems = new List<Item>();

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
        allItems.Add(invicibility);
        allItems.Add(bossSlayer);
        allItems.Add(powerGloves);
        allItems.Add(sideSlash);

        //Add items that can be seen at this level
        foreach (Item item in allItems)
        {
            if(item.itemDangerousness <= GameManager.instanceGM.actualDangerousness)
            {
                availableItems.Add(item);
                Debug.Log(item.itemName + ", " + item.itemDangerousness);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
