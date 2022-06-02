using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy
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
        
    }
}
