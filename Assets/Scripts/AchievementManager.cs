using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames.BasicApi;
using GooglePlayGames;

public class AchievementManager : MonoBehaviour
{

    public bool isConnectedToGooglePlayServices;

    //Achievement variables
    public int stepsNumber;
    public int roomWithoutTakingDamage;
    public int deathNumber;
    public int enemiesKilled;
    public int bossKilled;
    public int trapsActivated;
    public int itemsPurchased;
    public int weaponNumber;
    public int runesPurchased;

    public int enemiesKilledWithDagger;
    public int bossKilledWithDagger;
    public int enemiesKilledWithBleeding;
    public int bossKilledWithBleeding;

    public int enemiesKilledWithHandgun;
    public int bossKilledWithHandgun;

    public int enemiesKilledWithGrimory;
    public int bossKilledWithGrimory;

    public static AchievementManager instanceAM;

    void Awake()
    {
        if (instanceAM == null)
        {
            instanceAM = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanceAM != this)
        {
            Destroy(gameObject);
        }

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            isConnectedToGooglePlayServices = true;
        }
        else
        {
            isConnectedToGooglePlayServices = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void UpdateScoreAchievement()
    {
        if(GameManager.instanceGM.score >= 0)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
        if (GameManager.instanceGM.score >= 102)
        {
            Social.ReportProgress(GPGSIds.achievement_go_deeper, 100.0f, null);
        }
        if (GameManager.instanceGM.score >= 665)
        {
            Social.ReportProgress(GPGSIds.achievement_your_princess_is_in_another_castle, 100.0f, null);
        }
    }

    public void UpdateStepsAchievement()
    {
        stepsNumber++;
        if(stepsNumber >= 150)
        {
            Social.ReportProgress(GPGSIds.achievement_walking_simulator, 100.0f, null);
        }
        if (stepsNumber >= 1500)
        {
            Social.ReportProgress(GPGSIds.achievement_gotta_go_fast, 100.0f, null);
        }
    }

    public void UpdateRoomWithoutTakingDamageAchievement()
    {
        roomWithoutTakingDamage++;
        if(roomWithoutTakingDamage >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_pff_even_i_can_do_that, 100.0f, null);
        }
        if (roomWithoutTakingDamage >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_its_big_brain_time, 100.0f, null);
        }
        if (roomWithoutTakingDamage >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_did_you_cheat, 100.0f, null);
        }
    }

    public void UpdateCowardAchievement()
    {
        Debug.Log("Coward");
        Social.ReportProgress(GPGSIds.achievement_the_coward_way, 100.0f, null);
    }

    public void UpdateDeathNumber()
    {
        deathNumber++;
        if(deathNumber >= 1)
        {
            Social.ReportProgress(GPGSIds.achievement_learning, 100.0f, null);
        }
        if (deathNumber >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_git_gud, 100.0f, null);
        }
    }

    public void UpdateEnemiesKilled()
    {
        enemiesKilled++;
        if (enemiesKilled >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_hunter, 100.0f, null);
        }
        if (enemiesKilled >= 200)
        {
            Social.ReportProgress(GPGSIds.achievement_annihilation, 100.0f, null);
        }
    }

    public void UpdateBossesKilled()
    {
        bossKilled++;
        if (bossKilled >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_devil_slayer, 100.0f, null);
        }
        if (bossKilled >= 30)
        {
            Social.ReportProgress(GPGSIds.achievement_god_slayer, 100.0f, null);
        }
        if (GameObject.FindWithTag("Player").GetComponent<Player>().numEssence <= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_on_the_edge, 100.0f, null);
        }
        if (GameObject.FindWithTag("Player").GetComponent<Player>().numEssence >= 75)
        {
            Social.ReportProgress(GPGSIds.achievement_energy_saving, 100.0f, null);
        }
    }

    public void UpdateTrapsActivated()
    {
        trapsActivated++;
        if (trapsActivated >= 1)
        {
            Social.ReportProgress(GPGSIds.achievement_its_a_trap, 100.0f, null);
        }
        if (trapsActivated >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_oops_i_did_it_again, 100.0f, null);
        }
    }

    public void UpdateItemsPurchased()
    {
        itemsPurchased++;
        if (itemsPurchased >= 25)
        {
            Social.ReportProgress(GPGSIds.achievement_pocket_savings, 100.0f, null);
        }
        if (itemsPurchased >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_money_money_money, 100.0f, null);
        }
    }

    public void UpdateFullCounter()
    {
        Social.ReportProgress(GPGSIds.achievement_full_counter, 100.0f, null);
    }

    public void UpdateEnemiesKilledWithDagger()
    {
        enemiesKilledWithDagger++;
        if (enemiesKilledWithDagger >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_hope_you_didnt_kill_any_friend_in_this_frenzy, 100.0f, null);
        }
    }

    public void UpdateEnemiesKilledWithHandgun()
    {
        enemiesKilledWithHandgun++;
        if (enemiesKilledWithHandgun >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_i_shot_u_ded, 100.0f, null);
        }
    }

    public void UpdateEnemiesKilledWithGrimory()
    {
        enemiesKilledWithGrimory++;
        if (enemiesKilledWithGrimory >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_well_that_a_lot_of_crispy_chicken, 100.0f, null);
        }
    }

    public void UpdateBossesKilledWithDagger()
    {
        bossKilledWithDagger++;
        if (bossKilledWithDagger >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_they_were_so_cute_now_they_are_dead, 100.0f, null);
        }
    }

    public void UpdateBossesKilledWithHandgun()
    {
        bossKilledWithHandgun++;
        if (bossKilledWithHandgun >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_youre_like_billy_the_kid__but_uglier, 100.0f, null);
        }
    }

    public void UpdateBossesKilledWithGrimory()
    {
        bossKilledWithGrimory++;
        if (bossKilledWithGrimory >= 5)
        {
            Social.ReportProgress(GPGSIds.achievement_hope_you_didnt_burn_anything_expensive, 100.0f, null);
        }
    }
}
