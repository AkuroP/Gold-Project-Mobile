using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("==== Map References ====")]
    public List<MapSettings> mapSettings = new List<MapSettings>();
    public MapSettings mapSettingsBoss1 = new MapSettings();
    public MapSettings mapSettingsBoss2 = new MapSettings();

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
        int randomIndex = Random.Range(3, mapSettings.Count);
        GameObject currentMapInstance = Instantiate(Resources.Load("Prefabs/MapHolder"), new Vector3(0, 0f, 0), Quaternion.identity) as GameObject;
        currentMapInstance.GetComponent<Map>().Init(mapSettings[randomIndex]);
        //currentMapInstance.GetComponent<Map>().Init(mapSettingsBoss1);
        Debug.Log("Nom du niveau : " + mapSettings[randomIndex].mapName);
        return currentMapInstance.GetComponent<Map>();
    }


}


[System.Serializable]
public class MapSettings
{
    public int mapWidth, mapHeight;
    public int entranceTileIndex, exitTileIndex;
    public int mapDangerousness;

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
    public bool isPike;
    public bool isEnemySpawn;
    public bool isLight;

    public Color tileColor;
    public Sprite tileSprite;
    public Sprite upSprite;
}
