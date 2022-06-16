using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text weaponName;
    public Text weaponUp;
    public Text weaponUpPrice;

    public int itemPrice;
    private Player player;
    [SerializeField] private bool isMaxed;
    [SerializeField] private int maxUpgrade = 2;

    // Start is called before the first frame update
    void Start()
    {
        //UpdateWeapon(this.name);
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
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    player.weapon = new Weapon(WeaponType.DAGGER, 0, 1);
                    RuneManager.instanceRM.hasBuyDagger = "true";
                    PlayerPrefs.SetString("hasDagger", RuneManager.instanceRM.hasBuyDagger);
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
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    // player.weapon = new Weapon(WeaponType.HANDGUN, 0, 1);
                    RuneManager.instanceRM.hasBuyHandgun = "true";
                    PlayerPrefs.SetString("hasHandgun", RuneManager.instanceRM.hasBuyHandgun);
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
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    //player.weapon = new Weapon(WeaponType.GRIMOIRE, 0, 1);
                    RuneManager.instanceRM.hasBuyGrimoire = "true";
                    PlayerPrefs.SetString("hasGrimoire", RuneManager.instanceRM.hasBuyGrimoire);
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
    }

    private void UpgradeWeapon(WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case WeaponType.DAGGER:
                if (RuneManager.instanceRM.daggerLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2)))
                {
                    RuneManager.instanceRM.daggerLevel++;
                    PlayerPrefs.SetInt("daggerLevel", RuneManager.instanceRM.daggerLevel);
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                }
                break;

            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)))
                {
                    RuneManager.instanceRM.handgunLevel++;
                    PlayerPrefs.SetInt("handgunLevel", RuneManager.instanceRM.handgunLevel);
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                }
                break;

            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.grimoireLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2)))
                {
                    RuneManager.instanceRM.grimoireLevel++;
                    PlayerPrefs.SetInt("grimoireLevel", RuneManager.instanceRM.grimoireLevel);
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                }
                break;
        }
    }

    /*public void BuyItem()
    {
        if(!isMaxed)
        {
            if(GameObject.Find("Canvas").GetComponent<Shop>().gold >= this.itemPrice)
            {
                Debug.Log("ITEM BUYED");
                GameObject.Find("Canvas").GetComponent<Shop>().gold -= this.itemPrice;
                //localLevel += 1;
                //player.weapon = new Weapon(localWT, localLevel, 1);
                data = new DataSave(2, WeaponType.HANDGUN);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("CAN'T BUY ITEM");
            }
        }
        else
        {
            Debug.Log("ITEM MAX UPGRADED");
        }
    }*/

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
                }
            break;

            case "HANDGUN" :
                if(RuneManager.instanceRM.hasBuyHandgun)
                {
                    localPrice = (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                }
            break;

            case "GRIMOIRE" :
                if(RuneManager.instanceRM.hasBuyGrimoire)
                {
                    localPrice = (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
                }
            break;
        }
        weaponUpPrice.text = localPrice.ToString();
        if(localLevel <= maxUpgrade)
        {
            weaponUp.text = "Level " + localLevel.ToString();
            this.isMaxed = false;
        }
        else
        {
            weaponUp.text = "MAXED";
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
