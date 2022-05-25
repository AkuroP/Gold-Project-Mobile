using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemPrice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {
        if(GameObject.Find("Canvas").GetComponent<Shop>().gold >= this.itemPrice)
        {
            Debug.Log("ITEM BUYED");
            GameObject.Find("Canvas").GetComponent<Shop>().gold -= this.itemPrice;
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("CAN'T BUY ITEM");
        }
    }
}
