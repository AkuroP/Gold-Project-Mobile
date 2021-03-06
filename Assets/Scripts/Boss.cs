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

    public virtual void BossDeath()
    {
        currentMap.canExit = true;
        Feedback feedback = Instantiate(Resources.Load<Feedback>("Prefabs/Feedback"), this.transform.position, Quaternion.identity);
        feedback.Init(true, true, 3, this.transform.position);
        Destroy(this.gameObject);
        AchievementManager.instanceAM.UpdateBossesKilled();
        GameObject darkMatter = Resources.Load<GameObject>("Prefabs/DarkMatter");
        Instantiate(darkMatter, exitTile.transform.position, Quaternion.identity);
        GameObject.FindWithTag("Player").GetComponent<Player>().numEssence += 33;
        if (this is BossFrog)
        {
            GameManager.instanceGM.sfxAudioSource.clip = Resources.Load<AudioClip>("SoundDesign/SFX/Boss1Death");
            GameManager.instanceGM.sfxAudioSource.Play();
        }
        switch (player.weapon.typeOfWeapon)
        {
            case WeaponType.DAGGER:
                AchievementManager.instanceAM.UpdateBossesKilledWithDagger();
                break;
            case WeaponType.HANDGUN:
                AchievementManager.instanceAM.UpdateBossesKilledWithHandgun();
                break;
            case WeaponType.GRIMOIRE:
                AchievementManager.instanceAM.UpdateBossesKilledWithGrimory();
                break;
        }
    }
}
