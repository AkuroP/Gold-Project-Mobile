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
    public int nextBoss = 1;
    public int numberOfBosses = 2;
    public int shopRoomNumber = 5;
    public int actualDangerousness;
    public int turnNumber;
    public AudioClip LevelClip;
    public AudioClip BossClip;

    [SerializeField] private GameObject shopUIprefab;
    [SerializeField] private GameObject shopPrefab;
    public AudioSource sfxAudioSource;
    public AudioSource sfxAudioSource2;
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

    public GameObject pauseMenu;
    public bool isPaused;

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

            if(nextBoss >= numberOfBosses - 1)
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
            if(!playingEntity.hasPlay && playingEntity is Player)
            {
                SwipeDetection.instanceSD.blockInputs = false;
            }
            if(playingEntity.hasPlay)
            {
                //reset entity who has played
                playingEntity.myTurn = false;
                playingEntity.hasMove = false;
                playingEntity.hasAttack = false;
                playingEntity.hasPlay = false;
                playingEntity.hasCheckStatus = false;
                if(playingEntity is Player)
                {
                    SwipeDetection.instanceSD.blockInputs = true;
                    playingEntity.mobility = 0;
                    playingEntity.GetComponent<Player>().cdFire = false;
                }

                if(GameObject.FindWithTag("Player").GetComponent<Player>().hp > 0 && !GameObject.FindWithTag("Player").GetComponent<Player>().changingRoom)
                {
                    //next entity play
                    ChangeEntity();
                }
            }
        }
        else
        {
            if(GameObject.FindWithTag("Player").GetComponent<Player>().hp > 0 && !GameObject.FindWithTag("Player").GetComponent<Player>().changingRoom)
            {
                //next entity play
                ChangeEntity();
            }
        }
    
    }

    public void PauseResume(bool _pause)
    {
        if(_pause)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            isPaused = false;
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
        if(entities == player.currentMap.entities.Count)
        {
            AchievementManager.instanceAM.UpdateCowardAchievement();
        }

        //floor change
        if (room % 11 == 0)
        {
            floor++;
            room = 1;
            RuneManager.instanceRM.darkMatter += 1;
            PlayerPrefs.SetInt("darkMatter", RuneManager.instanceRM.darkMatter);
            if (Inventory.instanceInventory.HasItem("Life Regeneration"))
            {
                if(player.hp < player.maxHP)
                {
                    player.hp++;
                }
            }
        }

        //music change
        if (room == 1)
        {
            AudioSource audio = GetComponent<AudioSource>();

            audio.clip = LevelClip;
            audio.Play();

        }

        if (room == 10)
        {
            AudioSource audio = GetComponent<AudioSource>();

            audio.clip = BossClip;
            audio.Play();

        }

        //dangerousness update
        if (score % 20 == 0)
        {
            actualDangerousness = 1 + (score / 20);
        }


        //item update
        Inventory.instanceInventory.RefreshCooldown();

        //map generation
        if(room != 6)
        {
            instanceGM.NewMap();
        }
        //shop
        else
        {
            if (currentMap != null)
            {
                player.currentTile = currentMap.tilesList[currentMap.entranceTileIndex];
            }
            if (Inventory.instanceInventory.mysteryBoxInShop == true && Inventory.instanceInventory.mysteryBoxDangerousness < 5)
            {
                Inventory.instanceInventory.mysteryBoxDangerousness++;
            }
            UI.instanceUI.shopUI = Instantiate(shopUIprefab, UI.instanceUI.canvas.transform);
            Instantiate(shopPrefab);
            UI.instanceUI.shopUI.transform.Find("Shop").transform.Find("CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(UI.instanceUI.CloseShop);
            SwipeDetection.instanceSD.isInShop = true;
        }
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

    public void ChangeEntity()
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

        if(playingEntity != null)
        {
            if (playingEntity is Player)
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
                    sfxAudioSource.clip = Resources.Load<AudioClip>("Assets/audio/SFX_Item_WornBoots");
                    sfxAudioSource.Play();
                    playingEntity.mobility++;
                    if(Inventory.instanceInventory.items[0].itemName == "Worn Speed Boots")
                    {
                        UI.instanceUI.activeSlot1.sprite = Resources.Load<Sprite>("Assets/GA/HUD/activeItem1");
                    }
                    else if (Inventory.instanceInventory.items[1].itemName == "Worn Speed Boots")
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
        playingEntity.myTurn = true;
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
