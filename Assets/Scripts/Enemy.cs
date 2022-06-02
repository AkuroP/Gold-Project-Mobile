using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();
    public int enemyDamage;

    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn)
        {
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
