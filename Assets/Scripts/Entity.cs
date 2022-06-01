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
    protected int hp;
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

    private float timeElapsed;
    public float moveDuration;
    public bool hasMoved = false;
    public bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
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
            hasMoved = true;
            if(Vector3.Distance(this.transform.position, targetPosition) <= 1.1f)
            {
                canMove = false;
            }
        }
        else
        {
            moveInProgress = false;
            ResetAction();        
            timeElapsed = 0;
        }
    }

    public void ResetAction()
    {
        if(hasMoved && !canAttack)
        {
            canMove = true;
            hasMoved = false;
            canAttack = true;
            GameManager.instanceGM.ChangeTurn();
        }
    }

    //virtual attack function
    public virtual void Attack()
    {
        
    }

    //function to take damage / die
    public virtual void DamageSelf(int damage)
    {

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
