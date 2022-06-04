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

    //for pathfinding A* algorithm
    public int gCost;
    public int hCost;
    public int fCost;
    public Tile cameFromTile;

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

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public IEnumerator TurnColor(Color newColor, int step)
    {
        yield return new WaitForSeconds(step * 0.15f);
        Color oldColor = tileColor;
        //tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.3f);
        GetComponent<SpriteRenderer>().color = newColor;
        /*yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = oldColor;*/
    }

    public void ChangeVisual()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = tileColor;
    }

}

