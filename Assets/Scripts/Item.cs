using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text weaponLvl;
    public Text weaponUpPrice;

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
                if(RuneManager.instanceRM.daggerLevel == 0 && RuneManager.instanceRM.hasBuyDagger == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_0");
                }
                else if (RuneManager.instanceRM.daggerLevel == 1 && RuneManager.instanceRM.hasBuyDagger == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_1");
                }
                if (RuneManager.instanceRM.daggerLevel == 2 && RuneManager.instanceRM.hasBuyDagger == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_1");
                    gameObject.GetComponent<Button>().interactable = false;
                }
                break;
            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel == 0 && RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/gun");
                }
                else if (RuneManager.instanceRM.handgunLevel == 0 && RuneManager.instanceRM.hasBuyHandgun == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_4");
                }
                else if (RuneManager.instanceRM.handgunLevel == 1 && RuneManager.instanceRM.hasBuyHandgun == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_5");
                }
                if (RuneManager.instanceRM.handgunLevel == 2 && RuneManager.instanceRM.hasBuyHandgun == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_5");
                    gameObject.GetComponent<Button>().interactable = false;
                }
                break;
            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.handgunLevel == 0 && RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/grimoire_hud");
                }
                else if (RuneManager.instanceRM.grimoireLevel == 0 && RuneManager.instanceRM.hasBuyGrimoire == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_2");
                }
                else if (RuneManager.instanceRM.grimoireLevel == 1 && RuneManager.instanceRM.hasBuyGrimoire == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_3");
                }
                if (RuneManager.instanceRM.grimoireLevel == 2 && RuneManager.instanceRM.hasBuyGrimoire == "true")
                {
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runes_3");
                    gameObject.GetComponent<Button>().interactable = false;
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
                if (RuneManager.instanceRM.daggerLevel == 0)
                {
                    descriptionText.text = "When an enemy is hit, he is bleeds and loses 1 hp at the end of its next turn.";
                }
                else
                {
                    descriptionText.text = "When an enemy is killed, you get a bonus turn.";
                }
                break;

            case "HANDGUN":
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(delegate { BuyWeapon("HANDGUN"); });
                if (RuneManager.instanceRM.hasBuyHandgun == "false")
                {
                    descriptionText.text = "Ranged attack with a 3 block maximum range. Only the first enemy in range takes damage.";
                }
                else if (RuneManager.instanceRM.handgunLevel == 0)
                {
                    descriptionText.text = "Increases the handgun range by 1.";
                }
                else
                {
                    descriptionText.text = "Shots hit all enemies in the range.";
                }
                break;

            case "GRIMOIRE":
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(delegate { BuyWeapon("GRIMOIRE"); });
                if (RuneManager.instanceRM.hasBuyGrimoire == "false")
                {
                    descriptionText.text = "Melee attack with a 1 block range. The attack is an AOE that summons fire and hits all 3 blocks horizontally in the direction of the attack.";
                }
                else if (RuneManager.instanceRM.handgunLevel == 0)
                {
                    descriptionText.text = "Spawns a flame on the square behind you.";
                }
                else
                {
                    descriptionText.text = "The squares where fire has spawned do (for 1 turn) damage to everything that walks on them (except you).";
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

        CheckWeapon(this.name);
    }

    private void UpgradeWeapon(WeaponType _weaponType)
    {
        switch (_weaponType)
        {
            case WeaponType.DAGGER:
                if (RuneManager.instanceRM.daggerLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2)))
                {
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.daggerLevel + 2));
                    RuneManager.instanceRM.daggerLevel++;
                    PlayerPrefs.SetInt("daggerLevel", RuneManager.instanceRM.daggerLevel);
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.HANDGUN:
                if (RuneManager.instanceRM.handgunLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2)))
                {
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.handgunLevel + 2));
                    RuneManager.instanceRM.handgunLevel++;
                    PlayerPrefs.SetInt("handgunLevel", RuneManager.instanceRM.handgunLevel);
                    PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
                    AchievementManager.instanceAM.UpdateRunesPurchased();
                }
                break;

            case WeaponType.GRIMOIRE:
                if (RuneManager.instanceRM.grimoireLevel < 2 && RuneManager.instanceRM.darkMatter >= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2)))
                {
                    RuneManager.instanceRM.darkMatter -= (this.itemPrice * (RuneManager.instanceRM.grimoireLevel + 2));
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
