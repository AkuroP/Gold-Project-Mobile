using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    public Vector3 enemyActualPos;
    public float enemySpeed;
    // Start is called before the first frame update
    void Start()
    {
        enemyActualPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instanceGM.whatTurn == GameManager.Turn.ENEMYTURN)
        {
            Vector3.MoveTowards(enemyActualPos, enemyActualPos + new Vector3(.65f, 0f, 0f), enemySpeed * Time.deltaTime);
            if(this.transform.position == (enemyActualPos + new Vector3(.65f, 0f, 0f)))
            {
                GameManager.instanceGM.whatTurn = GameManager.Turn.PLAYERTURN;
                enemyActualPos = this.transform.position;
            }
        }
    }
    
}
