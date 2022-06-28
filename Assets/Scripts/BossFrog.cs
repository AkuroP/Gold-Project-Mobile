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
    private GameObject poisonGO;
    private List<GameObject> poisoninTiles = new List<GameObject>();

    [Header("==== Tongue Attack ====")]
    public List<Tile> tongueAttackZone = new List<Tile>();
    public bool tongueAttackInProgress = false;
    public int tongueAttackCD = 1;
    private int tongueDamage = 1;
    public float tongueAttackDuration = 0.3f;
    public float splitDuration = 0.1f;



    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            BossDeath();

        if (myTurn)
        {
            myTurn = false;
            turnDuration = 0;

            if (this.entityStatus.Count > 0)
            {
                this.CheckStatus(this);
            }

            CheckFire();
            
            StartTurn();
            StartCoroutine(EndTurn(turnDuration));
        }

        if (GameManager.instanceGM.floor >= 23)
        {
            switch (this.hp)
            {
                case 1:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 2:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 3:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                case 4:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud_goldheart");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                case 5:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud_goldheart");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud_goldheart");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                default:
                    break;
            }
        }
        else if (GameManager.instanceGM.floor >= 11)
        {
            switch (this.hp)
            {
                case 1:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 2:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 3:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                case 4:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud_goldheart");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (this.hp)
            {
                case 1:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 2:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 3:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                default:
                    break;
            }
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (GameManager.instanceGM.floor >= 23)
        {
            maxHP = 5;
        }
        else if (GameManager.instanceGM.floor >= 11)
        {
            maxHP = 4;
        }
        else
        {
            maxHP = 3;
        }
        hp = maxHP;
        if (GameManager.instanceGM.floor >= 25)
        {
            enemyDamage = 3;
        }
        else if (GameManager.instanceGM.floor >= 17)
        {
            enemyDamage = 2;
        }
        else
        {
            enemyDamage = 1;
        }
        prio = Random.Range(1, 5);
        moveCDMax = 0;
        moveCDCurrent = 0;
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/frog");

        if(!GameManager.instanceGM.isX2)
        {
            GameManager.instanceGM.allAnim.Add(this.enemyAnim);
        }
        else
        {
            this.enemyAnim.SetFloat("AnimSpeed", GameManager.instanceGM.animSpeedMultiplier);
            GameManager.instanceGM.allAnim.Add(this.enemyAnim);
        }

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Crapo");
        poisonGO = Resources.Load<GameObject>("Prefabs/FrogPoison");

        AssignPattern();

        isInitialize = true;

        turnArrow = this.transform.Find("Arrow").gameObject;

        heart1 = this.transform.Find("Heart1").gameObject;
        heart2 = this.transform.Find("Heart2").gameObject;
        heart3 = this.transform.Find("Heart3").gameObject;
    }

    public void AssignPattern()
    {
        //tongue Attack
        tongueAttackZone.Add(currentMap.tilesList[31]);
        tongueAttackZone.Add(currentMap.tilesList[38]);
        tongueAttackZone.Add(currentMap.tilesList[24]);

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
                enemyAnim.SetTrigger("Charge");
                tongueAttackCD--;
            }
            else
            {
                Debug.Log("attaque");
                StartAttackTongue();
                turnDuration += tongueAttackDuration;
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
                turnDuration += splitDuration;
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
        enemyAnim.SetTrigger("Atk_tongue");
        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/SFX_Atk_Boss1-2");
        GameManager.instanceGM.sfxAudioSource.Play();
        foreach (Tile tile in tongueAttackZone)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/FrogTongue"), tile.transform);
            StartCoroutine(ShowTile(tile, 0));
        }

        if (tongueAttackZone.Contains(player.currentTile))
        {
            Damage(tongueDamage, player);
            player.playerAnim.SetTrigger("Hurt");
            if (Inventory.instanceInventory.HasItem("Counter Ring"))
            {
                player.damageMultiplicator = 2;
            }
        }
    }

    public void ThrowSpitPoisonAttack(int numberOfSpit)
    {
        enemyAnim.SetTrigger("Atk_poison");
        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/SFX_Atk_Boss1-1");
        GameManager.instanceGM.sfxAudioSource.Play();
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

            while(impactTile.isWall || tempTiles.Contains(impactTile) || impactTile.entityOnTile is Boss || impactTile.isHole)
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

        //SpawnPoison();
    }


    public void UpdatePoisonSpitFalls()
    {
        turnDuration += splitDuration;
        for (int i = 0;i < poisonSpitList.Count; i++)
        {
            FrogPoisonSpit currentFPG = poisonSpitList[i];
            
            currentFPG.turnBeforeImpact--;

            if(currentFPG.turnBeforeImpact == 1)
            {
                //StartCoroutine(currentFPG.targetTile.TurnColor(new Color(0f, 1f, 0f, 1f), 0));
                GameObject poison = Instantiate(poisonGO, currentFPG.targetTile.transform);
                poisoninTiles.Add(poison);
            }

            if (currentFPG.turnBeforeImpact == 0)
            {
                GameObject currentPoison = currentFPG.targetTile.gameObject.GetComponentInChildren<AnimDestruct>().gameObject;
                StartSpitPoisonAttack(currentFPG, currentPoison.GetComponent<Animator>());
                poisonSpitList.Remove(currentFPG);

                poisoninTiles.Remove(currentPoison);
                i--;
            }
        }
    }

    public void StartSpitPoisonAttack(FrogPoisonSpit _fpg, Animator _poisonAnim)
    {
        _poisonAnim.SetBool("poisoned", true);
        StartCoroutine(ShowTile(_fpg.targetTile, 0));

        if (_fpg.targetTile.entityOnTile != null && _fpg.targetTile.entityOnTile is Player)
        {
            Damage(poisonSpitDamage, _fpg.targetTile.entityOnTile);
            player.playerAnim.SetTrigger("Hurt");
            if (Inventory.instanceInventory.HasItem("Counter Ring"))
            {
                player.damageMultiplicator = 2;
            }
        }
    }

    public override void BossDeath()
    {
        Debug.Log("RESET POISON");
        if(poisoninTiles.Count > 0)
        {
            for(int i = 0; i < poisoninTiles.Count; i++)
            {
                GameObject poison = poisoninTiles[i];
                poisoninTiles.Remove(poison);
                poison.GetComponent<AnimDestruct>().DestroyAnimGO();
                i--;
            }
        }
        poisonSpitList.Clear();
        base.BossDeath();
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
