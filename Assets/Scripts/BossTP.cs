using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTP : Boss
{
    void Update()
    {
        if (myTurn)
        {
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
