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

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
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
            case Entity.Direction.UP:
                Tile topTile = currentMap.FindTopTile(currentTile);
                if (currentMap.CheckMove(topTile))
                {
                    MovePlayer(topTile.transform.position);
                    currentTile = topTile;
                }
                break;
            case Entity.Direction.RIGHT:
                Tile rightTile = currentMap.FindRightTile(currentTile);
                if (currentMap.CheckMove(rightTile))
                {
                    MovePlayer(rightTile.transform.position);
                    currentTile = rightTile;
                }
                break;
            case Entity.Direction.BOTTOM:
                Tile bottomTile = currentMap.FindBottomTile(currentTile);
                if (currentMap.CheckMove(bottomTile))
                {
                    MovePlayer(bottomTile.transform.position);
                    currentTile = bottomTile;
                }
                break;
            case Entity.Direction.LEFT:
                Tile leftTile = currentMap.FindLeftTile(currentTile);
                if (currentMap.CheckMove(leftTile))
                {
                    MovePlayer(leftTile.transform.position);
                    currentTile = leftTile;
                }
                break;
        }
        //enableMove = false;
    }

    public void MovePlayer(Vector3 _targetPosition)
    {
        currentPosition = transform.position;
        targetPosition = _targetPosition;

        moveInProgress = true;
        canMove = false;
    }

    //draw attack zone
    public IEnumerator DebugAttack(Tile tile)
    {
        Color oldColor = tile.tileColor;
        tile.tileGO.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        tile.tileGO.GetComponent<SpriteRenderer>().color = oldColor;
    }

    public override void Attack()
    {
        numEssence -= attackCost;
        List<Enemy> enemiesInRange = new List<Enemy>();
        enemiesInRange = GetEnnemiesInRange(weaponRange);
        /*if(enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count - 1; i++)
            {
                enemiesInRange[i].Damage(weaponDamage);
            }
        }*/
    }

    private List<Enemy> GetEnnemiesInRange(int range)
    {
        List<Enemy> ennemiesInRange;
        switch (weaponType)
        {
            case WeaponType.DAGGER:
                switch (direction)
                {
                    case Direction.UP:
                        if(currentTile.topTile != null && currentTile.topTile.isReachable == true)
                        {
                            Tile topTile = currentTile.topTile;
                            StartCoroutine(DebugAttack(topTile));
                        }
                        break;
                    case Direction.RIGHT:
                        if (currentTile.rightTile != null && currentTile.rightTile.isReachable == true)
                        {
                            Tile rightTile = currentTile.rightTile;
                            StartCoroutine(DebugAttack(rightTile));
                        }       
                        break;
                    case Direction.BOTTOM:
                        if (currentTile.bottomTile != null && currentTile.bottomTile.isReachable == true)
                        {
                            Tile bottomTile = currentTile.bottomTile;
                            StartCoroutine(DebugAttack(bottomTile));
                        }
                        break;
                    case Direction.LEFT:
                        if (currentTile.leftTile != null && currentTile.leftTile.isReachable == true)
                        {
                            Tile leftTile = currentTile.leftTile;
                            StartCoroutine(DebugAttack(leftTile));
                        }
                        break;
                }
                ennemiesInRange = null;
                return ennemiesInRange;
            case WeaponType.GRIMOIRE:
                switch (direction)
                {
                    case Direction.UP:
                        if (currentTile.topTile != null)
                        {
                            Tile topTile = currentTile.topTile;
                            if(topTile.leftTile != null && topTile.leftTile.isReachable == true)
                            {
                                Tile topLeftTile = topTile.leftTile;
                                StartCoroutine(DebugAttack(topLeftTile));
                            }
                            if (topTile.rightTile != null && topTile.rightTile.isReachable == true)
                            {
                                Tile topRightTile = topTile.rightTile;
                                StartCoroutine(DebugAttack(topRightTile));
                            }
                            StartCoroutine(DebugAttack(topTile));
                        }
                        break;
                    case Direction.RIGHT:
                        if (currentTile.rightTile != null)
                        {
                            Tile rightTile = currentTile.rightTile;
                            if (rightTile.topTile != null && rightTile.topTile.isReachable == true)
                            {
                                Tile rightUpTile = rightTile.topTile;
                                StartCoroutine(DebugAttack(rightUpTile));
                            }
                            if (rightTile.bottomTile != null && rightTile.bottomTile.isReachable == true)
                            {
                                Tile rightBottomTile = rightTile.bottomTile;
                                StartCoroutine(DebugAttack(rightBottomTile));
                            }
                            StartCoroutine(DebugAttack(rightTile));
                        }
                        break;
                    case Direction.BOTTOM:
                        if (currentTile.bottomTile != null)
                        {
                            Tile bottomTile = currentTile.bottomTile;
                            if (bottomTile.leftTile != null && bottomTile.leftTile.isReachable == true)
                            {
                                Tile bottomLeftTile = bottomTile.leftTile;
                                StartCoroutine(DebugAttack(bottomLeftTile));
                            }
                            if (bottomTile.rightTile != null && bottomTile.rightTile.isReachable == true)
                            {
                                Tile bottomRightTile = bottomTile.rightTile;
                                StartCoroutine(DebugAttack(bottomRightTile));
                            }
                            StartCoroutine(DebugAttack(bottomTile));
                        }
                        break;
                    case Direction.LEFT:
                        if (currentTile.leftTile != null)
                        {
                            Tile leftTile = currentTile.leftTile;
                            if (leftTile.topTile != null && leftTile.topTile.isReachable == true)
                            {
                                Tile leftUpTile = leftTile.topTile;
                                StartCoroutine(DebugAttack(leftUpTile));
                            }
                            if (leftTile.bottomTile != null && leftTile.bottomTile.isReachable == true)
                            {
                                Tile leftBottomTile = leftTile.bottomTile;
                                StartCoroutine(DebugAttack(leftBottomTile));
                            }
                            StartCoroutine(DebugAttack(leftTile));
                        }
                        break;
                }
                ennemiesInRange = null;
                return ennemiesInRange;
            case WeaponType.HANDGUN:
                bool hit = false;
                Tile lastTile;
                switch (direction)
                {
                    case Direction.UP:
                        if (currentTile.topTile != null && currentTile.topTile.isReachable == true)
                        {
                            lastTile = currentTile.topTile;
                            StartCoroutine(DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.topTile;
                                    StartCoroutine(DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.RIGHT:
                        if (currentTile.rightTile != null && currentTile.rightTile.isReachable == true)
                        {
                            lastTile = currentTile.rightTile;
                            StartCoroutine(DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.rightTile;
                                    StartCoroutine(DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.BOTTOM:
                        if (currentTile.bottomTile != null && currentTile.bottomTile.isReachable == true)
                        {
                            lastTile = currentTile.bottomTile;
                            StartCoroutine(DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.bottomTile;
                                    StartCoroutine(DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.LEFT:
                        if (currentTile.leftTile != null && currentTile.leftTile.isReachable == true)
                        {
                            lastTile = currentTile.leftTile;
                            StartCoroutine(DebugAttack(lastTile));
                            for (int i = 1; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.leftTile;
                                    StartCoroutine(DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                }
                ennemiesInRange = null;
                return ennemiesInRange;
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
    public override void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
