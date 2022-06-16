using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCreep : Boss
{
    public BossTP sun;
        
    void Update()
    {
        if (hp <= 0)
        {
            sun.sunCreeps.Remove(this);
            Destroy(this.gameObject);
        }

        if (myTurn)
        { 
            StartTurn();

            hasPlay = true;
        }
    }

    public override void Init()
    {
        maxHP = 1;
        hp = maxHP;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        transform.GetChild(0).transform.position = transform.GetChild(0).transform.parent.transform.position;
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/SunCreep");
    }

    public void StartTurn()
    {
        Debug.Log("sun creep turn");
    }
}
