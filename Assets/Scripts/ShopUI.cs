using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{

    private Player player;

    [SerializeField] private Text essenceText;
    [SerializeField] private Text healthText;

    [SerializeField] private GameObject runeButton1;
    [SerializeField] private GameObject runeButton2;

    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject runeDescriptionPanel;

    [SerializeField] private Image[] inventoryItems;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        switch (player.weapon.typeOfWeapon)
        {
            case WeaponType.DAGGER:
                if (GameManager.instanceGM.floor < 3 || RuneManager.instanceRM.daggerLevel < 1 || GameManager.instanceGM.firstUpgrade == true)
                {
                    runeButton1.SetActive(false);
                }
                if (GameManager.instanceGM.floor < 6 || RuneManager.instanceRM.daggerLevel < 2 || GameManager.instanceGM.firstUpgrade == false || GameManager.instanceGM.secondUpgrade == true)
                {
                    runeButton2.SetActive(false);
                }
                break;
            case WeaponType.HANDGUN:
                if (GameManager.instanceGM.floor < 3 || RuneManager.instanceRM.handgunLevel < 1 || GameManager.instanceGM.firstUpgrade == true)
                {
                    runeButton1.SetActive(false);
                }
                if (GameManager.instanceGM.floor < 6 || RuneManager.instanceRM.handgunLevel < 2 || GameManager.instanceGM.firstUpgrade == false || GameManager.instanceGM.secondUpgrade == true)
                {
                    runeButton2.SetActive(false);
                }
                break;
            case WeaponType.GRIMOIRE:
                if (GameManager.instanceGM.floor < 3 || RuneManager.instanceRM.grimoireLevel < 1 || GameManager.instanceGM.firstUpgrade == true)
                {
                    runeButton1.SetActive(false);
                }
                if (GameManager.instanceGM.floor < 6 || RuneManager.instanceRM.grimoireLevel < 2 || GameManager.instanceGM.firstUpgrade == false || GameManager.instanceGM.secondUpgrade == true)
                {
                    runeButton2.SetActive(false);
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        essenceText.text = player.numEssence.ToString();
        healthText.text = player.hp.ToString();

        for (int i = 0; i < Inventory.instanceInventory.maxItemNumber; i++)
        {
            inventoryItems[i].sprite = Inventory.instanceInventory.items[i].itemSprite;
            inventoryItems[i].gameObject.GetComponent<ShopInventoryButton>().item = Inventory.instanceInventory.items[i];
        }
    }

    public void CloseDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
    }

    public void CloseRuneDescriptionPanel()
    {
        runeDescriptionPanel.SetActive(false);
    }
}
