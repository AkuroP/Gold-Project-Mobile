using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    

    //Number of essences (= points of action)
    public int numEssence = 100;
    [SerializeField] private int attackCost = 3;
    [SerializeField] private int moveCost = 1;

    public bool attackNext = false;
    public bool moveNext = false;
    public bool changingRoom = false;

    [SerializeField] private int weaponDamage;

    private GameManager instanceGM;

    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        currentPosition = transform.position;
        weapon = new Weapon(WeaponType.DAGGER);
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //turn management
        if (Input.GetKeyDown(KeyCode.B))
        {
            this.gameObject.transform.position = currentTile.gameObject.transform.position;
        }

        //hp management
        if (hp <= 0)
        {
            if(Inventory.instanceInventory.HasItem("Revivor"))
            {
                hp = 1;
                Inventory.instanceInventory.RemoveItem("Revivor");
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
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

            if (currentTile.isPike && !isOnThePike)
            {
                currentTile.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                isOnThePike = true;
                if (Inventory.instanceInventory.HasItem("Trap Protector") == true)
                {
                    ShopItem trapProtector = Inventory.instanceInventory.GetItem("Trap Protector");
                    Debug.Log(trapProtector.itemName + ", " + trapProtector.itemCooldown);
                    if (trapProtector.itemCooldown == 0)
                    {
                        trapProtector.itemCooldown = 5;
                    }
                    else
                    {
                        this.hp--;
                    }
                }
                else
                {
                    this.hp--;
                }
            }

            if (currentTile.tileIndex == currentMap.exitTileIndex && changingRoom == false)
            {
                changingRoom = true;
                StartCoroutine(GoToNextRoom());
            }
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
                        Move(topTile);
                        numEssence -= moveCost;
                    }
                    break;
                case Direction.RIGHT:
                    Tile rightTile = currentMap.FindRightTile(currentTile);
                    if (currentMap.CheckMove(rightTile))
                    {
                        Move(rightTile);
                        entitySr.flipX = true;
                        numEssence -= moveCost;
                    }
                    break;
                case Direction.BOTTOM:
                    Tile bottomTile = currentMap.FindBottomTile(currentTile);
                    if (currentMap.CheckMove(bottomTile))
                    {
                        Move(bottomTile);
                        numEssence -= moveCost;
                    }
                    break;
                case Direction.LEFT:
                    Tile leftTile = currentMap.FindLeftTile(currentTile);
                    if (currentMap.CheckMove(leftTile))
                    {
                        Move(leftTile);
                        entitySr.flipX = false;
                        numEssence -= moveCost;
                    }
                    break;
            }
        }
    }

    public override void StartAttack(List<AttackTileSettings> _upDirectionATS)
    {
        if(myTurn && !hasAttack)
        {
            numEssence -= attackCost;

            List<AttackTileSettings> attackPattern = ConvertPattern(_upDirectionATS, direction);

            List<Entity> enemiesInRange = new List<Entity>();

            enemiesInRange = GetEntityInRange(attackPattern, true);

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
            hasPlay = true;
        }
    }

    

    

    public void AttackButton()
    {
        if(SwipeDetection.instanceSD.blockInputs == false)
        {
            if (!attackNext)
            {
                attackNext = true;
            }
            else
            {
                attackNext = false;
            }
        }
    }

    public IEnumerator GoToNextRoom()
    {
        UI.instanceUI.Fade();
        yield return new WaitForSeconds(0.5f);
        GameManager.instanceGM.UpdateScoreAndMap();
        changingRoom = false;
    }

    //function to take damage / die
/*    public override void DamageSelf(int damage)
    {
        
    }*/
}
