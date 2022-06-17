using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public Text darkMatterText;

    public GameObject shopObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        darkMatterText.text = RuneManager.instanceRM.darkMatter.ToString();
    }

    public void OpenCloseShop(bool _openClose)
    {
        if(_openClose)
        {
            shopObject.SetActive(true);
        }
        else
        {
            shopObject.SetActive(false);
        }
    }

}
