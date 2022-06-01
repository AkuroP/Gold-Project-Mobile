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

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        if(!_targetTile.isHole)
        {
            currentTile = _targetTile;
        }
    }

    public virtual void FindNextTile()
    {
        switch (direction)
        {
            case Entity.Direction.UP:
                Tile topTile = currentMap.FindTopTile(currentTile);
                if (currentMap.CheckMove(topTile))
                {
                    Move(topTile);
                }
                break;
            case Entity.Direction.RIGHT:
                Tile rightTile = currentMap.FindRightTile(currentTile);
                if (currentMap.CheckMove(rightTile))
                {
                    Move(rightTile);
                }
                break;
            case Entity.Direction.BOTTOM:
                Tile bottomTile = currentMap.FindBottomTile(currentTile);
                if (currentMap.CheckMove(bottomTile))
                {
                    Move(bottomTile);
                }
                break;
            case Entity.Direction.LEFT:
                Tile leftTile = currentMap.FindLeftTile(currentTile);
                if (currentMap.CheckMove(leftTile))
                {
                    Move(leftTile);
                }
                break;
        }
        //enableMove = false;
    }

}
