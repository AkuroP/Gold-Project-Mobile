using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [Header("Map Models")]
    public List<MapReference> refMap = new List<MapReference>();


}

[System.Serializable]
public class MapReference
{
    public int mapWidth, mapHeight;

    [SerializeField]
    public List<TileSettings> mapTiles;
}
