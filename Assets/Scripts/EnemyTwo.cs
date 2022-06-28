using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwo : Enemy
{
    public List<AttackTileSettings> upDirectionATS1 = new List<AttackTileSettings>();
    public List<AttackTileSettings> upDirectionATS2 = new List<AttackTileSettings>();
    public bool pattern1;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/tentacules4");

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
        pattern1 = true;
        attackDuration = .45f;

        entityDangerousness = 1;

        entitySr = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/tentacule4");

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
        //pattern 1
        upDirectionATS1.Add(new AttackTileSettings(1, 0, 1));
        upDirectionATS1.Add(new AttackTileSettings(1, 1, 0));
        upDirectionATS1.Add(new AttackTileSettings(1, -1, 0));
        upDirectionATS1.Add(new AttackTileSettings(1, 0, -1));

        //pattern 2
        upDirectionATS2.Add(new AttackTileSettings(1, -1, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 1, 1));
        upDirectionATS2.Add(new AttackTileSettings(1, 1, -1));
        upDirectionATS2.Add(new AttackTileSettings(1, -1, -1));
    }

    // Update is called once per frame
    void Update()
    {
        if(this.myTurn)
        {
            turnArrow.SetActive(true);
            myTurn = false;
            turnDuration = 0;

            CheckFire();
            if(moveCDCurrent > 0)
            {
                moveCDCurrent--;
            }
            else
            {
                StartTurn();
                moveCDCurrent = moveCDMax;
            }

            StartCoroutine(EndTurn(turnDuration));
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

        }

        if(isInitialize)
            IsSelfDead();

        entitySr.sortingOrder = 11 - this.currentTile.tileY;

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
        enemyAnim.SetTrigger("Atk");
        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/SFX_Atk_Enemy2");
        GameManager.instanceGM.sfxAudioSource.Play();
        turnDuration += attackDuration;
        if(pattern1 == true)
        {
            dir = CheckAround(upDirectionATS1, false);

            if (dir != Direction.NONE)
            {
                direction = dir;
                StartAttack(upDirectionATS1);
            }
            else
            {
                int random = Random.Range(0,4);
                direction = (Direction)random;
                StartAttack(upDirectionATS1);
            }

        }
        else if (pattern1 == false)
        {
            dir = CheckAround(upDirectionATS2, true);

            if(dir != Direction.NONE)
            {
                direction = dir;
                StartAttack(upDirectionATS2);
            }
            else
            {
                dir = CheckAround(upDirectionATS2, true);
                            
                if(dir != Direction.NONE)
                {
                    direction = dir;
                    StartAttack(upDirectionATS2);
                }
                else
                {
                    int random = Random.Range(0,4);
                    direction = (Direction)random;
                    StartAttack(upDirectionATS2);
                }
            }
        }

        pattern1 = !pattern1;
    }

    /*private void AttackParity()
    {
        //InitAttackPattern();
        Direction dir = CheckAround(true);
        if(dir != Direction.NONE)
        {
            this.direction = dir;
            StartAttack();
        }
        if(this.parity == 0)
        {
            Debug.Log("ATTACK 1");
            this.parity = 1;
            hasAttack = true;
            hasMove = true;
        }
        else if(this.parity == 1)
        {
            Debug.Log("ATTACK 2");
            this.parity = 0;
            hasMove = true;
            hasAttack = true;
        }

    }*/
}
