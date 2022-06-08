using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFrog : Boss
{
    // Update is called once per frame
    void Update()
    {
        if (myTurn)
        {
            StartTurn();

            myTurn = false;
        }
    }

    public void Init()
    {

    }
}

public struct FrogPoisonSpit
{
    public int turnBeforeImpact;
    public Tile targetTile;

    public FrogPoisonSpit(int _turnBeforeImpact, Tile _targetTile)
    {
        turnBeforeImpact = _turnBeforeImpact;
        targetTile = _targetTile;
    }
}
