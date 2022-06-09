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

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Start is called before the first frame update
    void Start()
    {
        SignInToGooglePlayServices();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayGames.getAchievementsClient(this).unlock(getString(R.string.my_achievement_id));
    }

    public void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        switch (status)
        {
            case SignInStatus.Success:
                isConnectedToGooglePlayServices = true;
                break;
            default:
                isConnectedToGooglePlayServices = false;
                break;
        }
    }
}
