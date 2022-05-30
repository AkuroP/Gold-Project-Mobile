using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile
{
    public int tileIndex;
    public int tileX, tileY;

    public GameObject tileGO;

    public Color tileColor;

    public bool hasEntity = false;
    public bool isReachable = true;
    public bool isEnemySpawn = false;

    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public Tile(int _tileIndex, int _tileX, int _tileY, bool _isReachable, bool _isEnemySpawn, Color _tileColor)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;

        isReachable = _isReachable;
        isEnemySpawn = _isEnemySpawn;
        tileColor = _tileColor;
    }
}

