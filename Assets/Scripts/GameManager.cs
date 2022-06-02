using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceGM;

    //Turns enum and variable to know what turn it is
    public enum Turn
    {
        PLAYERTURN,
        ENEMYTURN
    }
    public Turn whatTurn;

    public int score;
    public int floor;
    public int room;
    public int actualDangerousness;

    [SerializeField] private GameObject shopUIprefab;
    [SerializeField] private GameObject shopPrefab;

    public List<GameObject> enemiesPlaying;
    public int allEnemiesActionFinished;

    private void Awake()
    {
        if (instanceGM != null)
        {
            Destroy(instanceGM);
        }
        instanceGM = this;
    }

    public Map currentMap;
    public List<Entity> allEntities = new List<Entity>();
    public Entity playingEntity;
    public int indexPlayingEntity = 0;

    // Start is called before the first frame update
    void Start()
    {
        actualDangerousness = 1 + (score / 20);
    }

    public void SetUpMapRound(Map _currentMap)
    {
        currentMap = _currentMap;

        //set up all the entities with player in first
        allEntities = TriGnome(_currentMap.entities);
        allEntities.Insert(0, _currentMap.player);

        //player play first
        playingEntity = allEntities[0];
        playingEntity.myTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(playingEntity != null)
        {
            if(playingEntity.hasMove && playingEntity.hasAttack)
            {
                //reset entity who has played
                playingEntity.myTurn = false;
                playingEntity.hasMove = false;
                playingEntity.hasAttack = false;

                //next entity play
                StartCoroutine(ChangeEntity());
            }
        }
        else
        {
            //next entity play
            StartCoroutine(ChangeEntity());
        }
    }

    public IEnumerator ChangeEntity()
    {
        //new entity play
        if (indexPlayingEntity >= allEntities.Count - 1)
        {
            indexPlayingEntity = 0;
        }
        else
        {
            indexPlayingEntity++;
        }
        playingEntity = allEntities[indexPlayingEntity];

        yield return new WaitForSeconds(0.5f);

        playingEntity.myTurn = true;
    }

    public List<Entity> TriGnome(List<Entity> entities)
    {
        int index = 1;
        while (index < entities.Count)
        {
            if (entities[index].prio < entities[index - 1].prio)
            {
                Entity temp = entities[index];
                entities[index] = entities[index - 1];
                entities[index - 1] = temp;
                if (index > 1)
                {
                    index--;
                }
            }
            else { index++; }
        }
        return entities;
    }

    /*public void ChangeTurn()
    {
        if(whatTurn == Turn.PLAYERTURN)
        {
            //ENEMY TURN
            whatTurn = Turn.ENEMYTURN;
            for(int i = 0; i < enemiesPlaying.Count; i++)
            {
                enemiesPlaying[i].GetComponent<Enemy>();
            }
            Debug.Log("RETURN PLAYER TURN");
            whatTurn = Turn.PLAYERTURN;
        }
        else if(whatTurn == Turn.ENEMYTURN)
        {
            //PLAYER TURN
            whatTurn = Turn.PLAYERTURN;
        }
    }*/
}
