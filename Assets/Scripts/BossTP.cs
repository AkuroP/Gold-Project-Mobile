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
    private GameObject sunfireGO;

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

        switch (this.sunCreeps.Count)
        {
            case 1:
                heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
                heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
                break;
            case 2:
                heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/emptys");
                break;
            case 3:
                heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/creepsheart");
                break;
            default:
                break;
        }
        if(sunCreeps.Count == 0)
        {
            heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
            heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
            heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Graphics/empty");
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
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/sun");
        sunfireGO = Resources.Load("Prefabs/SunFireEffect") as GameObject;

        AssignPattern();

        isInitialize = true;

        heart1 = this.transform.Find("Heart1").gameObject;
        heart2 = this.transform.Find("Heart2").gameObject;
        heart3 = this.transform.Find("Heart3").gameObject;
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
                List<Tile> fireTile = this.GetTileInRange(upDirectionATS1, false);
                Debug.Log("fire tile count : " + fireTile.Count);
                StartCoroutine(SpawnFire(fireTile));
                StartAttack(upDirectionATS1);
                turnDuration += attackDuration;
                doAttack2 = true;
            }
            else if(doAttack1 && doAttack2)
            {
                Debug.Log("att 2");
                List<Tile> fireTile = this.GetTileInRange(upDirectionATS2, false);
                Debug.Log("fire tile count : " + fireTile.Count);
                StartCoroutine(SpawnFire(fireTile));
                StartAttack(upDirectionATS2);
                turnDuration += attackDuration;
                doAttack1 = false;
                doAttack2 = false;
                attackPhase = false;
            }
            enemyAnim.SetTrigger("Atk");
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

                    enemyAnim.SetTrigger("Atk");
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
                enemyAnim.SetTrigger("Atk");
                turnDuration += attackDuration;
            }
        }
    }

    public void TPOut()
    {
        currentTile.entityOnTile = null;
        enemyAnim.SetBool("TP", true);
        heart1.GetComponent<SpriteRenderer>().enabled = false;
        heart2.GetComponent<SpriteRenderer>().enabled = false;
        heart3.GetComponent<SpriteRenderer>().enabled = false;
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
        entitySr.sortingOrder = 11 - currentTile.tileY;
        transform.position = currentTile.transform.position;
        enemyAnim.SetBool("TP", false);
        heart1.GetComponent<SpriteRenderer>().enabled = true;
        heart2.GetComponent<SpriteRenderer>().enabled = true;
        heart3.GetComponent<SpriteRenderer>().enabled = true;
        isInvisible = false;
    }

    private IEnumerator SpawnFire(List<Tile> _fireTiles)
    {
        for(int i = 0; i < _fireTiles.Count; i++)
        {
            float randomSpawn = Random.Range(0f, 0.05f);
            Debug.Log("Spawn Time :" + randomSpawn);
            yield return new WaitForSeconds(randomSpawn);
            GameObject sFG = Instantiate(sunfireGO);
            sFG.transform.parent = _fireTiles[i].transform;
            sFG.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }
    }

}
