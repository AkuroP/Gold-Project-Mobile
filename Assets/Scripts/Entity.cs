using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum Direction {UP, RIGHT, BOTTOM, LEFT};
    public Direction direction;

    //weapons enum and weapon equiped by the entity
    protected enum WeaponType {DAGGER, GRIMOIRE, HANDGUN};
    [SerializeField] protected WeaponType weaponType;
    [SerializeField] protected int weaponRange;

    //effects enum, effects on the weapon, effects on the entity
    protected enum WeaponEffect {NONE, BURN, FREEZE};
    [SerializeField] protected WeaponEffect weaponEffect;
    [SerializeField] protected WeaponEffect effectOnEntity;

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
    public virtual void Damage(int damage)
    {

    }
}
