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

    //Main menu
    public GameObject optionsMenu;

    //Shop prefabs
    [SerializeField] GameObject shopPrefab;
    [SerializeField] GameObject shopUIPrefab;

    //HUD UI
    [SerializeField] private Text essencesText;
    [SerializeField] private Text lifeText;
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;
    [SerializeField] private Text floorText;
    [SerializeField] private Text roomText;
    public GameObject attackButton;

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
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        essencesText.text = player.numEssence.ToString();
        lifeText.text = player.hp.ToString();
        slot1.sprite = Inventory.instanceInventory.items[0].itemSprite;
        slot2.sprite = Inventory.instanceInventory.items[1].itemSprite;
        slot3.sprite = Inventory.instanceInventory.items[2].itemSprite;
        floorText.text = GameManager.instanceGM.floor.ToString();
        roomText.text = GameManager.instanceGM.room.ToString();
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        GameObject shop = GameObject.FindWithTag("ShopInGame");
        Destroy(shop);
        Destroy(shopUI);
        GameManager.instanceGM.NewMap();
        SwipeDetection.instanceSD.blockInputs = false;
    }

    public void Fade()
    {
        Instantiate(fadePrefab);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }
}

