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
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 2;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 1;
        moveCDCurrent = 0;
        moveDuration = 0.25f;

        chargeAttackCurrent = chargeAttackRoundMax;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/MobFast");

        AssignPattern();

        isInitialize = true;
    }

    private void AssignPattern()
    {
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (!inChase)
            inChase = IsTargetInChaseRange(currentMap.player.currentTile, 3);

        if (myTurn)
        {
            StartTurn();

            hasPlay = true;
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

        if (isInitialize)
            IsSelfDead();
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

                    direction = dir;
                    StartAttack(upDirectionATS);
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
                                Move(temp);
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

                        Move(temp);

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
                StartAttack(upDirectionATS);
            }
        }
    }
}
