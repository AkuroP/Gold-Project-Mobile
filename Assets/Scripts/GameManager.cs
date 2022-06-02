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

    // Start is called before the first frame update
    void Start()
    {
        whatTurn = Turn.PLAYERTURN;
        enemiesPlaying = TriGnome(enemiesPlaying);
        actualDangerousness = 1 + (score / 20);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static List<GameObject> TriGnome(List<GameObject> enemies)
    {
        int index = 1;
        while (index < enemies.Count)
        {
            if (enemies[index].GetComponent<Enemy>().prio < enemies[index - 1].GetComponent<Enemy>().prio)
            {
                GameObject temp = enemies[index];
                enemies[index] = enemies[index - 1];
                enemies[index - 1] = temp;
                if (index > 1)
                {
                    index--;
                }
            }
            else { index++; }
        }
        return enemies;
    }

    public void ChangeTurn()
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
    }
}
