using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceGM;
    public MapBuilder instanceMB;

    //Turns enum and variable to know what turn it is
    public enum Turn
    {
        PLAYERTURN,
        ENEMYTURN
    }
    public Turn whatTurn;

    public int score;
    public int floor;
    public int room;
    public int nextBoss;
    public int numberOfBosses = 2;
    public int shopRoomNumber = 5;
    public int actualDangerousness;
    public int turnNumber;

    [SerializeField] private GameObject shopUIprefab;
    [SerializeField] private GameObject shopPrefab;
    [HideInInspector] public Player player;

    public List<GameObject> enemiesPlaying;
    public int allEnemiesActionFinished;

    public int gold = 100;

    private void Awake()
    {
        if (instanceGM != null)
        {
            Destroy(this);
        }
        instanceGM = this;
    }

    public Map currentMap;
    public List<Entity> allEntities = new List<Entity>();
    public Entity playingEntity;
    public int indexPlayingEntity = 0;

    public bool firstUpgrade;
    public bool secondUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        instanceMB = GameObject.FindWithTag("MapBuilder").GetComponent<MapBuilder>();

        NewMap();

        actualDangerousness = 1 + (score / 20);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        AchievementManager.instanceAM.UpdateScoreAchievement();
    }

    public void NewMap()
    {
        if(currentMap != null)
        {
            Destroy(currentMap.gameObject);
            currentMap = null;
        }

        if(room == 10)
        {
            currentMap = instanceMB.CreateMap(true, nextBoss);

            if(nextBoss >= numberOfBosses)
            {
                nextBoss = 0;
            }
            else
            {
                nextBoss++;
            }
        }
        else
        {
            currentMap = instanceMB.CreateMap();
        }

        SetUpMapRound(currentMap);
    }

    public void SetUpMapRound(Map _currentMap)
    {
        currentMap = _currentMap;

        //set up all the entities with player in first
        allEntities = TriGnome(_currentMap.entities);
        allEntities.Insert(0, _currentMap.player);

        //player play first
        playingEntity = allEntities[0];

        playingEntity.myTurn = true;
        playingEntity.hasMove = false;
        playingEntity.hasAttack = false;

        indexPlayingEntity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(playingEntity != null)
        {
            if(playingEntity.hasPlay)
            {
                //reset entity who has played
                playingEntity.myTurn = false;
                playingEntity.hasMove = false;
                playingEntity.hasAttack = false;
                playingEntity.hasPlay = false;
                playingEntity.hasCheckStatus = false;
                if(playingEntity.tag == "Player")
                {
                    SwipeDetection.instanceSD.blockInputs = true;
                    playingEntity.mobility = 0;
                    playingEntity.GetComponent<Player>().cdFire = false;
                }

                //next entity play
                StartCoroutine(ChangeEntity());
            }
        }
        else
        {
            //next entity play
            StartCoroutine(ChangeEntity());
        }
    }

    public void UpdateScoreAndMap()
    {
        score++;
        room++;
        shopRoomNumber--;
        turnNumber = 0;

        //Achievements check
        AchievementManager.instanceAM.UpdateScoreAchievement();
        AchievementManager.instanceAM.UpdateRoomWithoutTakingDamageAchievement();
        int entities = 0;
        foreach(Entity entity in player.currentMap.entities)
        {
            if(entity != null)
            {
                entities++;
            }
        }
        if(entities - 1 == player.currentMap.enemySpawnTiles.Count)
        {
            AchievementManager.instanceAM.UpdateCowardAchievement();
        }

        //floor change
        if (room % 11 == 0)
        {
            floor++;
            room = 1;
            if(Inventory.instanceInventory.HasItem("Life Regeneration"))
            {
                if(player.hp < player.maxHP)
                {
                    player.hp++;
                }
            }
        }

        //shop
        if (room == 6)
        {
            if (Inventory.instanceInventory.mysteryBoxInShop == true && Inventory.instanceInventory.mysteryBoxDangerousness < 5)
            {
                Inventory.instanceInventory.mysteryBoxDangerousness++;
            }
            UI.instanceUI.shopUI = Instantiate(shopUIprefab, UI.instanceUI.canvas.transform);
            Instantiate(shopPrefab);
            UI.instanceUI.shopUI.transform.Find("CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(UI.instanceUI.CloseShop);
            SwipeDetection.instanceSD.blockInputs = true;
        }

        //dangerousness update
        if (score % 20 == 0)
        {
            actualDangerousness = 1 + (score / 20);
        }


        //item update
        Inventory.instanceInventory.RefreshCooldown();

        //map generation
        instanceGM.NewMap();
    }

    public void ShopIG()
    {
        Debug.Log("SHOP IN GAME GO BRRR");
        if (Inventory.instanceInventory.mysteryBoxInShop == true && Inventory.instanceInventory.mysteryBoxDangerousness < 5)
        {
            Inventory.instanceInventory.mysteryBoxDangerousness++;
        }
        UI.instanceUI.shopUI = Instantiate(shopUIprefab, UI.instanceUI.canvas.transform);
        Instantiate(shopPrefab);
        UI.instanceUI.shopUI.transform.Find("CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(UI.instanceUI.CloseShop);
        SwipeDetection.instanceSD.blockInputs = true;
    }

    public IEnumerator ChangeEntity()
    {
        //new entity play
        if (indexPlayingEntity >= allEntities.Count - 1)
        {
            indexPlayingEntity = 0;
        }
        else
        {
            indexPlayingEntity++;
        }
        playingEntity = allEntities[indexPlayingEntity];
        //Debug.Log(allEntities[indexPlayingEntity].name);

        yield return new WaitForSeconds(0.5f);
        if(playingEntity != null)
        {
            if (playingEntity.tag == "Player")
            {
                turnNumber++;
                SwipeDetection.instanceSD.blockInputs = false;
                if (playingEntity.invincibilityTurn > 0)
                {
                    playingEntity.invincibilityTurn--;
                }
                if (Inventory.instanceInventory.HasItem("Speed Boots"))
                {
                    playingEntity.mobility++;
                }
                if (Inventory.instanceInventory.HasItem("Worn Speed Boots") && turnNumber % 3 == 0)
                {
                    playingEntity.mobility++;
                }
            }
            playingEntity.myTurn = true;
        }
    }

    public List<Entity> TriGnome(List<Entity> entities)
    {
        int index = 1;
        while (index < entities.Count)
        {
            if (entities[index].prio < entities[index - 1].prio)
            {
                Entity temp = entities[index];
                entities[index] = entities[index - 1];
                entities[index - 1] = temp;
                if (index > 1)
                {
                    index--;
                }
            }
            else { index++; }
        }
        return entities;
    }

    /*public void ChangeTurn()
    {
        if(whatTurn == Turn.PLAYERTURN)
        {
            //ENEMY TURN
            whatTurn = Turn.ENEMYTURN;
            for(int i = 0; i < enemiesPlaying.Count; i++)
            {
                enemiesPlaying[i].GetComponent<Enemy>();
            }
            Debug.Log("RETURN PLAYER TURN");
            whatTurn = Turn.PLAYERTURN;
        }
        else if(whatTurn == Turn.ENEMYTURN)
        {
            //PLAYER TURN
            whatTurn = Turn.PLAYERTURN;
        }
    }*/
}
