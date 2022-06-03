using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public int tileIndex;
    public int tileX, tileY;

    public Color tileColor;

    public bool isReachable = true;
    public bool isWall = true;
    public bool isHole = true;
    public bool isOpen = false;
    public bool isPike = true;
    public bool isEnemySpawn = false;

    public Entity entityOnTile = null;

    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public void Init(int _tileIndex, int _tileX, int _tileY, bool _isReachable, bool _isWall, bool _isHole, bool _isPike, bool _isEnemySpawn, Color _tileColor)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;

        isReachable = _isReachable;
        isWall = _isWall;
        isHole = _isHole;
        isPike = _isPike;
        isEnemySpawn = _isEnemySpawn;

        tileColor = _tileColor;

        //place self relatively to its parent
        float floatMapWidth = transform.parent.GetComponent<Map>().mapWidth / 2f;
        float floatMapHeight = transform.parent.GetComponent<Map>().mapHeight / 2f;
        transform.position = transform.parent.transform.position + new Vector3(((float)tileX - floatMapWidth) - .5f, (float)tileY - floatMapHeight - 0.5f, 0);

        ChangeVisual();
    }

    public void ChangeVisual()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = tileColor;
    }

}

