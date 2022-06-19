using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopRuneButton : MonoBehaviour
{
    
    [SerializeField] private int upgradeCost;
    [SerializeField] private int upgradeLevel;

    [SerializeField] private GameObject itemDescriptionPanel;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button buyButton;

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

    public void ShowDescriptionPanel()
    {
        itemDescriptionPanel.SetActive(false);
        descriptionPanel.SetActive(true);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(delegate { UpgradeWeapon(); });
        switch (player.weapon.typeOfWeapon)
        {
            case WeaponType.DAGGER:
                if(player.weapon.weaponLevel == 0)
                {
                    descriptionText.text = "When an enemy is hit, he is bleeds and loses 1 hp at the end of its next turn.";
                }
                else
                {
                    descriptionText.text = "When an enemy is killed, you get a bonus turn.";
                }
                break;

            case WeaponType.HANDGUN:
                if (player.weapon.weaponLevel == 0)
                {
                    descriptionText.text = "Increases the handgun range by 1.";
                }
                else
                {
                    descriptionText.text = "Shots hit all enemies in the range.";
                }
                break;

            case WeaponType.GRIMOIRE:
                if (player.weapon.weaponLevel == 0)
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

    public void UpgradeWeapon()
    {
        descriptionPanel.SetActive(false);
        if(player.numEssence > upgradeCost)
        {
            player.numEssence -= upgradeCost;
            GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/money-sound-effect");
            GameManager.instanceGM.sfxAudioSource.Play();
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
