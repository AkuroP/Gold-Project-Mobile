using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFive : Enemy
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();

    public List<Tile> pathToTarget = new List<Tile>();

    public bool inChase = false;
    public bool chargeAttack = false;
    public int chargeAttackRoundMax = 0;
    public int chargeAttackCurrent;

    // Start is called before the first frame update
    void Start()
    {
        //test = FindPath(currentTile, currentMap.player.currentTile, false);
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/doggo");
    }

    public override void Init()
    {
        Debug.Log("d�butInit");

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 2;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 1;
        moveCDCurrent = 0;
        moveDuration = 0.5f;
        attackDuration = .45f;

        entityDangerousness = 2;

        chargeAttackCurrent = chargeAttackRoundMax;

        entitySr = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/MobFast");

        AssignPattern();

        turnArrow = this.transform.Find("Arrow").gameObject;

        heart1 = this.transform.Find("Heart1").gameObject;
        heart2 = this.transform.Find("Heart2").gameObject;
        heart3 = this.transform.Find("Heart3").gameObject;
        heart2.SetActive(false);

        Debug.Log("finInit");

        isInitialize = true;
    }

    private void AssignPattern()
    {
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inChase)
            inChase = IsTargetInChaseRange(currentMap.player.currentTile, 3);

        if (myTurn)
        {
            turnArrow.SetActive(true);
            myTurn = false;
            turnDuration = 0;

            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

            StartTurn();
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

            if (currentTile.isPike && !isOnThePike)
            {
                currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_25");
                isOnThePike = true;
                AchievementManager.instanceAM.UpdateTrapsActivated();
                if (Inventory.instanceInventory.HasItem("Trap Protector") == true)
                {
                    ShopItem trapProtector = Inventory.instanceInventory.GetItem("Trap Protector");
                    Debug.Log(trapProtector.itemName + ", " + trapProtector.itemCooldown);
                    if (trapProtector.itemCooldown == 0)
                    {
                        trapProtector.itemCooldown = 5;
                    }
                    else
                    {
                        Damage(1, this);
                    }
                }
                else
                {
                    Damage(1, this);
                }
            }

            entitySr.sortingOrder = 11 - this.currentTile.tileY;
        }

        if (isInitialize)
            IsSelfDead();

        switch(this.hp)
        {
            case 1:
                heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                break;
            case 2:
                heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                break;
            default:
                break;
        }
    }

    public override void StartTurn()
    {
        if (!chargeAttack)
        {
            dir = CheckAround(upDirectionATS, false);

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
                    enemyAnim.SetTrigger("Atk");
                    chargeAttack = false;
                    chargeAttackCurrent = chargeAttackRoundMax;

                    StartAttack(upDirectionATS);
                    turnDuration += attackDuration;
                }
            }
            else
            {
                //Debug.Log("move");
                //chase move
                if (inChase)
                {
                    if (moveCDCurrent > 0)
                    {
                        moveCDCurrent--;
                    }
                    else
                    {
                        List<Tile> possibleAttackSpot = FindAvailableAttackSpot(upDirectionATS);
                        pathToTarget = FindQuickestPath(currentTile, possibleAttackSpot, false);

                        if(pathToTarget != null)
                        {
                            List<Tile> temp = new List<Tile>();
                            temp.Add(pathToTarget[1]);
                            if (pathToTarget.Count > 2)
                                temp.Add(pathToTarget[2]);

                            if (temp.Count > 0)
                            {
                                enemyAnim.SetTrigger("Move");
                                Move(temp);
                                turnDuration += moveDuration;
                                moveCDCurrent = moveCDMax;
                            }
                        }
                        
                    }
                }
                //random move
                else
                {
                    if (moveCDCurrent > 0)
                    {
                        moveCDCurrent--;
                    }
                    else
                    {
                        Tile firstTile = FindDirection(currentTile);
                        Tile secondTile = FindDirection(firstTile);

                        List<Tile> temp = new List<Tile>();
                        temp.Add(firstTile);
                        temp.Add(secondTile);

                        enemyAnim.SetTrigger("Move");
                        Move(temp);
                        turnDuration += moveDuration;

                        moveCDCurrent = moveCDMax;
                    }
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
                enemyAnim.SetTrigger("Atk");
                StartAttack(upDirectionATS);
                turnDuration += attackDuration;
            }
        }
    }
}
