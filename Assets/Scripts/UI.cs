using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public GameObject canvas;
    public GameObject shopUI;
    [SerializeField] GameObject shopPrefab;
    [SerializeField] GameObject shopUIPrefab;

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

    public void OpenShop()
    {
        shopUI.SetActive(true);
        shopUI.transform.Find("CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(CloseShop);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        GameManager.instanceGM.score++;
        GameManager.instanceGM.room++;
        GameObject shop = GameObject.FindWithTag("ShopInGame");
        Destroy(shop);
    }
}
