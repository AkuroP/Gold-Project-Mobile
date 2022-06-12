using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFrog : Boss
{
    public List<Tile> neighbourTiles = new List<Tile>();

    [Header("==== Spit Poison Attack ====")]
    public List<FrogPoisonSpit> poisonSpitList = new List<FrogPoisonSpit>();
    public int poisonSpitCD = 0;
    private int poisonSpitDamage = 1;

    [Header("==== Tongue Attack ====")]
    public List<Tile> tongueAttackZone = new List<Tile>();
    public bool tongueAttackInProgress = false;
    public int tongueAttackCD = 1;
    private int tongueDamage = 1;


    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            BossDeath();

        if (myTurn)
        {
            turnDuration = 0;
            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

            StartTurn();

            hasPlay = true;
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 3;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 0;
        moveCDCurrent = 0;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Crapo");

        AssignPattern();

        isInitialize = true;
    }

    public void AssignPattern()
    {
        //tongue Attack
        tongueAttackZone.Add(currentMap.tilesList[31]);
        tongueAttackZone.Add(currentMap.tilesList[38]);

        //neighbour Tiles
        neighbourTiles.Add(currentMap.tilesList[37]);
        neighbourTiles.Add(currentMap.tilesList[39]);
        neighbourTiles.Add(currentMap.tilesList[43]);
        neighbourTiles.Add(currentMap.tilesList[47]);
        neighbourTiles.Add(currentMap.tilesList[50]);
        neighbourTiles.Add(currentMap.tilesList[54]);
        neighbourTiles.Add(currentMap.tilesList[57]);
        neighbourTiles.Add(currentMap.tilesList[61]);
        neighbourTiles.Add(currentMap.tilesList[65]);
        neighbourTiles.Add(currentMap.tilesList[66]);
        neighbourTiles.Add(currentMap.tilesList[67]);
    }

    public override void StartTurn()
    {
        if(!tongueAttackInProgress)
            tongueAttackInProgress = CheckInTongueRange();

        if(tongueAttackInProgress)
        {
            if(tongueAttackCD > 0)
            {
                Debug.Log("charge");
                tongueAttackCD--;
            }
            else
            {
                Debug.Log("attaque");
                StartAttackTongue();
                tongueAttackCD = 1;
                tongueAttackInProgress = false;

                poisonSpitCD = 0;
            }
        }
        else
        {
            if(poisonSpitCD > 0)
            {
                poisonSpitCD--;
            }
            else
            {
                Debug.Log("throw");
                ThrowSpitPoisonAttack(6);
                poisonSpitCD = 1;
            }
        }

        UpdatePoisonSpitFalls();
    }

    public bool CheckInTongueRange()
    {
        foreach(Tile tile in tongueAttackZone)
        {
            if(tile.entityOnTile != null && tile.entityOnTile is Player)
            {
                return true;
            }
        }

        return false;
    }

    public void StartAttackTongue()
    {
        foreach (Tile tile in tongueAttackZone)
        {
            StartCoroutine(ShowTile(tile, 0));
        }

        if (tongueAttackZone.Contains(player.currentTile))
        {
            Damage(tongueDamage, player);
        }
    }

    public void ThrowSpitPoisonAttack(int numberOfSpit)
    {
        List<Tile> tempTiles = new List<Tile>();
        int timeBeforeImpact = 3;

        //target player if he is near but not attackable
        if (neighbourTiles.Contains(player.currentTile))
        {
            tempTiles.Add(player.currentTile);
            timeBeforeImpact = 2;
            numberOfSpit--;
        }

        //find random tiles
        for(int i = 0; i < numberOfSpit; i++)
        {
            Tile impactTile = currentMap.ReturnRandomTile();

            while(impactTile.isWall || tempTiles.Contains(impactTile) || impactTile.entityOnTile is Boss)
            {
                impactTile = currentMap.ReturnRandomTile();
            }

            tempTiles.Add(impactTile);
        }

        //create new spit
        foreach(Tile oneTile in tempTiles)
        {
                poisonSpitList.Add(new FrogPoisonSpit(timeBeforeImpact, oneTile));
        }
    }

    public void UpdatePoisonSpitFalls()
    {
        for(int i = 0;i < poisonSpitList.Count; i++)
        {
            FrogPoisonSpit currentFPG = poisonSpitList[i];

            currentFPG.turnBeforeImpact--;

            if(currentFPG.turnBeforeImpact == 1)
            {
                StartCoroutine(currentFPG.targetTile.TurnColor(new Color(0f, 1f, 0f, 1f), 0));
            }

            if (currentFPG.turnBeforeImpact == 0)
            {
                StartSpitPoisonAttack(currentFPG);
                poisonSpitList.Remove(currentFPG);
                i--;
            }
        }
    }

    public void StartSpitPoisonAttack(FrogPoisonSpit _fpg)
    {
        StartCoroutine(ShowTile(_fpg.targetTile, 0));

        if (_fpg.targetTile.entityOnTile != null && _fpg.targetTile.entityOnTile is Player)
        {
            Damage(poisonSpitDamage, _fpg.targetTile.entityOnTile);
        }
    }
}

[System.Serializable]
public class FrogPoisonSpit
{
    public int turnBeforeImpact;
    public Tile targetTile;

    public FrogPoisonSpit(int _turnBeforeImpact, Tile _targetTile)
    {
        turnBeforeImpact = _turnBeforeImpact;
        targetTile = _targetTile;
    }
}





/*public override void StartTurn()
{
    Debug.Log("Frog Turn");

    if (moveCDCurrent > 0)
    {
        if (tongueAttackInProgress)
        {
            if (tongueAttackCD > 0)
            {
                tongueAttackCD--;
            }
            else
            {
                tongueAttackCD = 1;
                tongueAttackInProgress = false;
                Debug.Log("attack");
                StartAttackTongue();
            }
        }
        else
        {
            moveCDCurrent--;
        }
    }
    else
    {
        if (!tongueAttackInProgress)
        {
            tongueAttackInProgress = CheckInTongueRange();

            if (tongueAttackInProgress)
            {
                tongueAttackCD--;
            }
            else
            {
                //keep going
                Debug.Log("throw");
                ThrowSpitPoisonAttack(6);
            }

            moveCDCurrent = moveCDMax;
        }
        else
        {
            if (tongueAttackCD > 0)
            {
                tongueAttackCD--;
                moveCDCurrent = moveCDMax;
            }
            else
            {
                tongueAttackCD = 1;
                tongueAttackInProgress = false;
                Debug.Log("attack");
                StartAttackTongue();
                moveCDCurrent = 0;
            }
        }


    }

    UpdatePoisonSpitFalls();
}*/
