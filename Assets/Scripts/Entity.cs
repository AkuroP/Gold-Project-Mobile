using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{ 
    UP, 
    RIGHT, 
    BOTTOM, 
    LEFT,
    NONE
}

public class Entity : MonoBehaviour
{
    [Header("==== Map Informations ====")]
    public Map currentMap;
    public Tile currentTile;
    public Tile lastNotHoleTile;

    [Header("==== Stat ====")]
    //hp for enemy, turn left for player
    public int maxHP;
    [SerializeField]
    public int hp;
    public int invincibilityTurn;
    //priority of entity (player always first)
    public int prio;
    [Header("==== Movement ====")]
    //mobility of entity per turn
    public int maxMobility;
    //mobility with items
    public int mobility = 0;

    public int damage;
    public int damageMultiplicator = 1;

    //directions enum and direction of the entity

    public Direction direction;

    //effects enum, effects on the weapon, effects on the entity
    //[SerializeField] protected WeaponEffect weaponEffect;
    //[SerializeField] protected WeaponEffect effectOnEntity;

    public Vector3 targetPosition, currentPosition;

    //player behaviour export 
    public bool canMove = true;
    public bool moveInProgress = false;

    public float timeElapsed;
    public float moveDuration;
    public bool canAttack = true;

    //turn by turn

    public bool myTurn = false;
    public bool hasMove = false;
    public bool hasAttack = false;
    public bool hasPlay = false;
    protected bool isOnThePike = false;

    public float turnDuration;

    //Sprite and anims
    [SerializeField] public SpriteRenderer entitySr;

    public List<Debuff> entityStatus = new List<Debuff>();

    public bool hasCheckStatus = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ApplyDebuff(Debuff.Status _debuff, int _debuffCD)
    {
        Debuff newDebuff = new Debuff(_debuff, _debuffCD);

        if(_debuff == Debuff.Status.BLEED)
        {
            for(int i = 0; i < entityStatus.Count; i++)
            {
                if(entityStatus[i].debuffStatus == Debuff.Status.BLEED)
                {
                    entityStatus.Remove(newDebuff);
                    Debug.Log("ALREADY BLEEDING");
                    break;
                }
            }
        }
        
        this.entityStatus.Add(newDebuff);
    }

    public void CheckStatus(Entity _entity)
    {
        Debug.Log("CHECKING STATUS");
        if(!this.hasCheckStatus)
        {
            for(int i = 0; i < entityStatus.Count; i++)
            {
                switch(entityStatus[i].debuffStatus)
                {
                    case Debuff.Status.BLEED :
                        this.Damage(1, _entity);
                        Debug.Log("BLEED -1 DAMAGE");
                        if (this.hp == 0 && this.GetComponent<BossFrog>() == null && this.GetComponent<BossTP>() == null)
                        {
                            AchievementManager.instanceAM.UpdateEnemiesKilledWithBleed();
                        }
                        else if (this.hp == 0 && this.GetComponent<EnemyOne>() == null && this.GetComponent<EnemyTwo>() == null && this.GetComponent<EnemyThree>() == null && this.GetComponent<EnemyFour>() == null && this.GetComponent<EnemyFive>() == null && this.GetComponent<EnemySix>() == null)
                        {
                            AchievementManager.instanceAM.UpdateBossesKilledWithBleed();
                        }
                        break;
                }
                entityStatus[i].debuffCD -= 1;
                if(entityStatus[i].debuffCD <= 0)
                {
                    entityStatus.RemoveAt(i);
                    i -= 1;
                }
            }
            this.hasCheckStatus = true;
        }
    }

