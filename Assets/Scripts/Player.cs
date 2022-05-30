using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
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

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Attack();
        }
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
                        if(this.GetComponent<PlayerBehaviour>().currentTile.topTile != null && this.GetComponent<PlayerBehaviour>().currentTile.topTile.isReachable == true)
                        {
                            Tile topTile = this.GetComponent<PlayerBehaviour>().currentTile.topTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(topTile));
                        }
                        break;
                    case Direction.RIGHT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.rightTile != null && this.GetComponent<PlayerBehaviour>().currentTile.rightTile.isReachable == true)
                        {
                            Tile rightTile = this.GetComponent<PlayerBehaviour>().currentTile.rightTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(rightTile));
                        }       
                        break;
                    case Direction.BOTTOM:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.bottomTile != null && this.GetComponent<PlayerBehaviour>().currentTile.bottomTile.isReachable == true)
                        {
                            Tile bottomTile = this.GetComponent<PlayerBehaviour>().currentTile.bottomTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(bottomTile));
                        }
                        break;
                    case Direction.LEFT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.leftTile != null && this.GetComponent<PlayerBehaviour>().currentTile.leftTile.isReachable == true)
                        {
                            Tile leftTile = this.GetComponent<PlayerBehaviour>().currentTile.leftTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(leftTile));
                        }
                        break;
                }
                ennemiesInRange = null;
                return ennemiesInRange;
            case WeaponType.GRIMOIRE:
                switch (direction)
                {
                    case Direction.UP:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.topTile != null)
                        {
                            Tile topTile = this.GetComponent<PlayerBehaviour>().currentTile.topTile;
                            if(topTile.leftTile != null && topTile.leftTile.isReachable == true)
                            {
                                Tile topLeftTile = topTile.leftTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(topLeftTile));
                            }
                            if (topTile.rightTile != null && topTile.rightTile.isReachable == true)
                            {
                                Tile topRightTile = topTile.rightTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(topRightTile));
                            }
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(topTile));
                        }
                        break;
                    case Direction.RIGHT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.rightTile != null)
                        {
                            Tile rightTile = this.GetComponent<PlayerBehaviour>().currentTile.rightTile;
                            if (rightTile.topTile != null && rightTile.topTile.isReachable == true)
                            {
                                Tile rightUpTile = rightTile.topTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(rightUpTile));
                            }
                            if (rightTile.bottomTile != null && rightTile.bottomTile.isReachable == true)
                            {
                                Tile rightBottomTile = rightTile.bottomTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(rightBottomTile));
                            }
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(rightTile));
                        }
                        break;
                    case Direction.BOTTOM:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.bottomTile != null)
                        {
                            Tile bottomTile = this.GetComponent<PlayerBehaviour>().currentTile.bottomTile;
                            if (bottomTile.leftTile != null && bottomTile.leftTile.isReachable == true)
                            {
                                Tile bottomLeftTile = bottomTile.leftTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(bottomLeftTile));
                            }
                            if (bottomTile.rightTile != null && bottomTile.rightTile.isReachable == true)
                            {
                                Tile bottomRightTile = bottomTile.rightTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(bottomRightTile));
                            }
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(bottomTile));
                        }
                        break;
                    case Direction.LEFT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.leftTile != null)
                        {
                            Tile leftTile = this.GetComponent<PlayerBehaviour>().currentTile.leftTile;
                            if (leftTile.topTile != null && leftTile.topTile.isReachable == true)
                            {
                                Tile leftUpTile = leftTile.topTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(leftUpTile));
                            }
                            if (leftTile.bottomTile != null && leftTile.bottomTile.isReachable == true)
                            {
                                Tile leftBottomTile = leftTile.bottomTile;
                                StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(leftBottomTile));
                            }
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(leftTile));
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
                        if (this.GetComponent<PlayerBehaviour>().currentTile.topTile != null && this.GetComponent<PlayerBehaviour>().currentTile.topTile.isReachable == true)
                        {
                            lastTile = this.GetComponent<PlayerBehaviour>().currentTile.topTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.topTile;
                                    StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.RIGHT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.rightTile != null && this.GetComponent<PlayerBehaviour>().currentTile.rightTile.isReachable == true)
                        {
                            lastTile = this.GetComponent<PlayerBehaviour>().currentTile.rightTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.rightTile;
                                    StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.BOTTOM:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.bottomTile != null && this.GetComponent<PlayerBehaviour>().currentTile.bottomTile.isReachable == true)
                        {
                            lastTile = this.GetComponent<PlayerBehaviour>().currentTile.bottomTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                            for (int i = 0; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.bottomTile;
                                    StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                                    if (lastTile.isReachable == false)
                                    {
                                        hit = true;
                                    }
                                }
                            }
                        }
                        break;
                    case Direction.LEFT:
                        if (this.GetComponent<PlayerBehaviour>().currentTile.leftTile != null && this.GetComponent<PlayerBehaviour>().currentTile.leftTile.isReachable == true)
                        {
                            lastTile = this.GetComponent<PlayerBehaviour>().currentTile.leftTile;
                            StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
                            for (int i = 1; i < 3 + range; i++)
                            {
                                if (hit == false)
                                {
                                    lastTile = lastTile.leftTile;
                                    StartCoroutine(GetComponent<PlayerBehaviour>().DebugAttack(lastTile));
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
