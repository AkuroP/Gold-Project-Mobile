using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetection : MonoBehaviour
{
    private Player player;

    private Vector2 startPos;

    public int distanceToDetectSwipe = 50;

    [SerializeField] private bool fingerDown;
    private bool swipeDone;
    public bool doubleClickTimerOn = false;
    public bool blockInputs = false;

    public float doubleClickTimer = 0f;
    [SerializeField] private float doubleClickInterval = 0.5f;

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
        if(doubleClickTimerOn == true)
        {
            doubleClickTimer += Time.deltaTime;
            if(doubleClickTimer > doubleClickInterval)
            {
                doubleClickTimer = 0f;
                doubleClickTimerOn = false;
            }
        }

        if (fingerDown == false && Input.GetMouseButtonDown(0) && blockInputs == false && EventSystem.current.currentSelectedGameObject != UI.instanceUI.attackButton)
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
                doubleClickTimerOn = false;
                doubleClickTimer = 0;
            }
            //swipe right
            else if (Input.mousePosition.x >= startPos.x + distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.RIGHT;
                swipeDone = true;
                doubleClickTimerOn = false;
                doubleClickTimer = 0;
            }
            //swipe down
            else if (Input.mousePosition.y <= startPos.y - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.BOTTOM;
                swipeDone = true;
                doubleClickTimerOn = false;
                doubleClickTimer = 0;
            }
            //swipe left
            else if (Input.mousePosition.x <= startPos.x - distanceToDetectSwipe)
            {
                fingerDown = false;
                player.direction = Direction.LEFT;
                swipeDone = true;
                doubleClickTimerOn = false;
                doubleClickTimer = 0;
            }
            //Double click if new click arrives in less than "doubleclickinterval" seconds after the last click release
            if(doubleClickTimerOn == true && doubleClickTimer < doubleClickInterval && player.attackNext == false && EventSystem.current.currentSelectedGameObject != UI.instanceUI.attackButton)
            {
                doubleClickTimer = 0f;
                doubleClickTimerOn = false;
                player.hasMove = true;
                player.hasAttack = true;
            }
        }

        if (fingerDown && Input.GetMouseButtonUp(0) && EventSystem.current.currentSelectedGameObject != UI.instanceUI.attackButton)
        {
            fingerDown = false;
            doubleClickTimerOn = true;
            doubleClickTimer = 0f;
        }

        if (swipeDone == true)
        {
            swipeDone = false;
            if (player.attackNext == true)
            {
                //Debug.Log("Attack");
                player.StartAttack(player.weapon.upDirectionATS);
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
