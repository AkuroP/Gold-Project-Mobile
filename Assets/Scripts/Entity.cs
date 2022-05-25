using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Stat")]
    //hp for enemy, turn left for player
    public int maxHP;
    private int hp;
    //priority of entity (player always first)
    public int prio;
    //mobility of entity per turn
    [Header("Movement")]
    public int maxMobility;
    //movement of entity in X and Y
    public int mobilityX;
    public int mobilityY;
    

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void test()
    {

    }
}
