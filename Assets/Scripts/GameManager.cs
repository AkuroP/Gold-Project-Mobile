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

    private void Awake()
    {
        if(instanceGM != null)
        {
            Destroy(instanceGM);
        }
        instanceGM = this;
    }

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        whatTurn = Turn.PLAYERTURN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
