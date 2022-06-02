using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    RIGHT,
    BOTTOM,
    LEFT
}

public class Entity : MonoBehaviour
{
    [Header("==== Map Informations ====")]
    public Map currentMap;
    public Tile currentTile;

    [Header("==== Stat ====")]
    //hp for enemy, turn left for player
    public int maxHP;
    public int hp;
    //priority of entity (player always first)
    public int prio;
    [Header("==== Movement ====")]
    //mobility of entity per turn
    public int maxMobility;
    //movement of entity in X and Y
    public int mobilityX;
    public int mobilityY;

    //directions enum and direction of the entity

    public Direction direction;

    //weapons enum and weapon equiped by the entity
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected int weaponRange;

    //effects enum, effects on the weapon, effects on the entity
    [SerializeField] protected WeaponEffect weaponEffect;
    [SerializeField] protected WeaponEffect effectOnEntity;

    public Vector3 targetPosition, currentPosition;

    //player behaviour export
    public bool canMove = true;
    public bool moveInProgress = false;

    public float timeElapsed;
    public float moveDuration;
    public bool canAttack = true;

    //turn by turn

    public bool myTurn = false;
    public bool hasMove = false;
    public bool hasAttack = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MovingProcess()
    {
        //move process
        if (moveInProgress && canMove && timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
            canMove = false;
        }
        else
        {
            moveInProgress = false;
            timeElapsed = 0;
        }
    }

    //virtual attack function
    public virtual void StartAttack()
    {

    }

    //draw attack zone
    public IEnumerator DrawAttack(Tile tile)
    {
        Color oldColor = tile.tileColor;
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        tile.GetComponent<SpriteRenderer>().color = oldColor;
    }

    //find ennemies in attack range
    public List<Entity> GetEntityInRange(List<AttackTileSettings> ats)
    {
        List<Entity> entityInPattern = new List<Entity>();

        foreach (AttackTileSettings oneATS in ats)
        {
            Tile attackedTile = currentTile;

            for (int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
            {
                if (oneATS.offsetX > 0)
                    attackedTile = currentMap.FindLeftTile(attackedTile);
                else if (oneATS.offsetX < 0)
                    attackedTile = currentMap.FindRightTile(attackedTile);
            }

            for (int i = 0; i < Mathf.Abs(oneATS.offsetY); i++)
            {
                if (oneATS.offsetY > 0)
                    attackedTile = currentMap.FindTopTile(attackedTile);
                else if (oneATS.offsetY < 0)
                    attackedTile = currentMap.FindBottomTile(attackedTile);
            }

            if (attackedTile != null)
            {
                //stop attack when a wall is reached
                if (attackedTile.isWall)
                {
                    return entityInPattern;
                }

                StartCoroutine(DrawAttack(attackedTile));
                if (attackedTile.entityOnTile)
                {
                    entityInPattern.Add(attackedTile.entityOnTile);
                }

                return entityInPattern;

            }
        }

        return null;
    }

    public List<AttackTileSettings> ConvertPattern(List<AttackTileSettings> upDirectionATS, Direction entityDirection)
    {
        List<AttackTileSettings> newATS = new List<AttackTileSettings>();

        switch (entityDirection)
        {
            case Direction.UP:
                newATS = upDirectionATS;
                break;

            case Direction.BOTTOM:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int newOffsetX = -1 * ats.offsetX;
                    int newOffsetY = -1 * ats.offsetY;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.LEFT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = ats.offsetY;
                    int newOffsetY = -1 * temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.RIGHT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = -1 * ats.offsetY;
                    int newOffsetY = temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;
        }

        return newATS;
    }

    //function to take damage / die
    public void Damage(int damage, Entity entity)
    {
        entity.hp -= damage;
    }

    public virtual void Move(Tile _targetTile)
    {
        currentPosition = transform.position;
        targetPosition = _targetTile.transform.position;
        moveInProgress = true;

        if(!_targetTile.isHole)
        {
            currentTile = _targetTile;
        }
    }

    public virtual void FindNextTile()
    {
        switch (direction)
        {
            case Direction.UP:
                Tile topTile = currentMap.FindTopTile(currentTile);
                if (currentMap.CheckMove(topTile))
                {
                    this.Move(topTile);
                }
                break;
            case Direction.RIGHT:
                Tile rightTile = currentMap.FindRightTile(currentTile);
                if (currentMap.CheckMove(rightTile))
                {
                    this.Move(rightTile);
                }
                break;
            case Direction.BOTTOM:
                Tile bottomTile = currentMap.FindBottomTile(currentTile);
                if (currentMap.CheckMove(bottomTile))
                {
                    this.Move(bottomTile);
                }
                break;
            case Direction.LEFT:
                Tile leftTile = currentMap.FindLeftTile(currentTile);
                if (currentMap.CheckMove(leftTile))
                {
                    this.Move(leftTile);
                }
                break;
        }
        //enableMove = false;
    }

}
