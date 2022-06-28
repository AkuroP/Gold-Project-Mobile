using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : Enemy
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();

    public List<Tile> pathToTarget = new List<Tile>();

    public bool inChase = false;
    public bool chargeAttack = false;
    public int chargeAttackRoundMax = 0;
    public int chargeAttackCurrent;

    private GameObject chasingEye;

    // Start is called before the first frame update
    void Start()
    {
        //test = FindPath(currentTile, currentMap.player.currentTile, false);
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/mob_base");

        if(!GameManager.instanceGM.isX2)
        {
            GameManager.instanceGM.allAnim.Add(this.enemyAnim);
        }
        else
        {
            this.enemyAnim.SetFloat("AnimSpeed", GameManager.instanceGM.animSpeedMultiplier);
            GameManager.instanceGM.allAnim.Add(this.enemyAnim);
        }
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (GameManager.instanceGM.floor >= 19)
        {
            maxHP = 3;
        }
        else if (GameManager.instanceGM.floor >= 7)
        {
            maxHP = 2;
        }
        else
        {
            maxHP = 1;
        }
        hp = maxHP;
        if (GameManager.instanceGM.floor >= 25)
        {
            enemyDamage = 3;
        }
        else if (GameManager.instanceGM.floor >= 13)
        {
            enemyDamage = 2;
        }
        else
        {
            enemyDamage = 1;
        }
        prio = Random.Range(1, 5);
        //InitAttackPattern();
        moveCDMax = 0;
        moveCDCurrent = 0;

        entityDangerousness = 1;

        chargeAttackCurrent = chargeAttackRoundMax;

        moveDuration = 0.4f;
        attackDuration = .35f;

        entitySr = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Mob");
        

        AssignPattern();

        turnArrow = this.transform.Find("Arrow").gameObject;

        if (GameManager.instanceGM.floor >= 19)
        {
            heart1 = this.transform.Find("Heart1").gameObject;
            heart2 = this.transform.Find("Heart2").gameObject;
            heart3 = this.transform.Find("Heart3").gameObject;
        }
        else if (GameManager.instanceGM.floor >= 7)
        {
            heart1 = this.transform.Find("Heart1").gameObject;
            heart2 = this.transform.Find("Heart2").gameObject;
            heart3 = this.transform.Find("Heart3").gameObject;
            heart2.SetActive(false);
        }
        else
        {
            heart1 = this.transform.Find("Heart1").gameObject;
            heart2 = this.transform.Find("Heart2").gameObject;
            heart3 = this.transform.Find("Heart3").gameObject;
            heart1.SetActive(false);
            heart3.SetActive(false);
        }

        

        isInitialize = true;
    }



    private void AssignPattern()
    {
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if(!inChase)
            inChase = IsTargetInChaseRange(currentMap.player.currentTile, 3);

        if (myTurn)
        {
            turnArrow.SetActive(true);
            myTurn = false;
            turnDuration = 0;

            CheckFire();
            StartTurn();
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            //Debug.Log("Move");
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;

            if (currentTile.isPike && !isOnThePike)
            {
                currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_25");
                isOnThePike = true;
                GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/spike");
                GameManager.instanceGM.sfxAudioSource.Play();
                if (hp == 1)
                {
                    StartCoroutine(ResetPike(currentTile));
                }
                else
                {
                    Damage(1, this);
                }
            }
            entitySr.sortingOrder = 11 - this.currentTile.tileY;
        }

        if (isInitialize)
            IsSelfDead();

        if (GameManager.instanceGM.floor >= 19)
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
        else if (GameManager.instanceGM.floor >= 7)
        {
            switch (this.hp)
            {
                case 1:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_0");
                    break;
                case 2:
                    heart1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    heart3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/GA/HUD/hud1_1");
                    break;
                default:
                    break;
            }
        }
    }

    public override void StartTurn()
    {
        if (!chargeAttack)
        {
            dir = CheckAround(upDirectionATS, false);

            if (dir != Direction.NONE)
            {
                //Debug.Log("in range");
                chargeAttack = true;
                direction = dir;
                Debug.Log("charge start");

                if (chargeAttackCurrent > 0)
                {
                    Debug.Log("charge in progress");
                    chargeAttackCurrent--;
                }
                else
                {
                    Debug.Log("attaque");
                    chargeAttack = false;
                    chargeAttackCurrent = chargeAttackRoundMax;

                    StartAttack(upDirectionATS);
                    enemyAnim.SetTrigger("Atk");
                    GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/SFX_Atk_Enemy3");
                    GameManager.instanceGM.sfxAudioSource.Play();
                    turnDuration += attackDuration;
                }
            }
            else
            {
                //Debug.Log("move");
                //chase move
                if (inChase)
                {
                    if(chasingEye == null)
                        chasingEye = Instantiate(Resources.Load<GameObject>("Prefabs/Discovered"), this.transform);
                    if (moveCDCurrent > 0)
                    {
                        moveCDCurrent--;
                    }
                    else
                    {
                        List<Tile> possibleAttackSpot = FindAvailableAttackSpot(upDirectionATS);
                        pathToTarget = FindQuickestPath(currentTile, possibleAttackSpot, false);

                        if (pathToTarget != null && pathToTarget.Count > 1)
                        {
                            enemyAnim.SetTrigger("Move");
                            Move(pathToTarget[1]);
                            turnDuration += moveDuration;
                        }
                            

                        moveCDCurrent = moveCDMax;
                    }
                }
                //random move
                else
                {
                    if(moveCDCurrent > 0)
                    {
                        moveCDCurrent--;
                    }
                    else
                    {
                        enemyAnim.SetTrigger("Move");
                        EnemyRandomMove();
                        turnDuration += moveDuration;
                        moveCDCurrent = moveCDMax;
                    }

                    if(chasingEye != null)
                    {
                        Destroy(chasingEye);
                    }
                }
            }
        }
        else
        {
            if(chargeAttackCurrent > 0)
            {
                Debug.Log("charge");
                chargeAttackCurrent--;
            }
            else
            {
                Debug.Log("attaque");
                chargeAttack = false;
                chargeAttackCurrent = chargeAttackRoundMax;

                direction = dir;
                StartAttack(upDirectionATS);
                enemyAnim.SetTrigger("Atk");
                turnDuration += attackDuration;
            }
        }
        StartCoroutine(EndTurn(turnDuration));
    }
}
