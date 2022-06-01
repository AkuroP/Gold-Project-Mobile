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
        currentPosition = transform.position;
        weapon = new Weapon(WeaponType.DAGGER);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instanceGM.whatTurn == GameManager.Turn.PLAYERTURN)
        {
            this.MovingProcess();
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
        if(GameManager.instanceGM.whatTurn == GameManager.Turn.PLAYERTURN)
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
            this.canAttack = false;
            ResetAction();
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
