using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopObject;

    public int gold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
