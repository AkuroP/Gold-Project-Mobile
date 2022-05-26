using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool canMove = true;
    public bool moveInProgress = false;

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
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration);
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
}
