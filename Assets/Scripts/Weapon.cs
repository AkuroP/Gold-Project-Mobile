using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    DAGGER,
    HANDGUN,
    GRIMOIRE
}

public enum WeaponEffect 
{
    NONE, 
    BURN, 
    FREEZE 
}

[System.Serializable]
public class Weapon
{
    public WeaponType typeOfWeapon;
    public int weaponLevel;
    public int weaponDamage;

    public List<AttackTileSettings> upDirectionATS;

    public Weapon(WeaponType _weaponType)
    {
        weaponDamage = 1;

        upDirectionATS = new List<AttackTileSettings>();

        switch (_weaponType)
        {
            case WeaponType.DAGGER:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                break;
            case WeaponType.HANDGUN:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(2, 0, 2));
                break;
            case WeaponType.GRIMOIRE:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                break;
            default:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                break;
        }
    }

    public List<AttackTileSettings> ConvertPattern(List<AttackTileSettings> upDirectionATS, Direction entityDirection)
    {
        List<AttackTileSettings> newATS = new List<AttackTileSettings>();

        switch (entityDirection)
        {
            case Direction.UP:
                newATS = upDirectionATS;
                break;

            case Direction.BOTTOM:
                foreach(AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int newOffsetX = -1 * ats.offsetX;
                    int newOffsetY = -1 * ats.offsetY;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.LEFT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = ats.offsetY;
                    int newOffsetY = -1 * temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.RIGHT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = -1 * ats.offsetY;
                    int newOffsetY = temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;
        }

        return newATS;
    }


    public void ApplyEffect()
    {
        switch(this.typeOfWeapon)
        {
            case WeaponType.DAGGER:
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
            case WeaponType.HANDGUN:
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
            case WeaponType.GRIMOIRE:
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


[System.Serializable]
public struct AttackTileSettings
{
    public int order, offsetX, offsetY;

    public AttackTileSettings(int _order, int _offsetX, int _offsetY)
    {
        this.order = _order;
        this.offsetX = _offsetX;
        this.offsetY = _offsetY;
    }
}
