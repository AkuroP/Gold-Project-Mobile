using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTP : Boss
{
    public List<AttackTileSettings> upDirectionATS1 = new List<AttackTileSettings>();
    public List<AttackTileSettings> upDirectionATS2 = new List<AttackTileSettings>();

    public List<Tile> pathToTarget = new List<Tile>();

    public bool isInvisible = false;
    public List<SunCreep> sunCreeps = new List<SunCreep>();

    public bool attackPhase = false;
    public bool doAttack1 = false;
    public bool doAttack2 = false;
    public int attack2CD = 1;

    public bool chargeAttack = false;
    public int chargeAttackRoundMax = 1;
    public int chargeAttackCurrent;

    void Update()
    {
        if(hp <= 0)
        {
            BossDeath();
        }

        if (myTurn)
        {
            myTurn = false;
            turnDuration = 0;

            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

            StartTurn();
            if(sunCreeps.Count > 0)
            {
                StartTurnPhase1();
            }
            else
            {
                StartTurnPhase2();
            }
            StartCoroutine(EndTurn(turnDuration));
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            //Debug.Log("Move");
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 1;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 0;
        moveCDCurrent = 0;

        chargeAttackCurrent = chargeAttackRoundMax;
        moveDuration = 0.25f;

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

    public void StartTurnPhase1()
    {
        //Debug.Log("sun turn");

        if(attackPhase)
        {
            if(doAttack1 && !doAttack2)
            {
                Debug.Log("att 1");
                StartAttack(upDirectionATS1);
                turnDuration += attackDuration;
                doAttack2 = true;
            }
            else if(doAttack1 && doAttack2)
            {
                Debug.Log("att 2");
                StartAttack(upDirectionATS2);
                turnDuration += attackDuration;
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

    public void StartTurnPhase2()
    {

        if (isInvisible)
        {
            TPIn(currentMap.tilesList[17]);
        }

        if (!chargeAttack)
        {
            dir = CheckAround(upDirectionATS1, false);

            if (dir != Direction.NONE)
            {
                //Debug.Log("in range");
                chargeAttack = true;

                direction = dir;
                Debug.Log("charge start");

                if (chargeAttackCurrent > 0)
                {
                    Debug.Log("charge in progress");
                    chargeAttackCurrent--;
                }
                else
                {
                    Debug.Log("attaque");
                    chargeAttack = false;
                    chargeAttackCurrent = chargeAttackRoundMax;


                    StartAttack(upDirectionATS1);
                    turnDuration += attackDuration;
                }
            }
            else
            {
                if (moveCDCurrent > 0)
                {
                    moveCDCurrent--;
                }
                else
                {
                    List<Tile> possibleAttackSpot = FindAvailableAttackSpot(upDirectionATS1);
                    pathToTarget = FindQuickestPath(currentTile, possibleAttackSpot, false);

                    if (pathToTarget != null && pathToTarget.Count > 1)
                    {
                        Move(pathToTarget[1]);
                        turnDuration += moveDuration;
                    }
                        

                    moveCDCurrent = moveCDMax;
                }
            }
        }
        else
        {
            if (chargeAttackCurrent > 0)
            {
                Debug.Log("charge in progress");
                chargeAttackCurrent--;
            }
            else
            {
                Debug.Log("attaque");
                chargeAttack = false;
                chargeAttackCurrent = chargeAttackRoundMax;

                direction = dir;
                StartAttack(upDirectionATS1);
                turnDuration += attackDuration;
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
