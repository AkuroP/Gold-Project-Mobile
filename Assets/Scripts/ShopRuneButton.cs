using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRuneButton : MonoBehaviour
{
    
    [SerializeField] private int upgradeCost;
    [SerializeField] private int upgradeLevel;

    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
            Destroy(this.gameObject);
        }
        if(upgradeLevel == 1)
        {
            GameManager.instanceGM.firstUpgrade = true;
        }
        if (upgradeLevel == 2)
        {
            GameManager.instanceGM.secondUpgrade = true;
        }
    }
}
