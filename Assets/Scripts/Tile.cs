using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public int tileIndex;
    public int tileX, tileY;

    public GameObject tileGO;

    public Color tileColor;

    public bool hasEntity = false;
    public bool isReachable = true;

    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public Tile(int _tileIndex, int _tileX, int _tileY, bool _isReachable, Color _tileColor)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;

        isReachable = _isReachable;
        tileColor = _tileColor;
    }
}

