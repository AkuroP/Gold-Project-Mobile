using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("==== Map Elements ====")]
    public GameObject player;
    private PlayerBehaviour pB;
    public GameObject tilePrefab;

    [Header("==== Map References ====")]
    public List<MapSettings> mapSettings = new List<MapSettings>();

    [Header("==== Maps ====")]
    public Transform mapOrigin;
    public Map currentMap;

    void Start()
    {
        pB = player.GetComponent<PlayerBehaviour>();

        //Create a map
        currentMap = CreateMap();
        InitializeMap(currentMap);

    }

    public Map CreateMap()
    {
        return new Map(mapSettings[1], player, mapOrigin.position);
    }

    public void InitializeMap(Map map)
    {
        //instantiate tiles
        for (int i = 0; i < map.tilesList.Count; i++)
        {
            Tile tile = map.tilesList[i];

            GameObject newTileGO = Instantiate(tilePrefab, map.mapOrigin + new Vector3(tile.tileX, tile.tileY, 0), Quaternion.identity);
            newTileGO.GetComponent<SpriteRenderer>().color = tile.tileColor;

            tile.tileGO = newTileGO;
        }

        //spawn all entities
        map.SpawnEntities();
    }

    void Update()
    {
        if (pB.canMove)
        {
            //ask for up
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Tile nextTile = currentMap.FindTopTile(pB.currentTile);

                if(currentMap.CheckMove(nextTile))
                {
                    pB.MovePlayer(nextTile.tileGO.transform.position);
                    pB.currentTile = nextTile;
                }
            }
            //ask for right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Tile nextTile = currentMap.FindRightTile(pB.currentTile);

                if (currentMap.CheckMove(nextTile))
                {
                    pB.MovePlayer(nextTile.tileGO.transform.position);
                    pB.currentTile = nextTile;
                }
            }
            //ask for bottom
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Tile nextTile = currentMap.FindBottomTile(pB.currentTile);

                if (currentMap.CheckMove(nextTile))
                {
                    pB.MovePlayer(nextTile.tileGO.transform.position);
                    pB.currentTile = nextTile;
                }
            }
            //ask for left
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Tile nextTile = currentMap.FindLeftTile(pB.currentTile);

                if (currentMap.CheckMove(nextTile))
                {
                    pB.MovePlayer(nextTile.tileGO.transform.position);
                    pB.currentTile = nextTile;
                }
            }
        }
    }
}

[System.Serializable]
public class MapSettings
{
    public int mapWidth, mapHeight;
    [SerializeField]
    public List<TileSettings> tileSettings = new List<TileSettings>();
}

[System.Serializable]
public class TileSettings
{
    public bool isReachable;
    public Color tileColor = Color.red;
    public Sprite tileSprite;
}

