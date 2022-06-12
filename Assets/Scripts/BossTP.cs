using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTP : Boss
{
    void Update()
    {
        if (myTurn)
        {
            turnDuration = 0;
            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

            StartTurn();

            myTurn = false;
        }
    }
}

public class Pillar
{
    Tile pilarTile;
    int pilarHP;

    public Pillar(Tile _pilarTile, int _pilarHP)
    {
        pilarHP = _pilarHP;
        pilarTile = _pilarTile;
    }
}
