using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();
    public int enemyDamage;
    private bool hasRandom;
    public enum EnemyType
    {
        ENEMY1,
        ENEMY2,
        ENEMY3
    } 
    public EnemyType whatEnemy;
    [SerializeField]private int parity = 0;

    private Player player;
    public float enemyRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        InitAttackPattern();
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn)
        {
            if(!hasMove && !hasAttack)
            {
                
                if(whatEnemy == EnemyType.ENEMY3)
                {
                    if(Vector3.Distance(this.transform.position, player.transform.position) > enemyRange)
                    {
                        if(!hasRandom)
                        {
                            EnemyRandomMove();
                            hasRandom = true;
                        }
                    }
                    else
                    {
                        List<Entity> playerInRange = new List<Entity>();
                        playerInRange = GetEntityInRange(upDirectionATS);
                        if(playerInRange == null || playerInRange.Count <= 0)
                        {
                            float difX = Mathf.Abs(player.transform.position.x) - Mathf.Abs(this.transform.position.x);
                            float difY = Mathf.Abs(player.transform.position.y) - Mathf.Abs(this.transform.position.y);
                            //Debug.Log("DIF X : " + difX);
                            //Debug.Log("DIF Y : " + difY);
                            //chase player
                            if(Mathf.Abs(difX) > Mathf.Abs(difY))
                            {
                                if(difX > 0)
                                {
                                    direction = Direction.RIGHT;
                                }
                                else
                                {
                                    direction = Direction.LEFT;
                                }
                            }
                            else
                            {
                                if(difY > 0)
                                {
                                    direction = Direction.UP;
                                }
                                else
                                {
                                    direction = Direction.BOTTOM;
                                }
                            }
                            FindNextTile();
                        }
                        else
                        {
                            Debug.Log(GetEntityInRange(upDirectionATS)[0].name);
                            if(!moveInProgress && playerInRange.Count > 0)
                            {
                                StartAttack();
                                hasAttack = true;
                            }
                        }
                        
                    }
                }

                if (moveInProgress && !canMove && timeElapsed < moveDuration && !hasAttack)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
                    timeElapsed += Time.deltaTime;
                }
                else
                {
                    moveInProgress = false;
                    canMove = true;
                    timeElapsed = 0;
                    hasMove = true;
                }
            }
            else
            {
                GoNext();
            }
            Debug.Log("my turn: " + this.gameObject.name);
            //hasMove = true;
        }
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        if (myTurn && Input.GetKeyDown(KeyCode.Z) && !hasAttack)
        {
            StartAttack();
            hasAttack = true;
        }
    }

    private void GoNext()
    {
        myTurn = false;
        hasMove = false;
        hasAttack = false;
        hasRandom = false;
        GameManager.instanceGM.StartCoroutine(GameManager.instanceGM.ChangeEntity());
    }
    private void InitAttackPattern()
    {
        upDirectionATS.Clear();
        switch(whatEnemy)
        {
            case EnemyType.ENEMY1 :
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
            break;
            case EnemyType.ENEMY2 :
            if(this.parity == 0)
            {
                //pattern 1
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 0));
                upDirectionATS.Add(new AttackTileSettings(1, -1, -1));
                upDirectionATS.Add(new AttackTileSettings(1, 0, -1));
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                //pattern 2
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, -1));
                this.parity = 0;
            }
            break;
            case EnemyType.ENEMY3 :
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
            break;
            
        }
    }

    public void ChasePlayer()
    {

    }
    /*public void EnemyBehaviour()
    {
        switch(whatEnemy)
        {
            case EnemyType.ENEMY1 :
            if(this.parity == 0)
            {
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                this.StartAttack();
                this.parity = 0;
            }
            break;
            case EnemyType.ENEMY2 :
            if(this.parity == 0)
            {
                upDirectionATS.Clear();
                //pattern 1
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 0));
                upDirectionATS.Add(new AttackTileSettings(1, -1, -1));
                upDirectionATS.Add(new AttackTileSettings(1, 0, -1));
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                upDirectionATS.Clear();
                //pattern 2
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, -1));
            }
            this.StartAttack();
            break;
            case EnemyType.ENEMY3 :
            if(Vector3.Distance(this.transform.position, player.transform.position) > enemyRange)
            {
                this.EnemyRandomMove();
                canAttack = false;
            }
            else
            {
                this.StartAttack();
            }   
            break;
        }
    }*/
    public void EnemyRandomMove()
    {
        int randomMove = Random.Range(0, 4);
        switch(randomMove)
        {
            case 0:
                direction = Direction.UP;
                FindNextTile();
            break;
            case 1:
                direction = Direction.LEFT;
                FindNextTile();
            break;
            case 2:
                direction = Direction.BOTTOM;
                FindNextTile();
            break;
            case 3:
                direction = Direction.LEFT;
                FindNextTile();
            break;
        }
        
    }
    public override void Move(Tile _targetTile)
    {
        if(!_targetTile.isWall)
        {
            base.Move(_targetTile);
            canMove = false;
        }
        else
        {
            if(whatEnemy == EnemyType.ENEMY3)
            {
                EnemyRandomMove();
            }
            else
            {
                hasMove = true;
            }
        }
    }

    public override void StartAttack()
    {
        if(whatEnemy == EnemyType.ENEMY2)
        {
            if(this.parity == 0)
            {
                upDirectionATS.Clear();
                //pattern 1
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 0));
                upDirectionATS.Add(new AttackTileSettings(1, -1, -1));
                upDirectionATS.Add(new AttackTileSettings(1, 0, -1));
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                upDirectionATS.Clear();
                //pattern 2
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, -1));
            }
        }
        List<AttackTileSettings> attackPattern = ConvertPattern(upDirectionATS, direction);

        List<Entity> enemiesInRange = new List<Entity>();
        enemiesInRange = GetEntityInRange(attackPattern);

        if (enemiesInRange != null && enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] is Player)
                    Damage(enemyDamage, enemiesInRange[i]);
            }
        }
    }

    //function to take damage / die
    /*public override void DamageSelf(int damage)
    {

    }*/
}
