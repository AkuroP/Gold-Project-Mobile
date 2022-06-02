using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    //player behaviour export 
/*    public bool canMove = true;
    public bool moveInProgress = false;


    public Vector3 targetPosition, currentPosition;

    private float timeElapsed;
    public float moveDuration;*/

    //Number of essences (= points of action)
    private int numEssence = 100;
    private int attackCost = 2;

    public bool attackNext = false;
    public bool moveNext = false;

    [SerializeField] private int weaponDamage;

    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        weapon = new Weapon(WeaponType.DAGGER);
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //turn management
        if(myTurn)
        {
            Debug.Log("my turn: " + this.gameObject.name);
        }

        //hp management
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartAttack();
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;

            
        }
    }

    public override void FindNextTile()
    {
       if(myTurn && !hasMove)
        {
            switch (direction)
            {
                case Direction.UP:
                    Tile topTile = currentMap.FindTopTile(currentTile);
                    if (currentMap.CheckMove(topTile))
                    {
                        MovePlayer(ref topTile);
                    }
                    break;
                case Direction.RIGHT:
                    Tile rightTile = currentMap.FindRightTile(currentTile);
                    if (currentMap.CheckMove(rightTile))
                    {
                        MovePlayer(ref rightTile);
                    }
                    break;
                case Direction.BOTTOM:
                    Tile bottomTile = currentMap.FindBottomTile(currentTile);
                    if (currentMap.CheckMove(bottomTile))
                    {
                        MovePlayer(ref bottomTile);
                    }
                    break;
                case Direction.LEFT:
                    Tile leftTile = currentMap.FindLeftTile(currentTile);
                    if (currentMap.CheckMove(leftTile))
                    {
                        MovePlayer(ref leftTile);
                    }
                    break;
            }
        }
    }

    public void MovePlayer(ref Tile _targetTile)
    {
        currentPosition = transform.position;
        targetPosition = _targetTile.transform.position;

        if(!_targetTile.isHole)
        {
            _targetTile.entityOnTile = currentTile.entityOnTile;
            currentTile.entityOnTile = null;
            currentTile = _targetTile;
        }

        moveInProgress = true;
        canMove = false;

        //for turn by turn
        hasMove = true;
    }

    

    public override void StartAttack()
    {
        if(myTurn && !hasAttack)
        {
            numEssence -= attackCost;

            List<AttackTileSettings> attackPattern = ConvertPattern(weapon.upDirectionATS, direction);

            List<Entity> enemiesInRange = new List<Entity>();

            enemiesInRange = GetEntityInRange(attackPattern);

            if (enemiesInRange != null && enemiesInRange.Count > 0)
            {
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] is Enemy)
                        Damage(weapon.weaponDamage, enemiesInRange[i]);
                }
            }

            //for turn by turn
            hasAttack = true;
        }
    }

    

    

    public void AttackButton()
    {
        if(!attackNext)
        {
            attackNext = true;
        }
        else
        {
            attackNext = false;
        }
    }

    //function to take damage / die
/*    public override void DamageSelf(int damage)
    {
        
    }*/
}
