using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public Player player;

    private Vector2 startPos;

    public int distanceToDetectSwipe = 50;

    private bool fingerDown;

    void Update()
    {
        /*if (fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            startPos = Input.touches[0].position;
            fingerDown = true;
        }

        if(fingerDown)
        {
            //swipe up
            if(Input.touches[0].position.y >= startPos.y + distanceToDetectSwipe)
            {
                fingerDown = false;
                Debug.Log("swipe up");
            }
            //swipe left
            else if(Input.touches[0].position.x <= startPos.x + distanceToDetectSwipe)
            {
                fingerDown = false;
                Debug.Log("swipe left");
            }
            //swipe right
            else if(Input.touches[0].position.x >= startPos.x + distanceToDetectSwipe)
            {
                fingerDown = false;
                Debug.Log("swipe left");
            }
        }

        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            fingerDown = false;
        }

        */
        //PC

        if (fingerDown == false && Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            fingerDown = true;
        }

        if (fingerDown)
        {
            //swipe up
            if (Input.mousePosition.y >= startPos.y + distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Entity.Direction.UP;
                Debug.Log("swipe up");
            }
            //swipe right
            else if (Input.mousePosition.x >= startPos.x + distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Entity.Direction.RIGHT;
                Debug.Log("swipe right");
            }
            //swipe down
            else if (Input.mousePosition.y <= startPos.y - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Entity.Direction.BOTTOM;
                Debug.Log("swipe down");
            }
            //swipe left
            else if (Input.mousePosition.x <= startPos.x - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Entity.Direction.LEFT;
                Debug.Log("swipe left");
            }
        }

        if (fingerDown && Input.GetMouseButtonUp(0))
        {
            fingerDown = false;
        }
    }
}
