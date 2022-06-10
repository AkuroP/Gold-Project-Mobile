using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTP : Boss
{
    public List<AttackTileSettings> upDirectionATS1 = new List<AttackTileSettings>();
    public List<AttackTileSettings> upDirectionATS2 = new List<AttackTileSettings>();

    public bool isInvisible = false;
    public List<SunCreep> sunCreeps = new List<SunCreep>();

    public bool attackPhase = false;
    public bool doAttack1 = false;
    public bool doAttack2 = false;
    public int attack2CD = 1;

    void Update()
    {
        if(sunCreeps.Count == 0)
        {
            BossDeath();
        }

        if (myTurn)
        {
            StartTurn();

            hasPlay = true;
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 3;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 0;
        moveCDCurrent = 0;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Sun");

        AssignPattern();

        isInitialize = true;
    }

    public void AssignPattern()
    {
        //pattern attaque 1
        upDirectionATS1.Add(new AttackTileSettings(1, 0, 1));
        upDirectionATS1.Add(new AttackTileSettings(1, 0, -1));
        upDirectionATS1.Add(new AttackTileSettings(1, 1, 0));
        upDirectionATS1.Add(new AttackTileSettings(1, -1, 0));        
        
        upDirectionATS1.Add(new AttackTileSettings(1, 1, 1));
        upDirectionATS1.Add(new AttackTileSettings(1, 1, -1));
        upDirectionATS1.Add(new AttackTileSettings(1, -1, -1));
        upDirectionATS1.Add(new AttackTileSettings(1, -1, 1));

        //pattern attaque 2
        upDirectionATS2.Add(new AttackTileSettings(1, 0, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 0, 2));
        upDirectionATS2.Add(new AttackTileSettings(1, 0, -1));
        upDirectionATS2.Add(new AttackTileSettings(1, 0, -2));
        
        upDirectionATS2.Add(new AttackTileSettings(1, 1, 0));
        upDirectionATS2.Add(new AttackTileSettings(1, 2, 0));
        upDirectionATS2.Add(new AttackTileSettings(1, -1, 0));
        upDirectionATS2.Add(new AttackTileSettings(1, -2, 0));

        upDirectionATS2.Add(new AttackTileSettings(1, 1, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 1, -1));
        upDirectionATS2.Add(new AttackTileSettings(1, -1, -1));
        upDirectionATS2.Add(new AttackTileSettings(1, -1, 1));
        
        upDirectionATS2.Add(new AttackTileSettings(1, 2, 2));
        upDirectionATS2.Add(new AttackTileSettings(1, 2, -2));
        upDirectionATS2.Add(new AttackTileSettings(1, -2, -2));
        upDirectionATS2.Add(new AttackTileSettings(1, -2, 2));
    }

    public void StartTurn()
    {
        Debug.Log("sun turn");

        if(attackPhase)
        {
            if(doAttack1 && !doAttack2)
            {
                Debug.Log("att 1");
                StartAttack(upDirectionATS1);
                doAttack2 = true;
            }
            else if(doAttack1 && doAttack2)
            {
                Debug.Log("att 2");
                StartAttack(upDirectionATS2);
                doAttack1 = false;
                doAttack2 = false;
                attackPhase = false;
            }
        }
        else
        {
            if (!isInvisible)
            {
                TPOut();
            }
            else
            {
                List<Tile> possibleAttackSpot = FindAvailableAttackSpot(upDirectionATS1);

                TPIn(possibleAttackSpot[Random.Range(0, possibleAttackSpot.Count - 1)]);

                attackPhase = true;
                doAttack1 = true;
            }
        }


    }

    public void TPOut()
    {
        currentTile.entityOnTile = null;
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        isInvisible = true;
    }
    
    public void TPIn(Tile newTile)
    {

        while(newTile.isWall || newTile.isHole || newTile.entityOnTile != null || newTile == currentTile)
        {
            newTile = currentMap.ReturnRandomTile();
        }

        newTile.entityOnTile = this;
        currentTile = newTile;
        transform.position = currentTile.transform.position;
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        isInvisible = false;
    }
}

