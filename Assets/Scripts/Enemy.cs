using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //for pathfinding A*
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    private List<Tile> openList;
    private List<Tile> closedList;

    public int enemyDamage;
    private bool hasRandom;
    public enum EnemyType
    {
        ENEMY1,
        ENEMY2,
        ENEMY3
    }
    public EnemyType whatEnemy;
    public int moveCDMax;
    public int moveCDCurrent;

    public int entityDangerousness;

    public Player player;
    public float enemyRange;

    public bool checkEnemiesInRange = false;
    public Direction dir;

    public bool isInitialize = false;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public Animator enemyAnim;

    private GameObject enemyAtkVFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Move(Tile _targetTile)
    {
        base.Move(_targetTile);
    }

    public virtual void StartTurn()
    {
        //Debug.Log("START TURN");
    }

    public void CheckFire()
    {
        if(this.currentTile.fireCD > 0)
        {
            this.Damage(1, this);
            this.currentTile.fireCD = 0;
        }
    }

    public void IsSelfDead()
    {
        //hp management
        if (hp <= 0)
        {
            AchievementManager.instanceAM.UpdateEnemiesKilled();
            Feedback feedback = Instantiate(Resources.Load<Feedback>("Prefabs/Feedback"), this.transform.position, Quaternion.identity);
            feedback.Init(true, true, 3, this.transform.position);
            this.currentTile.entityOnTile = null;
            Destroy(this.gameObject);
            player.numEssence += 8;
            switch (player.weapon.typeOfWeapon)
            {
                case WeaponType.DAGGER:
                    AchievementManager.instanceAM.UpdateEnemiesKilledWithDagger();
                    break;
                case WeaponType.HANDGUN:
                    AchievementManager.instanceAM.UpdateEnemiesKilledWithHandgun();
                    break;
                case WeaponType.GRIMOIRE:
                    AchievementManager.instanceAM.UpdateEnemiesKilledWithGrimory();
                    break;
            }
            if (this is EnemyOne)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster1Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
            else if (this is EnemyTwo)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster2Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
            else if (this is EnemyThree)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster3Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
            else if (this is EnemyFour)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster4Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
            else if (this is EnemyFive)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster5Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
            else if (this is EnemySix)
            {
                GameManager.instanceGM.sfxAudioSource2.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Monster6Death");
                GameManager.instanceGM.sfxAudioSource2.Play();
            }
        }
    }

    public Tile FindDirection(Tile currentTile)
    {
        List<Tile> tileAround = new List<Tile>();

        if(currentTile.topTile != null && currentTile.topTile.isReachable && !currentTile.topTile.isHole && currentTile.topTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.topTile);
        }
        if(currentTile.rightTile != null && currentTile.rightTile.isReachable && !currentTile.rightTile.isHole && currentTile.rightTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.rightTile);
        }
        if(currentTile.bottomTile != null && currentTile.bottomTile.isReachable && !currentTile.bottomTile.isHole && currentTile.bottomTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.bottomTile);
        }
        if(currentTile.leftTile != null && currentTile.leftTile.isReachable && !currentTile.leftTile.isHole && currentTile.leftTile.entityOnTile == null)
        {
            tileAround.Add(currentTile.leftTile);
        }

        Tile selectedTile = tileAround[Random.Range(0, tileAround.Count)];
        while(!currentMap.CheckMove(selectedTile, this))
        {
            selectedTile = tileAround[Random.Range(0, tileAround.Count)];
        }
        return selectedTile;
    }

    public Direction CheckAround(List<AttackTileSettings> _upDirectionATS, bool _drawAttack)
    {
        List<Entity> newList = new List<Entity>();

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.UP),_drawAttack);
        for(int i = 0; i < newList.Count; i++)
        {
            if(newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if(newList.Count > 0)
        {
            //Debug.Log("Enemy spotted UP");
            return Direction.UP;
        }

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.RIGHT), _drawAttack);
        for (int i = 0; i < newList.Count; i++)
        {
            if (newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if (newList.Count > 0)
        {
            //Debug.Log("Enemy spotted RIGHT");
            return Direction.RIGHT;
        }

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.BOTTOM), _drawAttack);
        for (int i = 0; i < newList.Count; i++)
        {
            if (newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if (newList.Count > 0)
        {
            //Debug.Log("Enemy spotted BOTTOM");
            return Direction.BOTTOM;
        }

        newList = GetEntityInRange(ConvertPattern(_upDirectionATS, Direction.LEFT), _drawAttack);
        for (int i = 0; i < newList.Count; i++)
        {
            if (newList[i] is Enemy)
            {
                newList.RemoveAt(i);
                i--;
            }
        }
        if (newList.Count > 0)
        {
            //Debug.Log("Enemy spotted LEFT");
            return Direction.LEFT;
        }

        return Direction.NONE;
    }

    public bool IsTargetInChaseRange(Tile _tragetCurrentTile, int range)
    {
        bool goodInX = false;
        bool goodInY = false;

        if(_tragetCurrentTile.tileX <= currentTile.tileX + range && _tragetCurrentTile.tileX >= currentTile.tileX - range)
            goodInX = true;

        if (_tragetCurrentTile.tileY <= currentTile.tileY + range && _tragetCurrentTile.tileY >= currentTile.tileY - range)
            goodInY = true;


        if(goodInX && goodInY)
        {
            return true;
        }
        else
            return false;
    }

    public List<Tile> FindAvailableAttackSpot(List<AttackTileSettings> _upATS)
    {
        Tile _playerTile = currentMap.player.currentTile;
        List<Tile> attackSpotTile = new List<Tile>();

        List<AttackTileSettings> allATS = new List<AttackTileSettings>();


        foreach(AttackTileSettings oneATS in _upATS)
        {
            allATS.Add(oneATS);
        }
        foreach(AttackTileSettings oneATS in ConvertPattern(_upATS, Direction.RIGHT))
        {
            allATS.Add(oneATS);
        }
        foreach(AttackTileSettings oneATS in ConvertPattern(_upATS, Direction.BOTTOM))
        {
            allATS.Add(oneATS);
        }
        foreach(AttackTileSettings oneATS in ConvertPattern(_upATS, Direction.LEFT))
        {
            allATS.Add(oneATS);
        }

        allATS = ReversePattern(allATS);

        //Debug.Log("allATS size: " + allATS.Count);

        foreach (AttackTileSettings oneATS in allATS)
        {
            Tile attackedTile = _playerTile;

            if (Mathf.Abs(oneATS.offsetX) == Mathf.Abs(oneATS.offsetY))
            {
                for (int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
                {
                    if (attackedTile == null || attackedTile.isWall) continue;

                    if (oneATS.offsetX > 0 && oneATS.offsetY > 0)
                        attackedTile = currentMap.FindRightTopTile(attackedTile);
                    else if (oneATS.offsetX > 0 && oneATS.offsetY < 0)
                        attackedTile = currentMap.FindRightBottomTile(attackedTile);
                    else if (oneATS.offsetX < 0 && oneATS.offsetY > 0)
                        attackedTile = currentMap.FindLeftTopTile(attackedTile);
                    else if (oneATS.offsetX < 0 && oneATS.offsetY < 0)
                        attackedTile = currentMap.FindLeftBottomTile(attackedTile);
                }
            }
            else
            {
                for (int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
                {
                    if (attackedTile == null || attackedTile.isWall) continue;

                    if (oneATS.offsetX > 0)
                        attackedTile = currentMap.FindLeftTile(attackedTile);
                    else if (oneATS.offsetX < 0)
                        attackedTile = currentMap.FindRightTile(attackedTile);
                }

                for (int i = 0; i < Mathf.Abs(oneATS.offsetY); i++)
                {
                    if (attackedTile == null || attackedTile.isWall) continue;

                    if (oneATS.offsetY > 0)
                        attackedTile = currentMap.FindTopTile(attackedTile);
                    else if (oneATS.offsetY < 0)
                        attackedTile = currentMap.FindBottomTile(attackedTile);
                }
            }

            if (attackedTile != null && !attackedTile.isWall && attackedTile.isReachable)
            {
                attackSpotTile.Add(attackedTile);
            }
        }

        //Debug.Log("attackSpotTile size: " + attackSpotTile.Count);

        return attackSpotTile;

    }

    public List<Tile> FindQuickestPath(Tile _originTile, List<Tile> _targetTileList, bool _drawDebug)
    {
        if(_targetTileList.Count > 0)
        {
            List<Tile> quickestPath = FindPath(_originTile, _targetTileList[0], false);

            if(quickestPath == null)
            {
                Debug.Log("personne aussi");
                return null;
            }

            if(_targetTileList.Count > 1)
            {
                for(int i = 0; i < _targetTileList.Count; i++)
                {
                    if (_targetTileList[i].entityOnTile != null) continue;

                    List<Tile> newPath = FindPath(_originTile, _targetTileList[i], false);

                    if (quickestPath != null && newPath != null && newPath.Count < quickestPath.Count)
                    {
                        quickestPath = newPath;
                    }
                }
            }

            //StartCoroutine(ShowTile(quickestPath[quickestPath.Count -1], 0));
            return quickestPath;
        }

        return null;
    }


    public List<Tile> FindPath(Tile _originTile, Tile _targetTile, bool _drawDebug)
    {
        Tile startTile = _originTile;
        Tile endTile = _targetTile;

        //open List for Tile to check, closed List for the one checked
        openList = new List<Tile>() { startTile };
        closedList = new List<Tile>();

        foreach(Tile oneTile in currentMap.tilesList)
        {
            oneTile.gCost = int.MaxValue;
            oneTile.CalculateFCost();
            oneTile.cameFromTile = null;
        }

        startTile.gCost = 0;
        startTile.hCost = CalculateDistanceCost(startTile, endTile);
        startTile.CalculateFCost();


        int step = 0;
        while(openList.Count > 0)
        {
            Tile currentTile = FindLowestFCostTile(openList);

            //======= DEBUG START =======//
            if(_drawDebug)
                StartCoroutine(currentTile.TurnColor(new Color(0f, 1f, 0f, 1f), step));
            //======= DEBUG END =======//

            if (currentTile == endTile)
            {
                //======= DEBUG START =======//
                if(_drawDebug)
                {
                    foreach (Tile tile in closedList)
                    {
                        if (!tile.isReachable) continue;
                        StartCoroutine(tile.TurnColor(new Color(0.6f, 0.6f, 0.6f, 1f), step));
                    }
                    foreach (Tile tile in closedList)
                    {
                        if (!tile.isReachable) continue;
                        StartCoroutine(tile.TurnColor(new Color(0.6f, 0.6f, 0.6f, 1f), step));
                    }
                }
                //======= DEBUG END =======//

                //Final Tile reached
                return CalculatePath(endTile, step);
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach(Tile neighbourTile in GetNeighboursList(currentTile))
            {
                //if in closed list Tile is alrday checked, go next
                if (closedList.Contains(neighbourTile)) continue;
                //if is not reachable go next
                if (!neighbourTile.isReachable || neighbourTile.entityOnTile is Enemy || neighbourTile.isOpen)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }

                if(_drawDebug)
                    StartCoroutine(neighbourTile.TurnColor(new Color(0f, 0f, 1f, 1f), step));

                int tentativeGCost = currentTile.gCost + CalculateDistanceCost(currentTile, neighbourTile);
                if (tentativeGCost < neighbourTile.gCost)
                {
                    neighbourTile.cameFromTile = currentTile;
                    neighbourTile.gCost = tentativeGCost;
                    neighbourTile.hCost = CalculateDistanceCost(neighbourTile, endTile);
                    neighbourTile.CalculateFCost();


                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
            }

            //======= DEBUG START =======//
            if(_drawDebug)
                StartCoroutine(currentTile.TurnColor(new Color(1f, 0f, 0f, 1f), step + 1));
            //======= DEBUG END =======//

            step++;
        }

        //open List is empty, cannot reach targetTile
        Debug.Log("personne");
        return null;
    }

    private int CalculateDistanceCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.tileX - b.tileX);
        int yDistance = Mathf.Abs(a.tileY - b.tileY);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Tile FindLowestFCostTile(List<Tile> tileList)
    {
        Tile lowestFCostTile = tileList[0];

        for(int i = 0; i < tileList.Count; i++)
        {
            if(tileList[i].fCost < lowestFCostTile.fCost)
            {
                lowestFCostTile = tileList[i];
            }
        }

        return lowestFCostTile;
    }

    private List<Tile> GetNeighboursList(Tile tile)
    {
        List<Tile> neighbourList = new List<Tile>();

        if(tile.topTile != null)
            neighbourList.Add(tile.topTile);
        if(tile.rightTile != null)
            neighbourList.Add(tile.rightTile);
        if(tile.bottomTile != null)
            neighbourList.Add(tile.bottomTile);
        if(tile.leftTile != null)
            neighbourList.Add(tile.leftTile);

        return neighbourList;
    }

    private List<Tile> CalculatePath(Tile endTile, int step = 0)
    {
        List<Tile> path = new List<Tile>();

        path.Add(endTile);

        Tile currentTile = endTile;
        while(currentTile.cameFromTile != null)
        {
            //======= DEBUG START =======//
            //StartCoroutine(currentTile.TurnColor(new Color(0f, 1f, 0f, 1f), step));
            //======= DEBUG END =======//

            path.Add(currentTile.cameFromTile);
            currentTile = currentTile.cameFromTile;
        }

        path.Reverse();
        return path;
    }




    public void EnemyRandomMove()
    {
        bool accessible = false;

        while(!accessible)
        {
            int randomMove = Random.Range(0, 4);
            switch (randomMove)
            {
                case 0:
                    direction = Direction.UP;
                    accessible = FindNextTile();
                    break;
                case 1:
                    direction = Direction.LEFT;
                    accessible = FindNextTile();
                    break;
                case 2:
                    direction = Direction.BOTTOM;
                    accessible = FindNextTile();
                    break;
                case 3:
                    direction = Direction.RIGHT;
                    accessible = FindNextTile();
                    break;
            }
        }
    }

    public override void StartAttack(List<AttackTileSettings> _upDirectionATS)
    {
        turnDuration += attackDuration;

        List<AttackTileSettings> attackPattern = ConvertPattern(_upDirectionATS, direction);

        if (attackPattern[0].offsetX > 1)
        {
            entitySr.flipX = true;
        }
        else if (attackPattern[0].offsetX < 1)
        {
            entitySr.flipX = false;
        }

        List<Entity> enemiesInRange = new List<Entity>();

        List<Tile> tileInRange =  GetTileInRange(ConvertPattern(_upDirectionATS, direction), false);
        enemyAtkVFX = Resources.Load<GameObject>("Prefabs/Enemy_Atk_VFX");
        if(this is EnemyTwo)
        {
            StartCoroutine(Enemy2WaitAnim(tileInRange));
        }
        else
        {
            SpawnAtkVFX(tileInRange);
        }
        
        
        //Debug.Log(direction);
        foreach(AttackTileSettings oneATS in attackPattern)
        {
            //Debug.Log("X: " + oneATS.offsetX + " Y: " + oneATS.offsetY);
        }

        enemiesInRange = GetEntityInRange(attackPattern, true);

        if (enemiesInRange != null && enemiesInRange.Count > 0)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] is Player)
                {

                    //Item activated when the player is damaged
                    if (Inventory.instanceInventory.HasItem("Invincibility") && Inventory.instanceInventory.items[Inventory.instanceInventory.GetItemIndex("Invincibility")].itemCooldown == 0)
                    {
                        player.invincibilityTurn = 3;
                        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_Highlight");
                        GameManager.instanceGM.sfxAudioSource.Play();
                        if (Inventory.instanceInventory.items[0].itemName == "Invincibility")
                        {
                            UI.instanceUI.activeSlot1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem1");
                            Inventory.instanceInventory.items[0].itemCooldown = 1;
                        }
                        else if (Inventory.instanceInventory.items[1].itemName == "Invincibility")
                        {
                            UI.instanceUI.activeSlot2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem2");
                            Inventory.instanceInventory.items[1].itemCooldown = 1;
                        }
                        else if (Inventory.instanceInventory.items[2].itemName == "Invincibility")
                        {
                            UI.instanceUI.activeSlot3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem3");
                            Inventory.instanceInventory.items[2].itemCooldown = 1;
                        }
                    }
                    else if (Inventory.instanceInventory.HasItem("Poison Fog"))
                    {
                        Inventory.instanceInventory.RemoveItem("Poison Fog");
                        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_Poison");
                        GameManager.instanceGM.sfxAudioSource.Play();
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach(GameObject enemy in enemies)
                        {
                            enemy.GetComponent<Enemy>().hp--;
                        }
                    }
                    else if (Inventory.instanceInventory.HasItem("Freeze Time"))
                    {
                        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_Highlight");
                        GameManager.instanceGM.sfxAudioSource.Play();
                        if (Inventory.instanceInventory.items[0].itemName == "Freeze Time")
                        {
                            UI.instanceUI.activeSlot1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem1");
                            Inventory.instanceInventory.items[0].itemCooldown = 1;
                        }
                        else if (Inventory.instanceInventory.items[1].itemName == "Freeze Time")
                        {
                            UI.instanceUI.activeSlot2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem2");
                            Inventory.instanceInventory.items[1].itemCooldown = 1;
                        }
                        else if (Inventory.instanceInventory.items[2].itemName == "Freeze Time")
                        {
                            UI.instanceUI.activeSlot3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem3");
                            Inventory.instanceInventory.items[2].itemCooldown = 1;
                        }
                        player.mobility += 2;
                        GameManager.instanceGM.indexPlayingEntity = GameManager.instanceGM.allEntities.Count - 1;
                        player.myTurn = true;
                    }

                    Damage(enemyDamage, enemiesInRange[i]);

                    //item that boosts the player when damages
                    if (Inventory.instanceInventory.HasItem("Counter Ring"))
                    {
                        GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_Counter");
                        GameManager.instanceGM.sfxAudioSource.Play();
                        player.damageMultiplicator = 2;
                        if (Inventory.instanceInventory.items[0].itemName == "Counter Ring")
                        {
                            UI.instanceUI.activeSlot1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem1");
                        }
                        else if (Inventory.instanceInventory.items[1].itemName == "Counter Ring")
                        {
                            UI.instanceUI.activeSlot2.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem2");
                        }
                        else
                        {
                            UI.instanceUI.activeSlot3.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem3");
                        }
                    }
                }
            }
        }

    }

    private IEnumerator Enemy2WaitAnim(List<Tile> _tileInRange)
    {
        yield return new WaitForSeconds(.28f);
        SpawnAtkVFX(_tileInRange);
    }

    private void SpawnAtkVFX(List<Tile> _tileInRange)
    {
        for(int i = 0; i < _tileInRange.Count; i++)
        {
            Instantiate(enemyAtkVFX, _tileInRange[i].transform);
        }
    }

}
