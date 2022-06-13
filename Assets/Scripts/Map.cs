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
    public List<Tile> enemySpawnTiles = new List<Tile>();
    public int entranceTileIndex, exitTileIndex;
    public bool canExit = true;

    public void Init(MapSettings _mapSettings, bool _spawnBoss = false, int _bossNumber = 0, bool _canExit = true)
    {
        mapOrigin = this.gameObject;

        mapHeight = _mapSettings.mapHeight;
        mapWidth = _mapSettings.mapWidth;
        numberOfTiles = mapWidth * mapHeight;

        entranceTileIndex = _mapSettings.entranceTileIndex;
        exitTileIndex = _mapSettings.exitTileIndex;

        canExit = _canExit;

        int tileIndex = 0;
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //get settings for this tile
                TileSettings newTileSettings = _mapSettings.tileSettings[tileIndex];

                //instantiate and initialize tile
                GameObject newTileInstance = Instantiate(Resources.Load("Prefabs/Tile"), mapOrigin.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;
                newTileInstance.GetComponent<Tile>().Init(tileIndex, j + 1, i + 1, newTileSettings.isReachable, newTileSettings.isWall, newTileSettings.isHole, newTileSettings.isPike, newTileSettings.isEnemySpawn, newTileSettings.isLight, newTileSettings.isShop, newTileSettings.tileSprite, newTileSettings.upSprite, newTileSettings.tileColor);

                if(newTileSettings.isEnemySpawn)
                    enemySpawnTiles.Add(newTileInstance.GetComponent<Tile>());

                if(tileIndex == exitTileIndex && !_spawnBoss)
                {
                    newTileInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_23");
                }
                else if (tileIndex == exitTileIndex && _spawnBoss)
                {
                    newTileInstance.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_7");
                }

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



        if(_spawnBoss)
        {
            switch (_bossNumber)
            {
                case 0:
                    SpawnPlayer();
                    SpawnFrog();
                    break;

                case 1:
                    SpawnPlayer();
                    SpawnSunCreep(SpawnSun());
                    break;
            }
        }
        else
        {
            SpawnPlayer();
            SpawnEntities();
        }
    }

    public void SpawnPlayer()
    {
        //instantiate and spawn player
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Debug.Log("entrance tile : " + entranceTileIndex);
        player.gameObject.transform.position = tilesList[entranceTileIndex].gameObject.transform.position;

        //assign map and current tile
        player.currentMap = GetComponent<Map>();
        player.currentTile = tilesList[entranceTileIndex];
        tilesList[entranceTileIndex].entityOnTile = player;
    }

    public void SpawnEntities()
    {
        //instantiate and spawn ennemies
        foreach(Tile tile in enemySpawnTiles)
        {
            GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), tile.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;
            int random = Mathf.Clamp(GameManager.instanceGM.actualDangerousness, 1, 3);
            int randomDangerousness = Random.Range(1, random + 1);
            int randomEnemy = 0;

            switch(randomDangerousness)
            {
                case 1:

                    randomEnemy = Random.Range(0, 3);
                    switch(randomEnemy)
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
                        case 2:
                            newEnemy.AddComponent<EnemyThree>();
                            newEnemy.GetComponent<EnemyThree>().Init();
                            newEnemy.GetComponent<EnemyThree>().currentMap = GetComponent<Map>();
                            break;
                    }
                    break;

                case 2:
                    randomEnemy = Random.Range(0, 2);
                    switch (randomEnemy)
                    {
                        case 0:
                            newEnemy.AddComponent<EnemyFour>();
                            newEnemy.GetComponent<EnemyFour>().Init();
                            newEnemy.GetComponent<EnemyFour>().currentMap = GetComponent<Map>();
                            break;
                        case 1:
                            newEnemy.AddComponent<EnemyFive>();
                            newEnemy.GetComponent<EnemyFive>().Init();
                            newEnemy.GetComponent<EnemyFive>().currentMap = GetComponent<Map>();
                            break;
                    }
                    break;

                case 3:
                    newEnemy.AddComponent<EnemySix>();
                    newEnemy.GetComponent<EnemySix>().Init();
                    newEnemy.GetComponent<EnemySix>().currentMap = GetComponent<Map>();
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

    public void SpawnFrog()
    {
        Tile tile = enemySpawnTiles[0];
        GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), tile.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;

        newEnemy.AddComponent<BossFrog>();
        newEnemy.GetComponent<BossFrog>().currentMap = GetComponent<Map>();
        newEnemy.GetComponent<BossFrog>().Init();

        newEnemy.GetComponent<Enemy>().currentTile = tile;
        tile.entityOnTile = newEnemy.GetComponent<Enemy>();

        //Frog is 3x3 so fill the tiles around
        if (tile.topTile != null)
            tile.topTile.entityOnTile = newEnemy.GetComponent<Enemy>();
        if (tile.rightTile != null)
            tile.rightTile.entityOnTile = newEnemy.GetComponent<Enemy>();
        if (tile.bottomTile != null)
            tile.bottomTile.entityOnTile = newEnemy.GetComponent<Enemy>();
        if (tile.leftTile != null)
            tile.leftTile.entityOnTile = newEnemy.GetComponent<Enemy>();

        if (FindRightTopTile(tile) != null)
            FindRightTopTile(tile).entityOnTile = newEnemy.GetComponent<Enemy>();
        if (FindRightBottomTile(tile) != null)
            FindRightBottomTile(tile).entityOnTile = newEnemy.GetComponent<Enemy>();
        if (FindLeftTopTile(tile) != null)
            FindLeftTopTile(tile).entityOnTile = newEnemy.GetComponent<Enemy>();
        if (FindLeftBottomTile(tile) != null)
            FindLeftBottomTile(tile).entityOnTile = newEnemy.GetComponent<Enemy>();

        entities.Add(newEnemy.GetComponent<Enemy>());
    }

    public BossTP SpawnSun()
    {
        Tile tile = enemySpawnTiles[2];
        GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), tile.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;

        newEnemy.AddComponent<BossTP>();
        newEnemy.GetComponent<BossTP>().currentMap = GetComponent<Map>();
        newEnemy.GetComponent<BossTP>().Init();

        newEnemy.GetComponent<Enemy>().currentTile = tile;
        tile.entityOnTile = newEnemy.GetComponent<Enemy>();

        entities.Add(newEnemy.GetComponent<Enemy>());

        return newEnemy.GetComponent<BossTP>();
    }
    
    public void SpawnSunCreep(BossTP _bossTP)
    {
        List<SunCreep> tempSunCreeps = new List<SunCreep>();

        foreach(Tile sunCreepTile in enemySpawnTiles)
        {
            if(sunCreepTile.entityOnTile == null)
            {
                GameObject newEnemy = GameObject.Instantiate(Resources.Load("Prefabs/Enemy"), sunCreepTile.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;

                newEnemy.AddComponent<SunCreep>();
                newEnemy.GetComponent<SunCreep>().currentMap = GetComponent<Map>();
                newEnemy.GetComponent<SunCreep>().Init();
                newEnemy.GetComponent<SunCreep>().sun = _bossTP;

                newEnemy.GetComponent<Enemy>().currentTile = sunCreepTile;
                sunCreepTile.entityOnTile = newEnemy.GetComponent<Enemy>();

                entities.Add(newEnemy.GetComponent<Enemy>());
                tempSunCreeps.Add(newEnemy.GetComponent<SunCreep>());

                newEnemy.transform.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 11 - newEnemy.GetComponent<Enemy>().currentTile.tileY;
            }
        }

        _bossTP.sunCreeps = tempSunCreeps;
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

    public Tile ReturnRandomTile()
    {
        return tilesList[Random.Range(0, mapHeight * mapWidth - 1)];
    }

    public bool CheckMove(Tile nextTile, Entity entity)
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
