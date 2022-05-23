using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int moveMaxCD;
    private int moveCD;
    private Vector3 playerActualPos;
    public float playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        playerActualPos = this.transform.position;
        moveCD = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerMove(6);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerMove(4);

        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerMove(8);

        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerMove(2);
        }
    }

    private void PlayerMove(int move)
    {
        if(moveCD <= 0)
        {
            switch(move)
            {
                case 6:
                    this.transform.position = new Vector3(this.transform.position.x + 0.65f, this.transform.position.y, this.transform.position.z);
                    /*Vector2.MoveTowards(playerActualPos, playerActualPos + new Vector3(.5f, 0f, 0f), playerSpeed * Time.deltaTime);
                    if(this.transform.position == playerActualPos + new Vector3(.5f, 0f, 0f))
                    {
                        moveCD = moveMaxCD;
                        StartCoroutine("Timer");
                    }*/
                break;
                case 4:
                    this.transform.position = new Vector3(this.transform.position.x - 0.65f, this.transform.position.y, this.transform.position.z);
                break;
                case 8:
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.65f, this.transform.position.z);
                break;
                case 2:
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.65f, this.transform.position.z);
                break;
            }
            GameManager.instanceGM.whatTurn = GameManager.Turn.ENEMYTURN;
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(moveMaxCD);
        Debug.Log("ui");
        moveCD = 0;
    }
}
