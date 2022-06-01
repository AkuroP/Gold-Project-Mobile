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
    public bool isEnemySpawn = false;

    public Entity entityOnTile = null;

    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public void Init(int _tileIndex, int _tileX, int _tileY, bool _isReachable, bool _isWall, bool _isHole, bool _isEnemySpawn, Color _tileColor)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;

        isReachable = _isReachable;
        isWall = _isWall;
        isHole = _isHole;
        isEnemySpawn = _isEnemySpawn;

        tileColor = _tileColor;

        //place self relatively to its parent
        transform.position = transform.parent.transform.position + new Vector3(tileX, tileY, 0);

        ChangeVisual();
    }

    public void ChangeVisual()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = tileColor;
    }

}

