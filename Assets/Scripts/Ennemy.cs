using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected override void Attack()
    {
        
    }

    //check if the player is in range
    private bool IsPlayerInRange(WeaponType weapon, int range)
    {
        bool inRange = false;
        switch (weaponType)
        {
            case WeaponType.SWORD:
                /*if(check si joueur sur une case)
                {
                    IsPlayerInRange = true;
                }*/
                break;
            case WeaponType.GRIMOIRE:
                /*if(check si joueur sur une case)
                {
                    IsPlayerInRange = true;
                }*/
                break;
            case WeaponType.GUN:
                /*if(check si joueur sur une case)
                {
                    IsPlayerInRange = true;
                }*/
                break;
        }
        return inRange;
    }

    //function to take damage / die
    public override void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(this);
        }
    }
}
