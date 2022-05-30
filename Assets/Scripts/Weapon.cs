using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum weaponType
    {
        DAGGER,
        HANDGUN,
        GRIMOIRE
    }

    public weaponType typeOfWeapon;
    public int weaponLevel;

    public int range;
    public void applyEffect()
    {
        switch(this.typeOfWeapon)
        {
            case weaponType.DAGGER:
            Debug.Log("EFFECT DAGGER LVL 0");
            if(this.weaponLevel >= 1)
            {
                Debug.Log("EFFECT DAGGER LVL 1");
                if(this.weaponLevel >= 2)
                {
                    Debug.Log("EFFECT DAGGER LVL 2");
                }
            }
            break;
            case weaponType.HANDGUN:
            Debug.Log("EFFECT HANDGUN LVL 0");
            if(this.weaponLevel >= 1)
            {
                Debug.Log("EFFECT HANDGUN LVL 1");
                if(this.weaponLevel >= 2)
                {
                    Debug.Log("EFFECT HANDGUN LVL 2");
                }
            }
            break;
            case weaponType.GRIMOIRE:
            Debug.Log("EFFECT GRIMOIRE LVL 0");
            if(this.weaponLevel >= 1)
            {
                Debug.Log("EFFECT GRIMOIRE LVL 1");
                if(this.weaponLevel >= 2)
                {
                    Debug.Log("EFFECT GRIMOIRE LVL 2");
                }
            }
            break;
        }
    }
}
