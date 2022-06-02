using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();
    public int enemyDamage;
    public enum EnemyType
    {
        ENEMY1,
        ENEMY2,
        ENEMY3
    } 
    public EnemyType whatEnemy;
    [SerializeField]private int parity = 0;

    private Player player;
    public float enemyRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn)
        {
            Debug.Log("my turn: " + this.gameObject.name);
            hasMove = true;
        }
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        if (myTurn && Input.GetKeyDown(KeyCode.Z) && !hasAttack)
        {
            StartAttack();
            hasAttack = true;
            EnemyBehaviour();
        }
    }

    private void InitAttackPattern()
    {
        switch(whatEnemy)
        {
            case EnemyType.ENEMY1 :
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
            break;
            case EnemyType.ENEMY3 :
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
            break;
            
        }
    }
    public void EnemyBehaviour()
    {
        switch(whatEnemy)
        {
            case EnemyType.ENEMY1 :
            if(this.parity == 0)
            {
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                this.StartAttack();
                this.parity = 0;
            }
            break;
            case EnemyType.ENEMY2 :
            if(this.parity == 0)
            {
                upDirectionATS.Clear();
                //pattern 1
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 0));
                upDirectionATS.Add(new AttackTileSettings(1, -1, -1));
                upDirectionATS.Add(new AttackTileSettings(1, 0, -1));
                this.parity = 1;
            }
            else if(this.parity == 1)
            {
                upDirectionATS.Clear();
                //pattern 2
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, -1));
            }
            this.StartAttack();
            break;
            case EnemyType.ENEMY3 :
            if(Vector3.Distance(this.transform.position, player.transform.position) > enemyRange)
            {
                this.EnemyMove();
                canAttack = false;
            }
            else
            {
                this.StartAttack();
            }   
            break;
        }
    }
    public void EnemyMove()
    {
        if(whatEnemy == EnemyType.ENEMY3)
        {
            int randomMove = Random.Range(0, 4);
            switch(randomMove)
            {
                case 0:
                    direction = Direction.UP;
                    FindNextTile();
                break;
                case 1:
                    direction = Direction.LEFT;
                    FindNextTile();
                break;
                case 2:
                    direction = Direction.BOTTOM;
                    FindNextTile();
                break;
                case 3:
                    direction = Direction.LEFT;
                    FindNextTile();
                break;
            }
        }
    }

    public override void StartAttack()
    {
        List<AttackTileSettings> attackPattern = ConvertPattern(upDirectionATS, direction);

        List<Entity> enemiesInRange = new List<Entity>();
        enemiesInRange = GetEntityInRange(attackPattern);

        if (enemiesInRange != null && enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] is Player)
                    Damage(enemyDamage, enemiesInRange[i]);
            }
        }
    }

    //function to take damage / die
    /*public override void DamageSelf(int damage)
    {

    }*/
}
