using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy
{
    public List<AttackTileSettings> upDirectionATS1 = new List<AttackTileSettings>();
    public List<AttackTileSettings> upDirectionATS2 = new List<AttackTileSettings>();
    public bool pattern1;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 1;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        //InitAttackPattern();
        moveCDMax = 0;
        moveCDCurrent = 0;
        pattern1 = true;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Tentacule4");

        AssignPattern();

        isInitialize = true;
    }

    private void AssignPattern()
    {
        //pattern 1
        upDirectionATS1.Add(new AttackTileSettings(1, 0, 1));
        upDirectionATS1.Add(new AttackTileSettings(1, 1, 0));
        upDirectionATS1.Add(new AttackTileSettings(1, -1, 0));
        upDirectionATS1.Add(new AttackTileSettings(1, 0, -1));

        //pattern 2
        upDirectionATS2.Add(new AttackTileSettings(1, -1, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 1, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 1, -1));
        upDirectionATS2.Add(new AttackTileSettings(1, -1, -1));
    }

    // Update is called once per frame
    void Update()
    {
        if(this.myTurn)
        {
            if(this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
                IsSelfDead();
            }
            if(moveCDCurrent > 0)
            {
                moveCDCurrent--;
            }
            else
            {
                StartTurn();
                moveCDCurrent = moveCDMax;
            }
            hasPlay = true;
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            Debug.Log("Move");
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;

        }

        if(isInitialize)
            IsSelfDead();
    }

    public override void StartTurn()
    {
        if(pattern1)
        {
            dir = CheckAround(upDirectionATS1, false);

            if(dir != Direction.NONE)
            {
                direction = dir;
                StartAttack(upDirectionATS1);
            }
            else
            {
                int random = Random.Range(0,4);
                direction = (Direction)random;
                StartAttack(upDirectionATS1);
            }

        }
        else
        {
            dir = CheckAround(upDirectionATS2, true);

            if(dir != Direction.NONE)
            {
                direction = dir;
                StartAttack(upDirectionATS2);
            }
            else
            {
                int random = Random.Range(0,4);
                direction = (Direction)random;
                StartAttack(upDirectionATS2);
            }
        }

        pattern1 = !pattern1;
    }

    /*private void AttackParity()
    {
        //InitAttackPattern();
        Direction dir = CheckAround(true);
        if(dir != Direction.NONE)
        {
            this.direction = dir;
            StartAttack();
        }
        if(this.parity == 0)
        {
            Debug.Log("ATTACK 1");
            this.parity = 1;
            hasAttack = true;
            hasMove = true;
        }
        else if(this.parity == 1)
        {
            Debug.Log("ATTACK 2");
            this.parity = 0;
            hasMove = true;
            hasAttack = true;
        }

    }*/
}
