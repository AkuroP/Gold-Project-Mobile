using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        InitAttackPattern();
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn)
        {
            AttackParity();
        }
    }

    private void AttackParity()
    {
        if(parity == 0)
        {
            parity = 1;
            Debug.Log("CHARGING");
            hasAttack = true;
            hasMove = true;
        }
        else if(parity == 1)
        {
            Direction dir = CheckAround(true);
            if(dir != Direction.NONE)
            {
                this.direction = dir;
                StartAttack();
            }
            Debug.Log("ATTACK !");
            hasMove = true;
            hasAttack = true;
            parity = 0;
        }
        
    }
}
