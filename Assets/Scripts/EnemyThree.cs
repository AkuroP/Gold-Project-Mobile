using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThree : Enemy
{

    public List<AttackTileSettings> upDirectionATS = new List<AttackTileSettings>();

    public List<Tile> test = new List<Tile>();

    // Start is called before the first frame update
    void Start()
    {
        test = FindPath(currentTile, currentMap.player.currentTile);
    }

    public override void Init()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        maxHP = 1;
        hp = maxHP;
        enemyDamage = 1;
        prio = Random.Range(1, 5);
        //InitAttackPattern();
        maxCD = 0;
        cd = 0;

        entitySr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        entitySr.sprite = Resources.Load<Sprite>("Assets/Graphics/Enemies/Mob");

        AssignPattern();

        isInitialize = true;
    }

    private void AssignPattern()
    {
        //pattern 1
        upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (myTurn)
        {
            if (cd > 0)
            {
                cd--;
            }
            else
            {
                StartTurn();
                cd = maxCD;
            }
            hasPlay = true;
        }

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            Debug.Log("Move");
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;
        }

        if (isInitialize)
            IsSelfDead();
    }

    public override void StartTurn()
    {
            dir = CheckAround(upDirectionATS, true);

            if (dir != Direction.NONE)
            {
                Debug.Log("in range");
            }
            else
            {
                Debug.Log("move");
            }
    }
}
