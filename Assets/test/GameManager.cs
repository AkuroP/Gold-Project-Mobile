using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceGM;

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
    // Start is called before the first frame update
    void Start()
    {
        whatTurn = Turn.PLAYERTURN;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
