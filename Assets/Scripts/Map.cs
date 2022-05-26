using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject player;

    

    [Header("Map Size")]
    public int mapHeight;
    public int mapWidth;

    [Header("Tiles")]
    [SerializeField]
    public List<Sprite> tileSprites;

    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();

    public PlayerBehaviour playerB; 

    // Start is called before the first frame update
    void Start()
    {
        playerB = player.GetComponent<PlayerBehaviour>();

        //generate map
        //i --> vertical axe
        //j --> horizontal axe
        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                Tile newTile = new Tile(tileIndex, j + 1, i + 1, tilePrefab);

                tilesList.Add(newTile);
                tileIndex++;
            }
        }

        

        //link tiles and instantiate map
        for (int i = 0; i < tilesList.Count; i++)
        {
            Tile tempTT, tempRT, tempBT, tempLT;
            tempTT = null;
            tempRT = null;
            tempBT = null;
            tempLT = null;

            Tile oneTile = tilesList[i];
           
            if(oneTile.tileX > 1)
            {
                tempLT = tilesList[i - 1];
            }

            if(oneTile.tileX < mapWidth)
            {
                tempRT = tilesList[i + 1];
            }

            if (oneTile.tileY > 1)
            {
                tempBT = tilesList[i - mapWidth];
            }

            if (oneTile.tileY < mapHeight)
            {
                tempTT = tilesList[i + mapWidth];
            }

            oneTile.topTile = tempTT;
            oneTile.rightTile = tempRT;
            oneTile.bottomTile = tempBT;
            oneTile.leftTile = tempLT;

            GameObject newTileGO = Instantiate(oneTile.tileGO, transform.position + new Vector3(oneTile.tileX - 1, oneTile.tileY - 1, 0), Quaternion.identity);
            newTileGO.AddComponent<TileBehaviour>();
            newTileGO.GetComponent<TileBehaviour>().tileIndex = oneTile.tileIndex;

            oneTile.tileGO = newTileGO;
        }

        //place player
        Vector3 spawnTilePos = tilesList[5].tileGO.transform.position;
        player.transform.position = spawnTilePos - new Vector3(0, 0, 1);

        playerB.currentTile = tilesList[5];
    }

    // Update is called once per frame
    void Update()
    {
        if (playerB.canMove)
        {
            //up
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Tile nextTile = FindTopTile(playerB.currentTile);

                CheckMove(nextTile);
            }   
            //up
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Tile nextTile = FindRightTile(playerB.currentTile);

                CheckMove(nextTile);
            }
            //up
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Tile nextTile = FindBottomTile(playerB.currentTile);

                CheckMove(nextTile);
            }
            //up
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Tile nextTile = FindLeftTile(playerB.currentTile);

                CheckMove(nextTile);
            }
        }
    }

    private Tile FindTopTile(Tile currentTile)
    {
            return currentTile.topTile;
    }

    private Tile FindBottomTile(Tile currentTile)
    {
            return currentTile.bottomTile;
    }

    private Tile FindRightTile(Tile currentTile)
    {
            return currentTile.rightTile;
    }

    private Tile FindLeftTile(Tile currentTile)
    {
            return currentTile.leftTile;
    }

    private void CheckMove(Tile nextTile)
    {
        //is there a tile in the direction
        if (nextTile != null)
        {
            Debug.Log("il y a une case");
            
            //is the tile accessible and empty
            if(!nextTile.hasEntity && nextTile.isReachable)
            {
                Debug.Log("case accessible"); 

                playerB.MovePlayer(nextTile.tileGO.transform.position - new Vector3(0, 0, 1));
                playerB.currentTile = nextTile;
            }
            else
            {
                Debug.Log("case inaccessible");
            }
        }
        else
        {
            Debug.Log("il n'y a pas de case");
        }
    }
}