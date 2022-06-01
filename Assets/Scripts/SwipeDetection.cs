using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private Player player;

    private Vector2 startPos;

    public int distanceToDetectSwipe = 50;

    private bool fingerDown;
    private bool swipeDone;

    public static SwipeDetection instanceSD;

    private void Awake()
    {
        if (instanceSD != null)
        {
            Destroy(instanceSD);
        }
        instanceSD = this;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
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
                player.direction = Direction.UP;
                swipeDone = true;
            }
            //swipe right
            else if (Input.mousePosition.x >= startPos.x + distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.RIGHT;
                swipeDone = true;
            }
            //swipe down
            else if (Input.mousePosition.y <= startPos.y - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.BOTTOM;
                swipeDone = true;
            }
            //swipe left
            else if (Input.mousePosition.x <= startPos.x - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.LEFT;
                swipeDone = true;
            }
        }

        if (fingerDown && Input.GetMouseButtonUp(0))
        {
            fingerDown = false;
        }

        if (swipeDone == true)
        {
            swipeDone = false;
            if (player.attackNext == true)
            {
                player.StartAttack();
                player.attackNext = false;
            }
            else
            {
                player.FindNextTile();
                player.moveNext = false;
            }
        }
    }
}
