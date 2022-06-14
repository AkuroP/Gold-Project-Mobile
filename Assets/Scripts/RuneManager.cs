using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{

    public bool hasBuyDagger = true;
    public bool hasBuyHandgun = false;
    public bool hasBuyGrimoire = false;

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
    }
}
