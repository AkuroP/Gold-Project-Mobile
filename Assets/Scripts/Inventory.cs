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
    public bool mysteryBoxOpened = false;
    public int mysteryBoxDangerousness = 0;

    //Bonus Heart
    public bool hasBonusHeart = false;

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

    public ShopItem GetItem(string itemName)
    {
        for (int i = 0; i < maxItemNumber; i++)
        {
            if (itemName == items[i].itemName)
            {
                return items[i];
            }
        }
        return null;
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

    public void RefreshCooldown()
    {
        for (int i = 0; i < maxItemNumber; i++)
        {
            if (items[i].itemCooldown > 0)
            {
                items[i].itemCooldown--;
                if (HasItem("Trap Protector") && items[GetItemIndex("Trap Protector")].itemCooldown == 0)
                {
                    GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_Highlight");
                    GameManager.instanceGM.sfxAudioSource.Play();
                }
            }
        }
    }

    public int GetItemIndex(string itemName)
    {
        for (int i = 0; i < maxItemNumber; i++)
        {
            if (itemName == items[i].itemName)
            {
                return i;
            }
        }
        return -1;
    }
}
