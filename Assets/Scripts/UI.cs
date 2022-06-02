using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text lifeText;
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;
    public GameObject attackButton;

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
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
        shopUI.transform.Find("CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(CloseShop);
        SwipeDetection.instanceSD.blockInputs = true;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        GameObject shop = GameObject.FindWithTag("ShopInGame");
        Destroy(shop);
        GameManager.instanceGM.NewMap();
        SwipeDetection.instanceSD.blockInputs = false;
    }
}
