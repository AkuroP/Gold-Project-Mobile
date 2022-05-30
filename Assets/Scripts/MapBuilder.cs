using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("==== Map Elements ====")]
    public Player player;
    private PlayerBehaviour pB;
    public GameObject tilePrefab;

    [Header("==== Map References ====")]
    public List<MapSettings> mapSettings = new List<MapSettings>();

    [Header("==== Maps ====")]
    public GameObject mapOrigin;
    public Map currentMap;

    [Header("==== UI Button ====")]
    public bool enableMove;
    public bool enableAttack;

    public float limitBtwRightLeftAndTopBot;

    void Start()
    {
        //Create a map
        currentMap = CreateMap();
        InitializeMap(currentMap);

    }

    public Map CreateMap()
    {
        return new Map(mapSettings[1], player, mapOrigin);
    }

    public void InitializeMap(Map map)
    {
        //instantiate tiles
        for (int i = 0; i < map.tilesList.Count; i++)
        {
            Tile tile = map.tilesList[i];

            GameObject newTileGO = Instantiate(tilePrefab, map.mapOrigin.transform.position + new Vector3(tile.tileX, tile.tileY, 0), Quaternion.identity, map.mapOrigin.transform);
            newTileGO.GetComponent<SpriteRenderer>().color = tile.tileColor;

            tile.tileGO = newTileGO;
        }

        //spawn all entities
        map.SpawnEntities();
    }

    
}


[System.Serializable]
public class MapSettings
{
    public int mapWidth, mapHeight;
    public int entranceTileIndex, exitTileIndex;
    [SerializeField]
    public List<TileSettings> tileSettings = new List<TileSettings>();
}

[System.Serializable]
public class TileSettings
{
    public bool isReachable;
    public bool isEnemySpawn;
    public Color tileColor = Color.red;
    public Sprite tileSprite;
}

