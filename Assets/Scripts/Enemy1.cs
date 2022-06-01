using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    public int attackCD;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        if(attackCD == 0)
        {
            Debug.Log("ATTACK !");
            attackCD = 1;
        }
        else if(attackCD == 1)
        {
            attackCD = 0;
        }
    }
}
