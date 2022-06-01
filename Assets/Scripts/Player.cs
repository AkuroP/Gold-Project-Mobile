using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    //player behaviour export 
    public bool canMove = true;
    public bool moveInProgress = false;


    public Vector3 targetPosition, currentPosition;

    private float timeElapsed;
    public float moveDuration;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Attack();
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

    public void FindNextTile()
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
        //enableMove = false;
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
    }

    //draw attack zone
    public IEnumerator DrawAttack(Tile tile)
    {
        Color oldColor = tile.tileColor;
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        tile.GetComponent<SpriteRenderer>().color = oldColor;
    }

    public override void Attack()
    {
        numEssence -= attackCost;

        List<AttackTileSettings> attackPattern = weapon.ConvertPattern(weapon.upDirectionATS, direction);

        List<Entity> enemiesInRange = new List<Entity>();
        enemiesInRange = GetEnnemiesInRange(attackPattern);

        if (enemiesInRange != null && enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].DamageSelf(weapon.weaponDamage);
            }
        }
    }

    public List<Entity> GetEnnemiesInRange(List<AttackTileSettings> ats)
    {
        List<Entity> ennemiesInPattern = new List<Entity>();

        foreach(AttackTileSettings oneATS in ats)
        {
            Tile attackedTile = currentTile;

            for(int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
            {
                if(oneATS.offsetX > 0)
                    attackedTile = currentMap.FindLeftTile(attackedTile);
                else if(oneATS.offsetX < 0)
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
                    return ennemiesInPattern;
                }

                StartCoroutine(DrawAttack(attackedTile));
                if (attackedTile.entityOnTile)
                {
                    ennemiesInPattern.Add(attackedTile.entityOnTile);
                }

                return ennemiesInPattern;
                
            }
        }



        return null; 
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
    public override void DamageSelf(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
