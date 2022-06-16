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
                if(!RuneManager.instanceRM.hasBuyDagger)
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                    player.weapon = new Weapon(WeaponType.DAGGER, 0, 1);
                    RuneManager.instanceRM.hasBuyDagger = true;
                }
                else
                {
                    UpgradeWeapon(WeaponType.DAGGER);
                }
            }
            else if(_weaponName == "HANDGUN")
            {
                if(RuneManager.instanceRM.hasBuyHandgun == false)
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                   // player.weapon = new Weapon(WeaponType.HANDGUN, 0, 1);
                    RuneManager.instanceRM.hasBuyHandgun = true;
                }
                else
                {
                    UpgradeWeapon(WeaponType.HANDGUN);
                }

                //this.UpdateWeapon(this.name);
            }
            else if(_weaponName == "GRIMOIRE")
            {
                if(RuneManager.instanceRM.hasBuyGrimoire == false)
                {
                    Debug.Log(_weaponName + " BUYED");
                    RuneManager.instanceRM.darkMatter -= itemPrice;
                    //player.weapon = new Weapon(WeaponType.GRIMOIRE, 0, 1);
                    RuneManager.instanceRM.hasBuyGrimoire = true;
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
        if (RuneManager.instanceRM.hasBuyDagger && RuneManager.instanceRM.hasBuyGrimoire && RuneManager.instanceRM.hasBuyHandgun)
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
                }
                break;

            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)))
                {
                    RuneManager.instanceRM.handgunLevel++;
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                }
                break;

            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.grimoireLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2)))
                {
                    RuneManager.instanceRM.grimoireLevel++;
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
                }
                break;
        }
    }

    private void CheckWeapon(string _weaponName)
    {
        switch(_weaponName)
        {
            case "DAGGER":
                if(!RuneManager.instanceRM.hasBuyDagger)
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
                if(!RuneManager.instanceRM.hasBuyHandgun)
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
                if(!RuneManager.instanceRM.hasBuyGrimoire)
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

    /*private void UpdateWeapon(string _weaponName)
    {
        //Debug.Log("UPDATE");
        weaponName.text = _weaponName;
        int localPrice = itemPrice;
        int localLevel = 0;
        switch(_weaponName)
        {
            case "DAGGER" :
                if(RuneManager.instanceRM.hasBuyDagger)
                {
                   localPrice = (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2));
                   localLevel = 
                   weaponLvl.text = "LVL " + localLevel.ToString();
                }
                else
                {
                    weaponLvl.text = "BUY";
                }
            break;

            case "HANDGUN" :
                if(RuneManager.instanceRM.hasBuyHandgun)
                {
                    localPrice = (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                }
                else
                {
                    weaponLvl.text = "BUY";
                }
            break;

            case "GRIMOIRE" :
                if(RuneManager.instanceRM.hasBuyGrimoire)
                {
                    localPrice = (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
                }
                else
                {
                    weaponLvl.text = "BUY";
                }
            break;
        }
        weaponUpPrice.text = localPrice.ToString();

        
        if(localLevel <= maxUpgrade)
        {
            weaponLvl.text = "Level " + localLevel.ToString();
            this.isMaxed = false;
        }
        else
        {
            weaponLvl.text = "MAXED";
            this.isMaxed = true;
            switch (_weaponName)
            {
                case "DAGGER":
                    AchievementManager.instanceAM.UpdateAllDaggerRunesPurchased();
                    break;
                case "HANDGUN":
                    AchievementManager.instanceAM.UpdateAllHandgunRunesPurchased();
                    break;
                case "GRIMOIRE":
                    AchievementManager.instanceAM.UpdateAllGrimoireRunesPurchased();
                    break;
            }
        }
    }*/
}
