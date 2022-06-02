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

    public static MapBuilder instanceMB;

    private void Awake()
    {
        if (instanceMB != null)
        {
            Destroy(instanceMB);
        }
        instanceMB = this;
    }

    void Start()
    {
    
    }

    public Map CreateMap()
    {
        int randomIndex = Random.Range(1, 4);
        GameObject currentMapInstance = Instantiate(Resources.Load("Prefabs/MapHolder"), new Vector3(-4, -5.4f, 0), Quaternion.identity) as GameObject;
        currentMapInstance.GetComponent<Map>().Init(mapSettings[randomIndex]);
        return currentMapInstance.GetComponent<Map>();
    }

    
}


[System.Serializable]
public class MapSettings
{
    public int mapWidth, mapHeight;
    public int entranceTileIndex, exitTileIndex;

    public string mapName;

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

