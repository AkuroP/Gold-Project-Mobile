using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public int enemyDamage;
    private bool hasRandom;
    public enum EnemyType
    {
        ENEMY1,
        ENEMY2,
        ENEMY3
    } 
    public EnemyType whatEnemy;
    public int maxCD;
    public int cd;

    public Player player;
    public float enemyRange;

    public bool checkEnemiesInRange = false;
    public Direction dir;

    public bool isInitialize = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public virtual void StartTurn()
    {
        Debug.Log("START TURN");
    }

    public void IsSelfDead()
    {
        //hp management
        if (hp <= 0)
        {
            Destroy(this.gameObject);
            player.numEssence += 5;
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
        
        Tile selectedTile = tileAround[Random.Range(0, tileAround.Count)];
        return selectedTile;
    }

    public Direction CheckAround(List<AttackTileSettings> _upDirectionATS, bool _drawAttack)
    {
        checkEnemiesInRange = true;
        List<Entity> newList = new List<Entity>();
        
        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.UP),_drawAttack);
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

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.RIGHT), _drawAttack);
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
        
       newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.BOTTOM), _drawAttack);
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

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.LEFT), _drawAttack);
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

    /*public void InitAttackPattern()
    {
        upDirectionATS.Clear();
        switch(whatEnemy)
        {
            case EnemyType.ENEMY1 :
                
            break;
            case EnemyType.ENEMY2 :
            if(this.parity == 0)
            {
                //pattern 1
                
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                
                this.parity = 0;
            }
            break;
            case EnemyType.ENEMY3 :
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
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
                direction = Direction.RIGHT;
                FindNextTile();
                break;
        }
        
    }

    public override void StartAttack(List<AttackTileSettings> _upDirectionATS)
    {
        
        List<AttackTileSettings> attackPattern = ConvertPattern(_upDirectionATS, direction);

        List<Entity> enemiesInRange = new List<Entity>();
        enemiesInRange = GetEntityInRange(attackPattern);

        if (enemiesInRange != null && enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] is Player)
                {
                    if (Inventory.instanceInventory.HasItem("Poison Fog") && Inventory.instanceInventory.HasItem("Invincibility") == false)
                    {
                        Inventory.instanceInventory.RemoveItem("Poison Fog");
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach(GameObject enemy in enemies)
                        {
                            enemy.GetComponent<Enemy>().hp--;
                        }
                    }
                    else if (Inventory.instanceInventory.HasItem("Invincibility"))
                    {
                        player.invincibilityTurn = 3;
                        Inventory.instanceInventory.RemoveItem("Invincibility");
                    }
                    Damage(enemyDamage, enemiesInRange[i]);
                    if(Inventory.instanceInventory.HasItem("Counter Ring"))
                    {
                        player.damageMultiplicator = 2;
                    }
                }
            }
        }

    }

    //function to take damage / die
    /*public override void DamageSelf(int damage)
    {

    }*/
}
