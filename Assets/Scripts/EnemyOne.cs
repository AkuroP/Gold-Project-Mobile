using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Enemy
{
    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();
    // Start is called before the first frame update
    void Start()
    {
        enemyAnim = this.GetComponentInChildren<Animator>();
        enemyAnim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Assets/GA/Enemies/anims/tentacule");
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 1;
        hp = maxHP;
        enemyDamage = 1;
        prio = 1;
        //InitAttackPattern();
        moveCDMax = 1;
        moveCDCurrent = 0;

        entityDangerousness = 1;

        entitySr = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.LoadAll<Sprite>("Assets/GA/Enemies/TentaculeIdle")[0];

        AssignPattern();

        turnArrow = this.transform.Find("Arrow").gameObject;

        heart1 = this.transform.Find("Heart1").gameObject;
        heart2 = this.transform.Find("Heart2").gameObject;
        heart3 = this.transform.Find("Heart3").gameObject;
        heart1.SetActive(false);
        heart3.SetActive(false);

        isInitialize = true;
    }

    private void AssignPattern()
    {
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if(myTurn)
        {
            turnArrow.SetActive(true);
            turnDuration = 0;
            myTurn = false;
            
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
    }

    public override void StartTurn()
    {
        enemyAnim.SetTrigger("Atk");
        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/SFX_Atk_Enemy1");
        GameManager.instanceGM.sfxAudioSource.Play();
        turnDuration += attackDuration;
        if(this.hp > 0)
        {
            dir = CheckAround(upDirectionATS, false);

            if(dir != Direction.NONE)
            {
                direction = dir;
                StartAttack(upDirectionATS);
            }
            else
            {
                int random = Random.Range(0,3);
                direction = (Direction)random;
                StartAttack(upDirectionATS);
            }
        }

    }

}
