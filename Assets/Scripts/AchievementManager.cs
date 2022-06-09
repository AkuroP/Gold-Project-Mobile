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
        if(instanceAM != null)
        {
            Destroy(this);
        }
        instanceAM = this;

        DontDestroyOnLoad(this.gameObject);

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
        if (GameManager.instanceGM.score >= 150)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
        if (GameManager.instanceGM.score >= 665)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
    }

    public void UpdateStepsAchievement()
    {
        stepsNumber++;
        if(stepsNumber >= 150)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
        if (stepsNumber >= 1500)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
    }

    public void UpdateRoomWithoutTakingDamageAchievement()
    {
        roomWithoutTakingDamage++;
        if(roomWithoutTakingDamage >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
        if (roomWithoutTakingDamage >= 50)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
        if (roomWithoutTakingDamage >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
        }
    }

    public void UpdateCowardAchievement()
    {
        Debug.Log("Coward");
        Social.ReportProgress(GPGSIds.achievement_first_steps, 100.0f, null);
    }
}
