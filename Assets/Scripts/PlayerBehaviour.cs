using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool canMove = true;
    public bool moveInProgress = false;

    public Map currentMap;
    public Tile currentTile;
    public Vector3 targetPosition, currentPosition;

    float timeElapsed;
    public float moveDuration;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       

        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;
        }
    }

    public void MovePlayer(Vector3 _targetPosition)
    {
        currentPosition = transform.position;
        targetPosition = _targetPosition;

        moveInProgress = true;
        canMove = false;
    }

    //draw attack zone
    public IEnumerator DebugAttack(Tile tile)
    {
        Color oldColor = tile.tileColor;
        tile.tileGO.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        tile.tileGO.GetComponent<SpriteRenderer>().color = oldColor;
    }
}
