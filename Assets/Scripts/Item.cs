using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text weaponLvl;
    public Text weaponUpPrice;

    public int level;

    public WeaponType weaponType;

    public int itemPrice;
    private Player player;

    public GameObject descriptionPanel;
    public Button buyButton;
    public Text descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        CheckWeapon(this.name);
    }

    void Update()
    {
        switch (weaponType)
        {
            case WeaponType.DAGGER:
                if(level == 0 && RuneManager.instanceRM.hasBuyDagger == "true")
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if(level - 1 == RuneManager.instanceRM.daggerLevel)
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level == 1 && level == RuneManager.instanceRM.daggerLevel)
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level + 2 == RuneManager.instanceRM.daggerLevel)
                {
                    this.GetComponent<Button>().interactable = false;
                }
                else if (level - 2 == RuneManager.instanceRM.daggerLevel)
                {
                    this.GetComponent<Button>().interactable = false;
                    weaponUpPrice.gameObject.SetActive(false);
                }
                else
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                    weaponUpPrice.text = "Unlocked";
                }
                break;
            case WeaponType.HANDGUN:
                if (level == 0 && RuneManager.instanceRM.hasBuyHandgun == "true")
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level == 0 && RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                    this.GetComponent<Button>().interactable = true;
                }
                else if (level - 1 == RuneManager.instanceRM.handgunLevel && RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    this.GetComponent<Button>().interactable = false;
                    weaponUpPrice.gameObject.SetActive(false);
                }
                else if (level - 1  == RuneManager.instanceRM.handgunLevel && RuneManager.instanceRM.hasBuyHandgun == "true")
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level == 1 && level == RuneManager.instanceRM.handgunLevel)
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level - 2 == RuneManager.instanceRM.handgunLevel)
                {
                    this.GetComponent<Button>().interactable = false;
                    weaponUpPrice.gameObject.SetActive(false);
                }
                else
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                    weaponUpPrice.text = "Unlocked";
                }
                break;
            case WeaponType.GRIMOIRE:
                if (level == 0 && RuneManager.instanceRM.hasBuyGrimoire == "true")
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level == 0 && RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                    this.GetComponent<Button>().interactable = true;
                }
                else if (level - 1 == RuneManager.instanceRM.grimoireLevel && RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    this.GetComponent<Button>().interactable = false;
                    weaponUpPrice.gameObject.SetActive(false);
                }
                else if (level - 1 == RuneManager.instanceRM.grimoireLevel && RuneManager.instanceRM.hasBuyGrimoire == "true")
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level == 1 && level == RuneManager.instanceRM.grimoireLevel)
                {
                    weaponUpPrice.text = "Unlocked";
                    weaponUpPrice.gameObject.SetActive(true);
                }
                else if (level - 2 == RuneManager.instanceRM.grimoireLevel)
                {
                    this.GetComponent<Button>().interactable = false;
                    weaponUpPrice.gameObject.SetActive(false);
                }
                else
                {
                    this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    this.GetComponent<Button>().interactable = true;
                    weaponUpPrice.gameObject.SetActive(true);
                    weaponUpPrice.text = "Unlocked";
                }
                break;
        }
    }

    public void ShowDescription(string _weaponName)
    {
        descriptionPanel.SetActive(true);
        switch (_weaponName)
        {
            case "DAGGER":
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(delegate { BuyWeapon("DAGGER"); });
                buyButton.gameObject.SetActive(true);
                if (level == 0)
                {
                    descriptionText.text = "Melee attack with a 1 block range.";
                    if (RuneManager.instanceRM.hasBuyDagger == "true")
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else if(level == 1)
                {
                    descriptionText.text = "When an enemy is hit, he is bleeds and loses 1 hp at the end of its next turn.";
                    if (RuneManager.instanceRM.daggerLevel == 1)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    descriptionText.text = "When an enemy is killed, you get a bonus turn.";
                    if (RuneManager.instanceRM.daggerLevel == 2)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                break;

            case "HANDGUN":
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(delegate { BuyWeapon("HANDGUN"); });
                buyButton.gameObject.SetActive(true);
                if (level == 0)
                {
                    descriptionText.text = "Ranged attack with a 3 block maximum range. Only the first enemy in range takes damage.";
                    if (RuneManager.instanceRM.hasBuyHandgun == "true")
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else if (level == 1)
                {
                    descriptionText.text = "Increases the handgun range by 1.";
                    if (RuneManager.instanceRM.handgunLevel == 1)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    descriptionText.text = "Shots hit all enemies in the range.";
                    if (RuneManager.instanceRM.handgunLevel == 2)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                break;

            case "GRIMOIRE":
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(delegate { BuyWeapon("GRIMOIRE"); });
                buyButton.gameObject.SetActive(true);
                if (level == 0)
                {
                    descriptionText.text = "Melee attack with a 1 block range. The attack is an AOE that summons fire and hits all 3 blocks horizontally in the direction of the attack.";
                    if (RuneManager.instanceRM.hasBuyGrimoire == "true")
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else if (level == 1)
                {
                    descriptionText.text = "Spawns a flame on the square behind you.";
                    if (RuneManager.instanceRM.grimoireLevel == 1)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    descriptionText.text = "The squares where fire has spawned do (for 1 turn) damage to everything that walks on them (except you).";
                    if (RuneManager.instanceRM.grimoireLevel == 2)
                    {
                        buyButton.gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

    public void BuyWeapon(string _weaponName)
    {
        if(RuneManager.instanceRM.darkMatter >= this.itemPrice)
        {
            UI.instanceUI.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/money-sound-effect");
            UI.instanceUI.sfxAudioSource.Play();
            if (_weaponName == "DAGGER")
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
            descriptionPanel.SetActive(false);
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
                if (RuneManager.instanceRM.daggerLevel < 2 && RuneManager.instanceRM.darkMatter >= this.itemPrice)
                {
                    RuneManager.instanceRM.darkMatter -= this.itemPrice;
                    RuneManager.instanceRM.daggerLevel++;
                    PlayerPrefs.SetInt("daggerLevel", RuneManager.instanceRM.daggerLevel);
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel < 2 && RuneManager.instanceRM.darkMatter >= this.itemPrice)
                {
                    RuneManager.instanceRM.darkMatter -= this.itemPrice;
                    RuneManager.instanceRM.handgunLevel++;
                    PlayerPrefs.SetInt("handgunLevel", RuneManager.instanceRM.handgunLevel);
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.grimoireLevel < 2 && RuneManager.instanceRM.darkMatter >= this.itemPrice)
                {
                    RuneManager.instanceRM.darkMatter -= this.itemPrice;
                    RuneManager.instanceRM.grimoireLevel++;
                    PlayerPrefs.SetInt("grimoireLevel", RuneManager.instanceRM.grimoireLevel);
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
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.daggerLevel < 2)
                    {
                        weaponUpPrice.text = (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2)).ToString();

                    }
                    else
                    {
                        weaponUpPrice.text = " ";
                    }
                }
            break;

            case "HANDGUN":
                if(RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.handgunLevel < 2)
                    {
                        weaponUpPrice.text = (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)).ToString();

                    }
                    else
                    {
                        weaponUpPrice.text = " ";
                    }
                }
            break;

            case "GRIMOIRE" :
                if(RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    weaponUpPrice.text = this.itemPrice.ToString();
                }
                else
                {
                    if(RuneManager.instanceRM.grimoireLevel < 2)
                    {
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
