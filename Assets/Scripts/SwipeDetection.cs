using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public Player player;

    private Vector2 startPos;

    public int distanceToDetectSwipe = 50;

    private bool fingerDown;
    public bool swipeAllowed;
    private bool swipeDone;

    private MapBuilder mapBuilder;

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
        mapBuilder = GameObject.FindWithTag("MapBuilder").GetComponent<MapBuilder>();
    }

    void Update()
    {
        
        if(swipeAllowed)
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
                    player.direction = Entity.Direction.UP;
                    swipeDone = true;
                }
                //swipe right
                else if (Input.mousePosition.x >= startPos.x + distanceToDetectSwipe)
                {
                    fingerDown = false;
                    player.direction = Entity.Direction.RIGHT;
                    swipeDone = true;
                }
                //swipe down
                else if (Input.mousePosition.y <= startPos.y - distanceToDetectSwipe)
                {
                    fingerDown = false;
                    player.direction = Entity.Direction.BOTTOM;
                    swipeDone = true;
                }
                //swipe left
                else if (Input.mousePosition.x <= startPos.x - distanceToDetectSwipe)
                {
                    fingerDown = false;
                    player.direction = Entity.Direction.LEFT;
                    swipeDone = true;
                }
            }

            if (fingerDown && Input.GetMouseButtonUp(0))
            {
                fingerDown = false;
            }

            if(swipeDone == true && player.GetComponent<Player>().attackNext == true)
            {
                swipeDone = false;
                swipeAllowed = false;
                player.GetComponent<Player>().Attack();
                player.GetComponent<Player>().attackNext = false;
            }
            else if (swipeDone == true && player.GetComponent<Player>().moveNext == true)
            {
                swipeDone = false;
                swipeAllowed = false;
                mapBuilder.MovePlayer();
                player.GetComponent<Player>().moveNext = false;
            }
        }
    }
}
