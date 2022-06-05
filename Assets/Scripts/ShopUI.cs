using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{

    private Player player;

    [SerializeField] private Text essenceText;

    [SerializeField] private Image[] inventoryItems;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        essenceText.text = player.numEssence.ToString();

        for(int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
        {
            inventoryItems[i].sprite = Inventory.instanceInventory.items[i].itemSprite;
            inventoryItems[i].gameObject.GetComponent<ShopInventoryButton>().item = Inventory.instanceInventory.items[i];
        }
    }
}
