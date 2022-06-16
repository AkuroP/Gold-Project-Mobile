using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRuneButton : MonoBehaviour
{
    
    [SerializeField] private int upgradeCost;
    [SerializeField] private int upgradeLevel;

    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        switch (player.weapon.typeOfWeapon)
        {
            case WeaponType.DAGGER:
                if (upgradeLevel == 1)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_0");
                }
                if (upgradeLevel == 2)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_1");
                }
                break;

            case WeaponType.HANDGUN:
                if (upgradeLevel == 1)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_4");
                }
                if (upgradeLevel == 2)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_5");
                }
                break;

            case WeaponType.GRIMOIRE:
                if (upgradeLevel == 1)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_2");
                }
                if (upgradeLevel == 2)
                {
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_3");
                }
                break;
        }

    }

    public void UpgradeWeapon()
    {
        if(player.numEssence > upgradeCost)
        {
            player.numEssence -= upgradeCost;
            switch (player.weapon.typeOfWeapon)
            {
                case WeaponType.DAGGER:
                    player.weapon = new Weapon(WeaponType.DAGGER, upgradeLevel, 1);
                    break;

                case WeaponType.HANDGUN:
                    player.weapon = new Weapon(WeaponType.HANDGUN, upgradeLevel, 1);
                    break;

                case WeaponType.GRIMOIRE:
                    player.weapon = new Weapon(WeaponType.GRIMOIRE, upgradeLevel, 1);
                    break;
            }
            if(!GameManager.instanceGM.firstUpgrade)
            {
                GameManager.instanceGM.firstUpgrade = true;
            }
            else
            {
                GameManager.instanceGM.secondUpgrade = true;
            }
            Destroy(this.gameObject);
        }

    }
}
