using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int tileIndex = -1;
    

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log(tileIndex);
    }
}

[System.Serializable]
public class Tile
{
    public int tileIndex;
    public int tileX, tileY;

    public GameObject tileGO;

    public bool hasEntity = false;
    public bool isReachable = true;

    [SerializeField]
    public Tile topTile, rightTile, bottomTile, leftTile;

    public Tile(int _tileIndex, int _tileX, int _tileY)
    {
        tileIndex = _tileIndex;
        tileX = _tileX;
        tileY = _tileY;
/*
        isReachable = _isReachable;
        hasEntity = _hasEntity;

        tileGO = _tileGO;*/
    }
}

