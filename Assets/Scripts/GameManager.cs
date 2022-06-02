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

    private GameObject player;
    [SerializeField] private GameObject[] enemiesPlaying;
    [SerializeField] private GameObject shopUIprefab;
    [SerializeField] private GameObject shopPrefab;

    private void Awake()
    {
        if (instanceGM != null)
        {
            Destroy(instanceGM);
        }
        instanceGM = this;

        player = GameObject.FindWithTag("Player");
        whatTurn = Turn.PLAYERTURN;
        enemiesPlaying = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesPlaying = TriGnome(enemiesPlaying);
        actualDangerousness = 1 + (score / 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (room == 5)
        {
            UI.instanceUI.shopUI = Instantiate(shopUIprefab, UI.instanceUI.canvas.transform);
            Instantiate(shopPrefab);
            UI.instanceUI.shopUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject[] TriGnome(GameObject[] enemies)
    {
        int index = 1;
        while (index < enemies.Length)
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
}
