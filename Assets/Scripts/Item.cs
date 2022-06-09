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

    private bool hasBuyDagger = false;
    private bool hasBuyHandgun = false;
    private bool hasBuyGrimoire = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        UpdateWeapon(this.name);
        
    }

    public void BuyWeapon(string _weaponName)
    {
        if(GameManager.instanceGM.gold >= this.itemPrice)
        {
            if(_weaponName == "DAGGER")
            {
                if(!hasBuyDagger)
                {
                    Debug.Log(_weaponName + " BUYED");
                    GameManager.instanceGM.gold -= itemPrice;
                    player.weapon = new Weapon(WeaponType.DAGGER, 0, 1);
                    hasBuyDagger = true;
                }
                else
                {
                    UpgradeWeapon(WeaponType.DAGGER);
                }
            }
            else if(_weaponName == "HANDGUN")
            {
                if(!hasBuyHandgun)
                {
                    Debug.Log(_weaponName + " BUYED");
                    GameManager.instanceGM.gold -= itemPrice;
                    player.weapon = new Weapon(WeaponType.HANDGUN, 0, 1);
                    hasBuyHandgun = true;
                }
                else
                {
                    UpgradeWeapon(WeaponType.HANDGUN);
                }

                this.UpdateWeapon(this.name);
            }
            else if(_weaponName == "GRIMOIRE")
            {
                if(!hasBuyGrimoire)
                {
                    Debug.Log(_weaponName + " BUYED");
                    GameManager.instanceGM.gold -= itemPrice;
                    player.weapon = new Weapon(WeaponType.GRIMOIRE, 0, 1);
                    hasBuyGrimoire = true;
                }
                else
                {
                    UpgradeWeapon(WeaponType.GRIMOIRE);
                }

                this.UpdateWeapon(this.name);
            }
            else
            {
                Debug.Log("NO SUCH WEAPON FOUND");
            }

            this.UpdateWeapon(this.name);
        }
        else
        {
            Debug.Log("YOU ARE POOR :(");
        }
    }

    private void UpgradeWeapon(WeaponType _weaponType)
    {
        if(!isMaxed)
        {
            if(GameManager.instanceGM.gold >= (this.itemPrice * (player.weapon.weaponLevel + 2)))
            {
                Debug.Log(_weaponType.ToString() + " UPGRADED TO LVL " + (player.weapon.weaponLevel + 1));
                player.weapon = new Weapon(_weaponType, player.weapon.weaponLevel + 1, 1);
                GameManager.instanceGM.gold -= (this.itemPrice * (player.weapon.weaponLevel + 2));
            }
            else
            {
                Debug.Log("NOT ENOUGH GOLD TO UPGRADE " + _weaponType.ToString() + " TO LVL " + (player.weapon.weaponLevel + 1));
            }
        }
        else
        {
            Debug.Log(_weaponType.ToString() + " ALREADY MAXED UPGRADED");
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

    private void UpdateWeapon(string _weaponName)
    {
        //Debug.Log("UPDATE");
        weaponName.text = _weaponName;
        int localPrice = itemPrice;
        int localLevel = 0;
        switch(_weaponName)
        {
            case "DAGGER" :
                if(hasBuyDagger)
                {
                   localPrice = (this.itemPrice * (player.weapon.weaponLevel + 2));
                   localLevel = player.weapon.weaponLevel + 1;
                }
            break;

            case "HANDGUN" :
                if(hasBuyHandgun)
                {
                    localPrice = (this.itemPrice * (player.weapon.weaponLevel + 2));
                    localLevel = player.weapon.weaponLevel + 1;
                }
            break;

            case "GRIMOIRE" :
                if(hasBuyGrimoire)
                {
                    localPrice = (this.itemPrice * (player.weapon.weaponLevel + 2));
                    localLevel = player.weapon.weaponLevel + 1;
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
        }
    }
}
