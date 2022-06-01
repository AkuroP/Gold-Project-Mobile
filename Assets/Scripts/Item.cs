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
    [SerializeField] private bool isMaxed;
    [SerializeField] private int maxUpgrade = 2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        UpdateWeapon();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem()
    {
        if(!isMaxed)
        {
            if(GameObject.Find("Canvas").GetComponent<Shop>().gold >= this.itemPrice)
            {
                Debug.Log("ITEM BUYED");
                GameObject.Find("Canvas").GetComponent<Shop>().gold -= this.itemPrice;
                player.weapon.weaponLevel += 1;
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("CAN'T BUY ITEM");
            }
        }
        else
        {
            Debug.Log("ITEM MAX UPGRADED");
        }
    }

    private void UpdateWeapon()
    {
        weaponName.text = player.weapon.typeOfWeapon.ToString();
        weaponUpPrice.text = itemPrice.ToString() + " essences";
        if(player.weapon.weaponLevel < maxUpgrade)
        {
            weaponUp.text = "Level " + (player.weapon.weaponLevel + 1).ToString();
            this.isMaxed = false;
        }
        else
        {
            weaponUp.text = "MAXED";
            this.isMaxed = true;
        }
    }
}
