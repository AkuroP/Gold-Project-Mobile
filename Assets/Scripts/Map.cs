using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    [Header("==== Map Entities ====")]
    public Player player;
    [SerializeField]
    public List<Entity> entities = new List<Entity>();

    [Header("==== Map Size ====")]
    public GameObject mapOrigin;
    public int mapHeight = 5;
    public int mapWidth = 5;
    private int numberOfTiles = 25;

    [Header("==== Tiles ====")]
    [SerializeField]
    public List<Tile> tilesList = new List<Tile>();
    [SerializeField]
    private List<Tile> enemySpawnTiles = new List<Tile>();
    public int entranceTileIndex, exitTileIndex;
    


    public Map(MapSettings _mapSettings, Player _player, GameObject _mapOrigin)
    {
        //map size
        mapWidth = _mapSettings.mapWidth;
        mapHeight = _mapSettings.mapHeight;
        numberOfTiles = mapWidth * mapHeight;
        mapOrigin = _mapOrigin;
        entranceTileIndex = _mapSettings.entranceTileIndex;

        //player
        player = _player;

        //create map tiles following reference map
        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                //get settings for this tile
                TileSettings newTileSettings = _mapSettings.tileSettings[tileIndex];

                //Instantiate the tile
                Tile newTile = new Tile(tileIndex, j + 1, i + 1, newTileSettings.isReachable, newTileSettings.isEnemySpawn, newTileSettings.tileColor);
                
                if(newTileSettings.isEnemySpawn)
                    enemySpawnTiles.Add(newTile);

                

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
        //spawn player
        player.gameObject.transform.position = tilesList[entranceTileIndex].tileGO.transform.position - new Vector3(0, 0, 1);
        player.currentTile = tilesList[entranceTileIndex];

        //spawn ennemies
        foreach(Tile tile in enemySpawnTiles)
        {
            Enemy newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), tile.tileGO.transform.position, Quaternion.identity) as Enemy;
            entities.Add(newEnemy);
        }
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
                //Debug.Log("case accessible");
                return true;
            }
            else
            {
                //Debug.Log("case inaccessible");
                return false;
            }
        }
        else
        {
            //Debug.Log("il n'y a pas de case");
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