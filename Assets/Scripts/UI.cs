using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    public GameObject canvas;
    public GameObject shopUI;
    private Player player;

    //Shop prefabs
    [SerializeField] GameObject shopPrefab;
    [SerializeField] GameObject shopUIPrefab;

    //HUD UI
    [SerializeField] private Text essencesText;
    [SerializeField] private Text darkMatterText;
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;
    public Image activeSlot1;
    public Image activeSlot2;
    public Image activeSlot3;
    [SerializeField] private Text floorText;
    [SerializeField] private Text roomText;
    public GameObject attackButton;
    public GameObject optionButton;
    public GameObject closeOptionButton;
    public GameObject optionMenu;
    public GameObject chooseWeaponMenu;
    public Button HandgunButton;
    public Button GrimoireButton;
    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;
    [SerializeField] private Image rune1;
    [SerializeField] private Image rune2;
    [SerializeField] private GameObject hubShop;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject tutorial;
    public Button lastTutoImage;
    public AudioSource sfxAudioSource;

    //Fade
    public GameObject fadePrefab;

    public static UI instanceUI;

    private void Awake()
    {
        if (instanceUI != null)
        {
            Destroy(instanceUI);
        }
        instanceUI = this;

        canvas = GameObject.FindWithTag("Canvas");
    }

    private void Start()
    {
        if(GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        sfxAudioSource = Camera.main.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(player != null)
        {
            essencesText.text = player.numEssence.ToString();
            if(Inventory.instanceInventory.items[0].itemName == "")
            {
                slot1.sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            }
            else
            {
                slot1.sprite = Inventory.instanceInventory.items[0].itemHudSprite1;
            }
            if(Inventory.instanceInventory.items[1].itemName == "")
            {
                slot2.sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            }
            else
            {
                slot2.sprite = Inventory.instanceInventory.items[1].itemHudSprite2;
            }
            if (Inventory.instanceInventory.items[2].itemName == "")
            {
                slot3.sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            }
            else
            {
                slot3.sprite = Inventory.instanceInventory.items[2].itemHudSprite3;
            }
            darkMatterText.text = RuneManager.instanceRM.darkMatter.ToString();
            floorText.text = GameManager.instanceGM.floor.ToString();
            roomText.text = GameManager.instanceGM.room.ToString();
            switch(player.hp)
            {
                case 1 :
                    heart1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart1");
                    heart2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/emptyheart2");
                    heart3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/emptyheart3");
                    break;
                case 2:
                    heart1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart1");
                    heart2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart2");
                    heart3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/emptyheart3");
                    break;
                case 3:
                    heart1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart1");
                    heart2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart2");
                    heart3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart3");
                    break;
                case 4:
                    heart1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/goldheart1");
                    heart2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart2");
                    heart3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/fullheart3");
                    break;
                default:
                    break;
            }
            if (GameManager.instanceGM.firstUpgrade)
            {
                switch (player.weapon.typeOfWeapon)
                {
                    case WeaponType.DAGGER:
                        rune1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runedagger1");
                        break;

                    case WeaponType.HANDGUN:
                        rune1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runegun1");
                        break;

                    case WeaponType.GRIMOIRE:
                        rune1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runegrimoire1");
                        break;
                }
                if (GameManager.instanceGM.secondUpgrade)
                {
                    switch (player.weapon.typeOfWeapon)
                    {
                        case WeaponType.DAGGER:
                            rune2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runedagger2");
                            break;

                        case WeaponType.HANDGUN:
                            rune2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runegun2");
                            break;

                        case WeaponType.GRIMOIRE:
                            rune2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/runegrimoire2");
                            break;
                    }
                }
            }
            else
            {
                rune1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
                rune2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            }
        }

        if(HandgunButton != null && RuneManager.instanceRM.hasBuyHandgun == "true")
        {
            HandgunButton.interactable = true;
        }
        else if (HandgunButton != null && RuneManager.instanceRM.hasBuyHandgun == "false")
        {
            HandgunButton.interactable = false;
        }

        if (GrimoireButton != null && RuneManager.instanceRM.hasBuyGrimoire == "true")
        {
            GrimoireButton.interactable = true;
        }
        else if (GrimoireButton != null && RuneManager.instanceRM.hasBuyGrimoire == "false")
        {
            GrimoireButton.interactable = false;
        }
    }

    public void OpenMainMenuOptions()
    {
        optionMenu.SetActive(true);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonmenu");
        sfxAudioSource.Play();
    }

    public void CloseMainMenuOptions()
    {
        optionMenu.SetActive(false);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonreturn");
        sfxAudioSource.Play();
    }

    public void CloseShop()
    {
        
        //GameManager.instanceGM.NewMap();
        /*Tile playerLastTile = new Tile();
        switch(player.direction)
        {
            case Direction.UP :
                playerLastTile = GameManager.instanceGM.currentMap.FindBottomTile(player.currentTile);
            break;

            case Direction.RIGHT :
                playerLastTile = GameManager.instanceGM.currentMap.FindLeftTile(player.currentTile);
            break;

            case Direction.LEFT :
                playerLastTile = GameManager.instanceGM.currentMap.FindRightTile(player.currentTile);
            break;

            case Direction.BOTTOM :
                playerLastTile = GameManager.instanceGM.currentMap.FindTopTile(player.currentTile);
            break;

            default :
                playerLastTile = GameManager.instanceGM.currentMap.FindBottomTile(player.currentTile);
            break;
        }*/
        player.isInShop = false;
        SwipeDetection.instanceSD.isInShop = false;
        //player.Move(playerLastTile);
        shopUI.SetActive(false);
        GameObject shop = GameObject.FindWithTag("ShopInGame");
        Destroy(shop);
        Destroy(shopUI);
        //map generation
        GameManager.instanceGM.NewMap();
    }

    public void Fade()
    {
        Instantiate(fadePrefab);
    }

    public void StartGame(string weaponType)
    {
        switch(weaponType)
        {
            case "Dagger":
                RuneManager.instanceRM.currentWeapon = WeaponType.DAGGER;
                sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Drawnknife");
                sfxAudioSource.Play();
                break;
            case "Handgun":
                RuneManager.instanceRM.currentWeapon = WeaponType.HANDGUN;
                sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/reloadpistol");
                sfxAudioSource.Play();
                break;
            case "Grimoire":
                RuneManager.instanceRM.currentWeapon = WeaponType.GRIMOIRE;
                sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/openmagicbook");
                sfxAudioSource.Play();
                break;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenChooseWeapon()
    {
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonmenu");
        sfxAudioSource.Play();
        if (RuneManager.instanceRM.hasBuyHandgun == "true" || RuneManager.instanceRM.hasBuyGrimoire == "true")
        {
            chooseWeaponMenu.SetActive(true);
        }
        else if (AchievementManager.instanceAM.stepsNumber <= 0)
        {
            tutorial.SetActive(true);
            lastTutoImage.onClick.RemoveAllListeners();
            lastTutoImage.onClick.AddListener(delegate { lastTutoImage.GetComponent<TutorialButton>().FinalImageAndLauchGame(); });
        }
        else
        {
            RuneManager.instanceRM.currentWeapon = WeaponType.DAGGER;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void CloseChooseWeapon()
    {
        chooseWeaponMenu.SetActive(false);
        optionMenu.SetActive(false);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonreturn");
        sfxAudioSource.Play();
    }

    public void CloseHubShop()
    {
        descriptionPanel.SetActive(false);
        hubShop.SetActive(false);
        optionMenu.SetActive(false);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonreturn");
        sfxAudioSource.Play();
    }

    public void CloseRuneDescription()
    {
        descriptionPanel.SetActive(false);
        optionMenu.SetActive(false);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonreturn");
        sfxAudioSource.Play();
    }

    public void OpenOptions()
    {
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonmenu");
        sfxAudioSource.Play();
        GameManager.instanceGM.PauseResume(true);
    }

    public void CloseTitleScreen()
    {
        titleScreen.SetActive(false);
        optionMenu.SetActive(false);
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonmenu");
        sfxAudioSource.Play();
    }

    public void GoToMainMenu()
    {
        sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/buttonreturn");
        sfxAudioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

