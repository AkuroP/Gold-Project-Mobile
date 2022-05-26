using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    [Header("==== Map Entities ====")]
    public GameObject player;
    [SerializeField]
    public List<string> entities = new List<string>();

    [Header("==== Map Size ====")]
    public Vector3 mapOrigin;
    public int mapHeight = 5;
    public int mapWidth = 5;
    private int numberOfTiles = 25;

    [Header("==== Tiles ====")]
    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();
    public int playerSpawnIndex = 0;

    
    public Map(MapSettings _mapSettings)
    {
        //map size
        mapWidth = _mapSettings.mapWidth;
        mapHeight = _mapSettings.mapHeight;
        numberOfTiles = mapWidth * mapHeight;

        //create map tiles following reference map
        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Tile newTile = new Tile(tileIndex, j + 1, i + 1);

                tilesList.Add(newTile);
                tileIndex++;
            }
        }
    }

    public Map(int _mapWidth, int _mapHeight, GameObject _player, Vector3 _mapOrigin)
    {
        //map size
        mapWidth = _mapWidth;
        mapHeight = _mapHeight;
        numberOfTiles = mapWidth * mapHeight;
        mapOrigin = _mapOrigin;

        //player
        player = _player;

        //create map tiles following reference map
        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Tile newTile = new Tile(tileIndex, j + 1, i + 1);

                tilesList.Add(newTile);
                tileIndex++;
            }
        }

        //link tiles together
        for (int i = 0; i < tilesList.Count; i++)
        {
            Tile tempTT, tempRT, tempBT, tempLT;
            tempTT = null;
            tempRT = null;
            tempBT = null;
            tempLT = null;

            Tile oneTile = tilesList[i];

            if (oneTile.tileX > 1)
            {
                tempLT = tilesList[i - 1];
            }

            if (oneTile.tileX < mapWidth)
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
        }
    }


    public void SpawnEntities()
    {
        player.transform.position = tilesList[playerSpawnIndex].tileGO.transform.position - new Vector3(0, 0, 1);
        player.GetComponent<PlayerBehaviour>().currentTile = tilesList[playerSpawnIndex];
    }

    public Tile FindTopTile(Tile currentTile)
    {
        return currentTile.topTile;
    }

    public Tile FindBottomTile(Tile currentTile)
    {
        return currentTile.bottomTile;
    }

    public Tile FindRightTile(Tile currentTile)
    {
        return currentTile.rightTile;
    }

    public Tile FindLeftTile(Tile currentTile)
    {
        return currentTile.leftTile;
    }

    public bool CheckMove(Tile nextTile)
    {
        //is there a tile in the direction
        if (nextTile != null)
        {
            //is the tile accessible and empty
            if (!nextTile.hasEntity && nextTile.isReachable)
            {
                Debug.Log("case accessible");
                return true;
            }
            else
            {
                Debug.Log("case inaccessible");
                return false;
            }
        }
        else
        {
            Debug.Log("il n'y a pas de case");
            return false;
        }

    }

    

    /*private void ChangeStyle(Tile tile)
    {
        if (tile != null)
        {
            if (!tile.isReachable)
            {
                tile.tileGO.GetComponent<SpriteRenderer>().material.color = Color.black;
            }
        }
    }*/
}