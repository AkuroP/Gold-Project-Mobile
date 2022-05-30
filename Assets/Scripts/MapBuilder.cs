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

    [Header("==== UI Button ====")]
    public bool enableMove;
    public bool enableAttack;

    public float limitBtwRightLeftAndTopBot;

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

    public void MovePlayer()
    {
        switch (player.GetComponent<Player>().direction)
        {
            case Entity.Direction.UP:
                Tile topTile = currentMap.FindTopTile(pB.currentTile);
                if (currentMap.CheckMove(topTile))
                {
                    pB.MovePlayer(topTile.tileGO.transform.position);
                    pB.currentTile = topTile;
                }
                break;
            case Entity.Direction.RIGHT:
                Tile rightTile = currentMap.FindRightTile(pB.currentTile);
                if (currentMap.CheckMove(rightTile))
                {
                    pB.MovePlayer(rightTile.tileGO.transform.position);
                    pB.currentTile = rightTile;
                }
                break;
            case Entity.Direction.BOTTOM:
                Tile bottomTile = currentMap.FindBottomTile(pB.currentTile);
                if (currentMap.CheckMove(bottomTile))
                {
                    pB.MovePlayer(bottomTile.tileGO.transform.position);
                    pB.currentTile = bottomTile;
                }
                break;
            case Entity.Direction.LEFT:
                Tile leftTile = currentMap.FindLeftTile(pB.currentTile);
                if (currentMap.CheckMove(leftTile))
                {
                    pB.MovePlayer(leftTile.tileGO.transform.position);
                    pB.currentTile = leftTile;
                }
                break;
        }
        enableMove = false;
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

