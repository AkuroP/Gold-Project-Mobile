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

    public bool checkEnemiesInRange = false;

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
            if(!checkEnemiesInRange)
            {
                Direction dir = CheckAround(false);
                
                if(dir != Direction.NONE)
                {
                    direction = dir;
                    StartAttack();
                }
                else
                {
                    Tile nextTile = FindDirection();
                    Debug.Log(nextTile);
                    if(nextTile != null)
                    {
                        Move(nextTile);
                        dir = CheckAround(true);
                        if(dir != Direction.NONE)
                        {
                            direction = dir;
                            StartAttack();
                        }
                    }
                    else
                    {
                        //Debug.Log("PAS BOUGE");
                    }
                }
                hasMove = true;
                hasAttack = true;
                checkEnemiesInRange = false;

            }

            
            Debug.Log("my turn: " + this.gameObject.name);
            //hasMove = true;
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            Debug.Log("Move");
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;
            
        }
        
        //hp management
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private Tile FindDirection()
    {
        List<Tile> tileAround = new List<Tile>();
        
        if(currentTile.topTile != null && currentTile.topTile.isReachable && !currentTile.topTile.isHole && currentTile.topTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.topTile);
        }
        if(currentTile.rightTile != null && currentTile.rightTile.isReachable && !currentTile.rightTile.isHole && currentTile.rightTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.rightTile);
        }
        if(currentTile.bottomTile != null && currentTile.bottomTile.isReachable && !currentTile.bottomTile.isHole && currentTile.bottomTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.bottomTile);
        }
        if(currentTile.leftTile != null && currentTile.leftTile.isReachable && !currentTile.leftTile.isHole && currentTile.leftTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.leftTile);
        }
        
        Tile selectedTile = tileAround[Random.Range(0, tileAround.Count - 1)];
        return selectedTile;
    }

    private Direction CheckAround(bool _drawAttack)
    {
        checkEnemiesInRange = true;
        List<Entity> newList = new List<Entity>();
        
        newList = GetEntityInRange(ConvertPattern(upDirectionATS, Direction.UP),_drawAttack);
        for(int i = 0; i < newList.Count; i++)
        {
            if(newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if(newList.Count > 0)
        {
            //Debug.Log("Enemy spotted UP");
            return Direction.UP;
        }

        newList = GetEntityInRange(ConvertPattern(upDirectionATS, Direction.RIGHT), _drawAttack);
        for(int i = 0; i < newList.Count; i++)
        {
            if(newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if(newList.Count > 0)
        {
            //Debug.Log("Enemy spotted RIGHT");
            return Direction.RIGHT;
        }
        
       newList = GetEntityInRange(ConvertPattern(upDirectionATS, Direction.BOTTOM), _drawAttack);
        for(int i = 0; i < newList.Count; i++)
        {
            if(newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if(newList.Count > 0)
        {
            //Debug.Log("Enemy spotted BOTTOM");
            return Direction.BOTTOM;
        }

        newList = GetEntityInRange(ConvertPattern(upDirectionATS, Direction.LEFT), _drawAttack);
        for(int i = 0; i < newList.Count; i++)
        {
            if(newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if(newList.Count > 0)
        {
            //Debug.Log("Enemy spotted LEFT");
            return Direction.LEFT;
        }
        return Direction.NONE;
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

    public override void StartAttack()
    {
        
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

        //for turn by turn
        hasAttack = true;
    }

    //function to take damage / die
    /*public override void DamageSelf(int damage)
    {

    }*/
}
