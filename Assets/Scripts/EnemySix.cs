using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySix : Enemy
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();

    public List<Tile> pathToTarget = new List<Tile>();

    public bool inChase = false;
    public bool chargeAttack = false;
    public int chargeAttackRoundMax = 2;
    public int chargeAttackCurrent;

    // Start is called before the first frame update
    void Start()
    {
        //test = FindPath(currentTile, currentMap.player.currentTile, false);
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 3;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        //InitAttackPattern();
        moveCDMax = 0;
        moveCDCurrent = 0;

        entityDangerousness = 3;

        chargeAttackCurrent = chargeAttackRoundMax;

        moveDuration = 0.5f;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/MobGros");

        AssignPattern();

        isInitialize = true;
    }



    private void AssignPattern()
    {
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
        upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
        upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (!inChase)
            inChase = IsTargetInChaseRange(currentMap.player.currentTile, 3);

        if (myTurn)
        {
            turnDuration = 0;
            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

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

                        if (pathToTarget != null && pathToTarget.Count > 1)
                            Move(pathToTarget[1]);

                        moveCDCurrent = moveCDMax;
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
                        EnemyRandomMove();
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
