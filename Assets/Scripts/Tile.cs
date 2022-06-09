using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    public int tileIndex;
    public int tileX, tileY;

    public Sprite tileSprite;
    public Sprite upSprite;
    public Color tileColor;

    public bool isReachable = true;
    public bool isWall = true;
    public bool isHole = true;
    public bool isOpen = false;
    public bool isPike = true;
    public bool isEnemySpawn = false;
    public bool isLight = false;
    public bool isShop = false;

    public int fireCD = 0;

    public Entity entityOnTile = null;

    //for pathfinding A* algorithm
    public int gCost;
    public int hCost;
    public int fCost;
    public Tile cameFromTile;


    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public void Init(int _tileIndex, int _tileX, int _tileY, bool _isReachable, bool _isWall, bool _isHole, bool _isPike, bool _isEnemySpawn, bool _isLight, bool _isShop, Sprite _tileSprite, Sprite _upSprite, Color _tileColor)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;

        isReachable = _isReachable;
        isWall = _isWall;
        isHole = _isHole;
        isPike = _isPike;
        isEnemySpawn = _isEnemySpawn;
        isLight = _isLight;
        isShop = _isShop;

        tileSprite = _tileSprite;
        if (isReachable && !isPike && !isHole && !isWall)
        {
            tileSprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_7");
        }
        else if (isReachable && isHole && !isWall && !isPike)
        {
            tileSprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_26");
        }
        else if (isReachable && isPike && !isWall && !isHole)
        {
            tileSprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_24");
        }

        //Shop
        if(isShop && isReachable && !isWall)
        {
            tileSprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_26");
        }
        else if(isShop && !isReachable && isWall)
        {
            tileSprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_25");
        }
        
        upSprite = _upSprite;
        tileColor = _tileColor;

        //place self relatively to its parent
        float floatMapWidth = transform.parent.GetComponent<Map>().mapWidth / 2f;
        float floatMapHeight = transform.parent.GetComponent<Map>().mapHeight / 2f;
        transform.position = transform.parent.transform.position + new Vector3(((float)tileX - floatMapWidth) - .5f, (float)tileY - floatMapHeight - 0.5f, 0);

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
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
        if(tileSprite != null)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
        if (upSprite != null)
        {
            this.gameObject.transform.Find("UpSprite").GetComponent<SpriteRenderer>().sprite = upSprite;
        }
    }

    public void OnFire(int _fireCD)
    {
        fireCD = _fireCD;
    }

}

