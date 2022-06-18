using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity
{


    //Number of essences (= points of action)
    public int numEssence = 100;
    [SerializeField] private int attackCost = 3;
    public int moveCost = 1;

    public bool attackNext = false;
    public bool moveNext = false;
    public bool changingRoom = false;

    [SerializeField] private int weaponDamage;

    private GameManager instanceGM;

    public Weapon weapon;

    public List<Tile> tilesOnFire = new List<Tile>();
    public bool cdFire = false;

    public bool isInShop = false;

    public Image buttonImage;

    [HideInInspector] public Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        currentPosition = transform.position;
        weapon = new Weapon(RuneManager.instanceRM.currentWeapon, 0, 1);
        hp = maxHP;
        attackDuration = 0.2f;

        turnArrow = this.transform.Find("Arrow").gameObject;

        switch (RuneManager.instanceRM.currentWeapon)
        {
            case WeaponType.DAGGER:
                buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/DagueUp");
                break;
            case WeaponType.HANDGUN:
                buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/GunUp");
                break;
            case WeaponType.GRIMOIRE:
                buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/SpellUp");
                break;
        }
        playerAnim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.myTurn)
        {
            turnArrow.SetActive(true);
            if (!cdFire)
            {
                if(tilesOnFire.Count > 0)
                {
                    for(int i = 0; i < tilesOnFire.Count; i++)
                    {
                        if(tilesOnFire[i].fireCD > 0)
                        {
                            tilesOnFire[i].fireCD -= 1;
                            if(tilesOnFire[i].fireCD <= 0)
                            {
                                tilesOnFire.Remove(tilesOnFire[i]);
                                i--;
                            }
                        }
                    }
                }
                cdFire = true;
            }
        }
        else
        {
            turnArrow.SetActive(false);
        }

        //turn management
        if (Input.GetKeyDown(KeyCode.B))
        {
            this.gameObject.transform.position = currentTile.gameObject.transform.position;
        }

        //hp management
        if (hp <= 0)
        {
            if(Inventory.instanceInventory.HasItem("Revivor"))
            {
                hp = 1;
                Inventory.instanceInventory.RemoveItem("Revivor");
            }
            else
            {
                playerAnim.SetBool("Death", true);
                AchievementManager.instanceAM.roomWithoutTakingDamage = 0;
                AchievementManager.instanceAM.UpdateDeathNumber();
                StartCoroutine(ToMainMenu());
            }
        }


        //move process
        if (moveInProgress && !canMove && timeElapsed < moveDuration)
        {
            playerAnim.SetBool("Move", true);
            transform.position = Vector3.Lerp(currentPosition, targetPosition, timeElapsed / moveDuration) - new Vector3(0, 0, 1);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            playerAnim.SetBool("Move", false);
            moveInProgress = false;
            canMove = true;
            timeElapsed = 0;

            if (currentTile.isPike && !isOnThePike)
            {
                currentTile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Assets/Tiles/TilemapsDark_Spritesheet_25");
                isOnThePike = true;
                AchievementManager.instanceAM.UpdateTrapsActivated();
                if (Inventory.instanceInventory.HasItem("Trap Protector") == true)
                {
                    ShopItem trapProtector = Inventory.instanceInventory.GetItem("Trap Protector");
                    Debug.Log(trapProtector.itemName + ", " + trapProtector.itemCooldown);
                    if (trapProtector.itemCooldown == 0)
                    {
                        trapProtector.itemCooldown = 5;
                    }
                    else
                    {
                        Damage(1, this);
                    }
                }
                else
                {
                    Damage(1, this);
                }
            }

            if (currentTile.isShop && !isInShop)
            {
                GameManager.instanceGM.ShopIG();
                isInShop = true;
            }

            if (currentTile.tileIndex == currentMap.exitTileIndex && changingRoom == false && currentMap.canExit)
            {
                changingRoom = true;
                StartCoroutine(GoToNextRoom());
            }
        }

    }

    public override void StartAttack(List<AttackTileSettings> _upDirectionATS)
    {
        if(myTurn && !hasAttack)
        {
            playerAnim.SetTrigger("Atk");
            numEssence -= attackCost;

            /*List<AttackTileSettings> attackPattern = ConvertPattern(_upDirectionATS, direction);

            List<Entity> enemiesInRange = new List<Entity>();

            enemiesInRange = GetEntityInRange(attackPattern, true);

            if (enemiesInRange != null && enemiesInRange.Count > 0)
            {
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] is Enemy)
                    {
                        if(Inventory.instanceInventory.HasItem("Power Gloves"))
                        {
                            Damage(weapon.weaponDamage + 1, enemiesInRange[i]);
                        }
                        else
                        {
                            Damage(weapon.weaponDamage, enemiesInRange[i]);
                        }
                    }
                }
            }*/

            //for turn by turn
            hasAttack = true;
            if(Inventory.instanceInventory.HasItem("Power Gloves"))
            {
                Debug.Log("USING GLOVE");
                this.weapon.ApplyEffect(this, 1);
            }
            else
            {
                this.weapon.ApplyEffect(this, 0);
            }
            if (mobility > 0)
            {
                mobility--;
            }
            else
            {
                StartCoroutine(EndTurn(attackDuration));
            }
        }
    }





    public void AttackButton()
    {
        if(SwipeDetection.instanceSD.blockInputs == false && !hasAttack)
        {
            if (!attackNext)
            {
                attackNext = true;
                switch(RuneManager.instanceRM.currentWeapon)
                {
                    case WeaponType.DAGGER:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/DagueDown");
                        break;
                    case WeaponType.HANDGUN:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/GunDown");
                        break;
                    case WeaponType.GRIMOIRE:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/SpellDown");
                        break;
                }
            }
            else
            {
                attackNext = false;
                switch (RuneManager.instanceRM.currentWeapon)
                {
                    case WeaponType.DAGGER:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/DagueUp");
                        break;
                    case WeaponType.HANDGUN:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/GunUp");
                        break;
                    case WeaponType.GRIMOIRE:
                        buttonImage.sprite = Resources.Load<Sprite>("Assets/Graphics/UI/HUD/SpellUp");
                        break;
                }
            }
        }
    }

    public IEnumerator GoToNextRoom()
    {
        UI.instanceUI.Fade();
        yield return new WaitForSeconds(0.5f);
        GameObject darkMatter = GameObject.FindWithTag("DarkMatter");
        if(darkMatter != null)
        {
            Destroy(darkMatter);
        }
        GameManager.instanceGM.UpdateScoreAndMap();
        changingRoom = false;
    }

    public IEnumerator ToMainMenu()
    {
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene("MainMenu");
    }

    //function to take damage / die
/*    public override void DamageSelf(int damage)
    {

    }*/
}
