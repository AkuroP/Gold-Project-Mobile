using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("==== Map References ====")]
    public List<MapSettings> mapSettings = new List<MapSettings>();
    public MapSettings mapSettingsBoss1 = new MapSettings();
    public MapSettings mapSettingsBoss2 = new MapSettings();
    public MapSettings bossMapSettings = null;

    public MapSettings mapSettingShop = new MapSettings();

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

    public Map CreateMap(bool isBoss = false, int bossNumber = 0)
    {
        int randomIndex = Random.Range(3, mapSettings.Count);
        Debug.Log(randomIndex);
        GameObject currentMapInstance = Instantiate(Resources.Load("Prefabs/MapHolder"), new Vector3(0, 0f, 0), Quaternion.identity) as GameObject;


        if (isBoss)
        {
            switch (bossNumber)
            {
                case 0:
                    bossMapSettings = mapSettingsBoss1;
                    break;

                case 1:
                    bossMapSettings = mapSettingsBoss2;
                    break;

                default:
                    bossMapSettings = mapSettingsBoss1;
                    break;
            }
            currentMapInstance.GetComponent<Map>().Init(bossMapSettings, true, bossNumber, false);
            Debug.Log("Nom du niveau : " + bossMapSettings.mapName);
        }
        /*else if(GameManager.instanceGM.shopRoomNumber <= 0)
        {
            currentMapInstance.GetComponent<Map>().Init(mapSettingShop);
            if(GameManager.instanceGM.player != null)
                GameManager.instanceGM.player.moveCost = 0;
            GameManager.instanceGM.shopRoomNumber = 6;
        }*/
        else
        {
            currentMapInstance.GetComponent<Map>().Init(mapSettings[randomIndex]);
            Debug.Log("Nom du niveau : " + mapSettings[randomIndex].mapName);
            if(GameManager.instanceGM.player != null)
                GameManager.instanceGM.player.moveCost = 1;
        }
        //currentMapInstance.GetComponent<Map>().Init(mapSettingsBoss1);
        //Debug.Log("Nom du niveau : " + mapSettings[randomIndex].mapName);
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
    public bool isFixedEnemySpawn;
    public bool isMobileEnemySpawn;
    public bool isLight;
    public bool isShop;

    public Color tileColor;
    public Sprite tileSprite;
    public Sprite upSprite;
}
