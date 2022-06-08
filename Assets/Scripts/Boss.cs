using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    private Map map;
    private Tile exitTile;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.FindWithTag("Player").GetComponent<Player>().currentMap;
        exitTile = map.tilesList[map.exitTileIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BossDeath()
    {
        currentMap.canExit = true;
        Destroy(this.gameObject);
        GameObject darkMatter = Resources.Load<GameObject>("Prefabs/DarkMatter");
        Instantiate(darkMatter, exitTile.transform.position, Quaternion.identity);
    }
}
