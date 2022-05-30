using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text weaponName;
    public Text weaponUp;
    public Text weaponUpPrice;

    public int itemPrice;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        weaponName.text = player.weapon.typeOfWeapon.ToString();
        weaponUp.text = "Level " + (player.weapon.weaponLevel + 1).ToString();
        weaponUpPrice.text = itemPrice.ToString() + " essences";
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
