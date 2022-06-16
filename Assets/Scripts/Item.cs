using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text weaponLvl;
    public Text weaponUpPrice;

    public int itemPrice;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        CheckWeapon(this.name);
    }

    public void BuyWeapon(string _weaponName)
    {
        if(RuneManager.instanceRM.darkMatter >= this.itemPrice)
        {
            if(_weaponName == "DAGGER")
            {
                if(RuneManager.instanceRM.hasBuyDagger == "false")
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                    player.weapon = new Weapon(WeaponType.DAGGER, 0, 1);
                    RuneManager.instanceRM.hasBuyDagger = "true";
                }
                else
                {
                    UpgradeWeapon(WeaponType.DAGGER);
                }
            }
            else if(_weaponName == "HANDGUN")
            {
                if(RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                   // player.weapon = new Weapon(WeaponType.HANDGUN, 0, 1);
                    RuneManager.instanceRM.hasBuyHandgun = "true";
                    PlayerPrefs.SetString("hasHandgun", RuneManager.instanceRM.hasBuyHandgun);
                    AchievementManager.instanceAM.UpdateHandgunPurchase();
                }
                else
                {
                    UpgradeWeapon(WeaponType.HANDGUN);
                }

                //this.UpdateWeapon(this.name);
            }
            else if(_weaponName == "GRIMOIRE")
            {
                if(RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                    //player.weapon = new Weapon(WeaponType.GRIMOIRE, 0, 1);
                    RuneManager.instanceRM.hasBuyGrimoire = "true";
                    PlayerPrefs.SetString("hasGrimoire", RuneManager.instanceRM.hasBuyGrimoire);
                    AchievementManager.instanceAM.UpdateGrimoirePurchase();
                }
                else
                {
                    UpgradeWeapon(WeaponType.GRIMOIRE);
                }

                //this.UpdateWeapon(this.name);
            }
            else
            {
                Debug.Log("NO SUCH WEAPON FOUND");
            }

            //this.UpdateWeapon(this.name);
        }
        else
        {
            Debug.Log("YOU ARE POOR :(");
        }
        if (RuneManager.instanceRM.hasBuyDagger == "true" && RuneManager.instanceRM.hasBuyGrimoire == "true" && RuneManager.instanceRM.hasBuyHandgun == "true")
        {
            AchievementManager.instanceAM.UpdateallWeaponPurchased();
        }

        CheckWeapon(this.name);
    }

    private void UpgradeWeapon(WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case WeaponType.DAGGER:
                if (RuneManager.instanceRM.daggerLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2)))
                {
                    RuneManager.instanceRM.daggerLevel++;
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)))
                {
                    RuneManager.instanceRM.handgunLevel++;
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.grimoireLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2)))
                {
                    RuneManager.instanceRM.grimoireLevel++;
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;
        }
    }

    private void CheckWeapon(string _weaponName)
    {
        switch(_weaponName)
        {
            case "DAGGER":
                if(RuneManager.instanceRM.hasBuyDagger == "false")
                {
                    weaponLvl.text = "BUY".ToString();
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.daggerLevel < 2)
                    {
                        weaponLvl.text = "LVL " + RuneManager.instanceRM.daggerLevel.ToString();
                        weaponUpPrice.text = (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2)).ToString();

                    }
                    else
                    {
                        weaponLvl.text = "MAXED";
                        weaponUpPrice.text = " ";
                    }
                }
            break;

            case "HANDGUN":
                if(RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    weaponLvl.text = "BUY".ToString();
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.handgunLevel < 2)
                    {
                        weaponLvl.text = "LVL " + RuneManager.instanceRM.handgunLevel.ToString();
                        weaponUpPrice.text = (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)).ToString();

                    }
                    else
                    {
                        weaponLvl.text = "MAXED";
                        weaponUpPrice.text = " ";
                    }
                }
            break;

            case "GRIMOIRE" :
                if(RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    weaponLvl.text = "BUY".ToString();
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.grimoireLevel < 2)
                    {
                        weaponLvl.text = "LVL " + RuneManager.instanceRM.grimoireLevel.ToString();
                        weaponUpPrice.text = (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2)).ToString();

                    }
                    else
                    {
                        weaponLvl.text = "MAXED";
                        weaponUpPrice.text = " ";
                    }
                }
            break;
        }
    }
}
