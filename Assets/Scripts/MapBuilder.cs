using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("==== Map References ====")]
    public List<MapSettings> mapSettings = new List<MapSettings>();

    [Header("==== UI Button ====")]
    public bool enableMove;
    public bool enableAttack;

    public float limitBtwRightLeftAndTopBot;

    void Start()
    {
        //Create a map
        Map currentMap = CreateMap();
        currentMap.Init(mapSettings[1]);
    }

    public Map CreateMap()
    {
        GameObject currentMapInstance = Instantiate(Resources.Load("Prefabs/MapHolder"), new Vector3(-2, -2, 0), Quaternion.identity) as GameObject;
        return currentMapInstance.GetComponent<Map>();
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
    public bool isHole;
    public bool isWall;
    public bool isEnemySpawn;

    public Color tileColor;
    public Sprite tileSprite;
}

