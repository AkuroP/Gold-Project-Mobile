using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //for pathfinding A*
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private List<Tile> openList;
    private List<Tile> closedList;

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

    public bool IsTargetInChaseRange(Tile _tragetCurrentTile, int range)
    {
        bool goodInX = false;
        bool goodInY = false;

        if(_tragetCurrentTile.tileX <= currentTile.tileX + range && _tragetCurrentTile.tileX >= currentTile.tileX - range)
            goodInX = true;

        if (_tragetCurrentTile.tileY <= currentTile.tileY + range && _tragetCurrentTile.tileY >= currentTile.tileY - range)
            goodInY = true;


        if(goodInX && goodInY)
        {
            Debug.Log("start chase");
            return true;
        }
        else
            return false;
    }

    public List<Tile> FindPath(Tile _originTile, Tile _targetTile, bool _drawDebug)
    {
        Tile startTile = _originTile;
        Tile endTile = _targetTile;

        //open List for Tile to check, closed List for the one checked
        openList = new List<Tile>() { startTile };
        closedList = new List<Tile>();

        foreach(Tile oneTile in currentMap.tilesList)
        {
            oneTile.gCost = int.MaxValue;
            oneTile.CalculateFCost();
            oneTile.cameFromTile = null;
        }

        startTile.gCost = 0;
        startTile.hCost = CalculateDistanceCost(startTile, endTile);
        startTile.CalculateFCost();


        int step = 0;
        while(openList.Count > 0)
        {
            Tile currentTile = FindLowestFCostTile(openList);

            //======= DEBUG START =======//
            if(_drawDebug)
                StartCoroutine(currentTile.TurnColor(new Color(0f, 1f, 0f, 1f), step));
            //======= DEBUG END =======//

            if (currentTile == endTile)
            {
                //======= DEBUG START =======//
                if(_drawDebug)
                {
                    foreach (Tile tile in closedList)
                    {
                        if (!tile.isReachable) continue;
                        StartCoroutine(tile.TurnColor(new Color(0.6f, 0.6f, 0.6f, 1f), step));
                    }
                    foreach (Tile tile in closedList)
                    {
                        if (!tile.isReachable) continue;
                        StartCoroutine(tile.TurnColor(new Color(0.6f, 0.6f, 0.6f, 1f), step));
                    }
                }
                //======= DEBUG END =======//

                //Final Tile reached
                return CalculatePath(endTile, step);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach(Tile neighbourTile in GetNeighboursList(currentTile))
            {
                //if in closed list Tile is alrday checked, go next
                if (closedList.Contains(neighbourTile)) continue;
                //if is not reachable go next 
                if (!neighbourTile.isReachable)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }

                if(_drawDebug)
                    StartCoroutine(neighbourTile.TurnColor(new Color(0f, 0f, 1f, 1f), step));
  
                int tentativeGCost = currentTile.gCost + CalculateDistanceCost(currentTile, neighbourTile);
                if (tentativeGCost < neighbourTile.gCost)
                {
                    neighbourTile.cameFromTile = currentTile;
                    neighbourTile.gCost = tentativeGCost;
                    neighbourTile.hCost = CalculateDistanceCost(neighbourTile, endTile);
                    neighbourTile.CalculateFCost();


                    if (!openList.Contains(neighbourTile)) 
                    { 
                        openList.Add(neighbourTile); 
                    }
                }
            }

            //======= DEBUG START =======//
            if(_drawDebug)
                StartCoroutine(currentTile.TurnColor(new Color(1f, 0f, 0f, 1f), step + 1));
            //======= DEBUG END =======//

            step++;
        }
        
        //open List is empty, cannot reach targetTile
        return null;
    }

    private int CalculateDistanceCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.tileX - b.tileX);
        int yDistance = Mathf.Abs(a.tileY - b.tileY);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Tile FindLowestFCostTile(List<Tile> tileList)
    {
        Tile lowestFCostTile = tileList[0];

        for(int i = 0; i < tileList.Count; i++)
        {
            if(tileList[i].fCost < lowestFCostTile.fCost)
            {
                lowestFCostTile = tileList[i];
            }
        }

        return lowestFCostTile;
    }

    private List<Tile> GetNeighboursList(Tile tile)
    {
        List<Tile> neighbourList = new List<Tile>();

        if(tile.topTile != null)
            neighbourList.Add(tile.topTile);
        if(tile.rightTile != null)
                    neighbourList.Add(tile.rightTile);
        if(tile.bottomTile != null)
                    neighbourList.Add(tile.bottomTile);
        if(tile.leftTile != null)
                    neighbourList.Add(tile.leftTile);

        return neighbourList;
    }

    private List<Tile> CalculatePath(Tile endTile, int step)
    {
        List<Tile> path = new List<Tile>();

        path.Add(endTile);

        Tile currentTile = endTile;
        while(currentTile.cameFromTile != null)
        {
            //======= DEBUG START =======//
            //StartCoroutine(currentTile.TurnColor(new Color(0f, 1f, 0f, 1f), step));
            //======= DEBUG END =======//

            path.Add(currentTile.cameFromTile);
            currentTile = currentTile.cameFromTile;
        }

        path.Reverse();
        return path;
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
                    Damage(enemyDamage, enemiesInRange[i]);
            }
        }

    }

}
