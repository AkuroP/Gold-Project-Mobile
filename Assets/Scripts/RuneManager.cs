using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{

    public string hasBuyDagger = "true";
    public string hasBuyHandgun = "false";
    public string hasBuyGrimoire = "false";

    public int daggerLevel;
    public int handgunLevel;
    public int grimoireLevel;

    public int darkMatter;

    public WeaponType currentWeapon;

    public static RuneManager instanceRM;

    void Awake()
    {
        if (instanceRM == null)
        {
            instanceRM = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanceRM != this)
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetString("hasDagger") != "")
        {
            hasBuyDagger = PlayerPrefs.GetString("hasDagger");
        }
        if (PlayerPrefs.GetString("hasHandgun") != "")
        {
            hasBuyHandgun = PlayerPrefs.GetString("hasHandgun");
        }
        if (PlayerPrefs.GetString("hasGrimoire") != "")
        {
            hasBuyGrimoire = PlayerPrefs.GetString("hasGrimoire");
        }

        daggerLevel = PlayerPrefs.GetInt("daggerLevel");
        handgunLevel = PlayerPrefs.GetInt("handgunLevel");
        grimoireLevel = PlayerPrefs.GetInt("grimoireLevel");

        if(PlayerPrefs.GetInt("darkMatter") != 0)
        {
            darkMatter = PlayerPrefs.GetInt("darkMatter");
        }  
    }
}