    public void MovingProcess()
    {
        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;
            hasMove = true;

            if (currentTile.isPike && !isOnThePike)
            {
                this.hp--;
                currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_25");
                isOnThePike = true;
            }
        }
    }

    //virtual attack function
    public virtual void StartAttack(List<AttackTileSettings> _upDirectionATS)
    {
        
    }

    //draw attack zone
    public IEnumerator DrawAttack(Tile tile)
    {
        Color oldColor = tile.tileColor;
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(0.5f);
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
    }

    public IEnumerator ShowTile(Tile tile, float delay)
    {
        Color oldColor = tile.tileColor;
        yield return new WaitForSeconds(delay * 0.25f);
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f, 1f);
        yield return new WaitForSeconds(0.25f);
        tile.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); ;
    }

    //find ennemies in attack range
    public List<Entity> GetEntityInRange(List<AttackTileSettings> ats, bool _drawAttack = false)
    {
        List<Entity> entityInPattern = new List<Entity>();

        foreach (AttackTileSettings oneATS in ats)
        {
            Tile attackedTile = currentTile;

            if(Mathf.Abs(oneATS.offsetX) == Mathf.Abs(oneATS.offsetY))
            {
                for(int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
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
                        attackedTile = currentMap.FindRightTile(attackedTile);
                    else if (oneATS.offsetX < 0)
                        attackedTile = currentMap.FindLeftTile(attackedTile);
                }

                for (int i = 0; i < Mathf.Abs(oneATS.offsetY); i++)
                {
                    if(attackedTile == null || attackedTile.isWall) continue;

                    if (oneATS.offsetY > 0)
                        attackedTile = currentMap.FindTopTile(attackedTile);
                    else if (oneATS.offsetY < 0)
                        attackedTile = currentMap.FindBottomTile(attackedTile);
                }
            }

            if (attackedTile != null && !attackedTile.isWall)
            {
                if (_drawAttack)
                    StartCoroutine(DrawAttack(attackedTile));
                
                if (attackedTile.entityOnTile)
                {
                    entityInPattern.Add(attackedTile.entityOnTile);
                    if(CompareTag("Player") && this.GetComponent<Player>().weapon.typeOfWeapon == WeaponType.HANDGUN && oneATS.order == 4)
                    {
                        AchievementManager.instanceAM.UpdateSogeking();
                    }
                }
            }
        }

        if(CompareTag("Player") && Inventory.instanceInventory.HasItem("Side Slash"))
        {
            if(direction == Direction.UP || direction == Direction.BOTTOM)
            {
                entityInPattern.Add(currentTile.leftTile.entityOnTile);
                entityInPattern.Add(currentTile.rightTile.entityOnTile);
            }
            else
            {
                entityInPattern.Add(currentTile.topTile.entityOnTile);
                entityInPattern.Add(currentTile.bottomTile.entityOnTile);
            }
        }

        return entityInPattern;
    }

    public List<Tile> GetTileInRange(List<AttackTileSettings> ats, bool _drawAttack = false)
    {
        List<Tile> allTile = new List<Tile>();
        foreach (AttackTileSettings oneATS in ats)
        {
            Tile attackedTile = currentTile;

            if(Mathf.Abs(oneATS.offsetX) == Mathf.Abs(oneATS.offsetY))
            {
                for(int i = 0; i < Mathf.Abs(oneATS.offsetX); i++)
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
                        attackedTile = currentMap.FindRightTile(attackedTile);
                    else if (oneATS.offsetX < 0)
                        attackedTile = currentMap.FindLeftTile(attackedTile);
                }

                for (int i = 0; i < Mathf.Abs(oneATS.offsetY); i++)
                {
                    if(attackedTile == null || attackedTile.isWall) continue;

                    if (oneATS.offsetY > 0)
                        attackedTile = currentMap.FindTopTile(attackedTile);
                    else if (oneATS.offsetY < 0)
                        attackedTile = currentMap.FindBottomTile(attackedTile);
                }
            }

            if (attackedTile != null && !attackedTile.isWall)
            {
                allTile.Add(attackedTile);
            }
        }

        return allTile;
    }

    public List<AttackTileSettings> ConvertPattern(List<AttackTileSettings> upDirectionATS, Direction entityDirection)
    {
        List<AttackTileSettings> newATS = new List<AttackTileSettings>();

        switch (entityDirection)
        {
            case Direction.UP:
                newATS = upDirectionATS;
                break;

            case Direction.BOTTOM:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int newOffsetX = -1 * ats.offsetX;
                    int newOffsetY = -1 * ats.offsetY;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.RIGHT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = ats.offsetY;
                    int newOffsetY = -1 * temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;

            case Direction.LEFT:
                foreach (AttackTileSettings ats in upDirectionATS)
                {
                    int newOrder = ats.order;
                    int temp = ats.offsetX;
                    int newOffsetX = -1 * ats.offsetY;
                    int newOffsetY = temp;

                    newATS.Add(new AttackTileSettings(newOrder, newOffsetX, newOffsetY));
                }
                break;
        }

        return newATS;
    }

    public List<AttackTileSettings> ReversePattern(List<AttackTileSettings> ats)
    {
        List<AttackTileSettings> newATS = new List<AttackTileSettings>();

        foreach (AttackTileSettings oneATS in ats)
        {
            newATS.Add(new AttackTileSettings(oneATS.order, -oneATS.offsetX, -oneATS.offsetY));
        }

        return newATS;
    }

    //function to take damage / die
    public void Damage(int damage, Entity entity)
    {
        
        if (damage*damageMultiplicator >= 3 && entity.hp == 3 && entity.maxHP == 3 && !entity.CompareTag("Player"))
        {
            AchievementManager.instanceAM.UpdateFullCounter();
        }
        if (entity.invincibilityTurn == 0)
        {
            entity.hp -= damage * damageMultiplicator;
            damageMultiplicator = 1;
            if (entity.CompareTag("Player"))
            {
                AchievementManager.instanceAM.roomWithoutTakingDamage = -1;
            }
        }
    }

    public virtual void Move(Tile _targetTile)
    {
        currentPosition = transform.position;
        targetPosition = _targetTile.transform.position;
        
        Debug.Log(currentPosition + " / " + targetPosition);

        if (currentTile.isHole == true && currentTile.isOpen == false)
        {
            currentTile.isReachable = false;
            currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_14");
        }
        if (currentTile.isPike == true)
        {
            currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_24");
            isOnThePike = false;
        }

        if (!_targetTile.isHole)
        {
            _targetTile.entityOnTile = currentTile.entityOnTile;
            currentTile.entityOnTile = null;
            currentTile = _targetTile;
            lastNotHoleTile = currentTile;
        }
        else if (_targetTile.isHole)
        {
            _targetTile.entityOnTile = currentTile.entityOnTile;
            currentTile.entityOnTile = null;
            currentTile = _targetTile;
        }

        moveInProgress = true;
        if(mobility > 0)
        {
            mobility--;
        }
        else
        {
            hasMove = true;
            hasPlay = true;
        }
        if(this.CompareTag("Player"))
        {
            AchievementManager.instanceAM.UpdateStepsAchievement();
        }
        canMove = false;
    }  

    public IEnumerator MoveWithDelay(Tile _targetTile, float _delay)
    {
        yield return new WaitForSeconds(_delay);

        currentPosition = transform.position;
        targetPosition = _targetTile.transform.position;

        Debug.Log(_delay + " / " + currentPosition + " / " + targetPosition);

        if (!_targetTile.isHole)
        {
            _targetTile.entityOnTile = currentTile.entityOnTile;
            currentTile.entityOnTile = null;
            currentTile = _targetTile;
        }

        moveInProgress = true;
        canMove = false;
    }
    
    public virtual void Move(List<Tile> _targetTileList)
    {
        for(int i = 0; i < _targetTileList.Count; i++)
        {
            StartCoroutine(MoveWithDelay(_targetTileList[i], moveDuration * i + 0.15f * i));
        }
    }

    public virtual void FindNextTile()
    {
        switch (direction)
        {
            case Direction.UP:
                Tile topTile = currentMap.FindTopTile(currentTile);
                if (currentMap.CheckMove(topTile, this))
                {
                    this.Move(topTile);
                }
                break;
            case Direction.RIGHT:
                Tile rightTile = currentMap.FindRightTile(currentTile);
                if (currentMap.CheckMove(rightTile, this))
                {
                    this.Move(rightTile);
                    entitySr.flipX = true;
                }
                break;
            case Direction.BOTTOM:
                Tile bottomTile = currentMap.FindBottomTile(currentTile);
                if (currentMap.CheckMove(bottomTile, this))
                {
                    this.Move(bottomTile);
                }
                break;
            case Direction.LEFT:
                Tile leftTile = currentMap.FindLeftTile(currentTile);
                if (currentMap.CheckMove(leftTile, this))
                {
                    this.Move(leftTile);
                    entitySr.flipX = false;
                }
                break;
        }
        //enableMove = false;
    }

    protected IEnumerator Hole()
    {
        yield return new WaitForSeconds(0.5f);
        currentTile = lastNotHoleTile;
        currentTile.entityOnTile = this.gameObject.GetComponent<Entity>();
        this.gameObject.transform.position = currentTile.gameObject.transform.position;
    }

    public IEnumerator EndTurn(float waitDuration)
    {
        yield return new WaitForSeconds();
        hasPlay = true;
    }

}
