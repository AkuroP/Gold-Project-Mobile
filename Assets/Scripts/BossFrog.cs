using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFrog : Boss
{
    [Header("==== Spit Poison Attack ====")]
    public List<FrogPoisonSpit> poisonSpitList = new List<FrogPoisonSpit>();
    private int poisonSpitDamage = 1;


    [Header("==== Tongue Attack ====")]
    public List<Tile> tongueAttackZone = new List<Tile>();
    private bool tongueAttackInProgress = false;
    private int tongueAttackCD = 1;
    private int tongueDamage = 1;


    // Update is called once per frame
    void Update()
    {

        if (myTurn)
        {
            StartTurn();

            hasPlay = true;
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 1;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        moveCDMax = 1;
        moveCDCurrent = 0;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Boss");

        AssignPattern();

        isInitialize = true;
    }

    public void AssignPattern()
    {
        //tongue Attack
        tongueAttackZone.Add(currentMap.tilesList[31]);
        tongueAttackZone.Add(currentMap.tilesList[38]);
    }

    public override void StartTurn()
    {
        Debug.Log("Frog Turn");

        if(!tongueAttackInProgress)
        {
            tongueAttackInProgress = CheckInTongueRange();

            if (tongueAttackInProgress)
            {
                tongueAttackCD--;
            }
            else
            {
                //keep going
                ThrowSpitPoisonAttack(6);
            }
        }
        else
        {
            if (tongueAttackCD > 0)
            {
                tongueAttackCD--;
            }
            else
            {
                tongueAttackCD = 1;
                tongueAttackInProgress = false;
                StartAttackTongue();
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

        //find random tiles
        for(int i = 0; i < numberOfSpit; i++)
        {
            Tile impactTile = currentMap.ReturnRandomTile();

            while(impactTile.isWall || tempTiles.Contains(impactTile))
            {
                impactTile = currentMap.ReturnRandomTile();
            }

            tempTiles.Add(impactTile);
        }

        //create new spit
        foreach(Tile oneTile in tempTiles)
        {
            if(poisonSpitList.Count < 20)
                poisonSpitList.Add(new FrogPoisonSpit(3, oneTile));
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
