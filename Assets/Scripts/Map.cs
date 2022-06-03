using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map : MonoBehaviour
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
    

    public void Init(MapSettings _mapSettings)
    {
        mapOrigin = this.gameObject;

        mapHeight = _mapSettings.mapHeight;
        mapWidth = _mapSettings.mapWidth;
        numberOfTiles = mapWidth * mapHeight;

        entranceTileIndex = _mapSettings.entranceTileIndex;
        exitTileIndex = _mapSettings.exitTileIndex;

        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //get settings for this tile
                TileSettings newTileSettings = _mapSettings.tileSettings[tileIndex];

                //instantiate and initialize tile
                GameObject newTileInstance = Instantiate(Resources.Load("Prefabs/Tile"), mapOrigin.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;
                newTileInstance.GetComponent<Tile>().Init(tileIndex, j + 1, i + 1, newTileSettings.isReachable, newTileSettings.isWall, newTileSettings.isHole, newTileSettings.isEnemySpawn, newTileSettings.tileColor);

                if(newTileSettings.isEnemySpawn)
                    enemySpawnTiles.Add(newTileInstance.GetComponent<Tile>());

                tilesList.Add(newTileInstance.GetComponent<Tile>());
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

        SpawnEntities();
    }


    public void SpawnEntities()
    {
        //instantiate and spawn player
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.gameObject.transform.position = tilesList[entranceTileIndex].gameObject.transform.position;

        //assign map and current tile
        player.currentMap = GetComponent<Map>();
        player.currentTile = tilesList[entranceTileIndex];
        tilesList[entranceTileIndex].entityOnTile = player;

        //instantiate and spawn ennemies
        foreach(Tile tile in enemySpawnTiles)
        {
            GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), tile.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;
            int random = Random.Range(0,2);
            //Debug.Log("RANDOM : " + random);
            switch(random)
            {
                case 0:
                    newEnemy.AddComponent<EnemyOne>();
                    newEnemy.GetComponent<EnemyOne>().Init();
                    newEnemy.GetComponent<EnemyOne>().currentMap = GetComponent<Map>();
                break;

                case 1:
                    newEnemy.AddComponent<EnemyTwo>();
                    newEnemy.GetComponent<EnemyTwo>().Init();
                    newEnemy.GetComponent<EnemyTwo>().currentMap = GetComponent<Map>();
                break;

                default:
                    newEnemy.AddComponent<EnemyTwo>();
                    newEnemy.GetComponent<EnemyTwo>().Init();
                    newEnemy.GetComponent<EnemyTwo>().currentMap = GetComponent<Map>();
                break;
            }
            
            newEnemy.GetComponent<Enemy>().currentTile = tile;
            tile.entityOnTile = newEnemy.GetComponent<Enemy>();

            entities.Add(newEnemy.GetComponent<Enemy>());
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

    public Tile FindLeftTopTile(Tile currentTile)
    {
        if(currentTile.tileX == 1 || currentTile.tileY == mapHeight)
            return null;
        else
        {
            int index = currentTile.tileIndex + mapWidth - 1;
            return tilesList[index];
        }
    }

    public Tile FindRightTopTile(Tile currentTile)
    {
        if(currentTile.tileX == mapWidth || currentTile.tileY == mapHeight)
            return null;
        else
        {
            int index = currentTile.tileIndex + mapWidth + 1;
            return tilesList[index];
        }
    }

    public Tile FindLeftBottomTile(Tile currentTile)
    {
        if(currentTile.tileX == 1 || currentTile.tileY == 1)
            return null;
        else
        {
            int index = currentTile.tileIndex - mapWidth - 1;
            return tilesList[index];
        }
    }

    public Tile FindRightBottomTile(Tile currentTile)
    {
        if(currentTile.tileX == mapWidth || currentTile.tileY == 1)
            return null;
        else
        {
            int index = currentTile.tileIndex - mapWidth + 1;
            return tilesList[index];
        }
    }

    public bool CheckMove(Tile nextTile)
    {
        //is there a tile in the direction
        if (nextTile != null)
        {
            //is the tile accessible and empty
            if (nextTile.entityOnTile == null && nextTile.isReachable)
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
}