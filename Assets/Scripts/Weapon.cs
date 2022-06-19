using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    DAGGER,
    HANDGUN,
    GRIMOIRE,
}

[System.Serializable]
public class Weapon
{
    public WeaponType typeOfWeapon;
    public int weaponLevel;
    public int weaponDamage;

    public List<AttackTileSettings> upDirectionATS;
    
    public int bleedingCD;

    public Weapon(WeaponType _weaponType, int _weaponLevel, int _weaponDamage)
    {
        weaponLevel = _weaponLevel;
        weaponDamage = _weaponDamage;
        typeOfWeapon = _weaponType;

        upDirectionATS = new List<AttackTileSettings>();

        switch (_weaponType)
        {
            case WeaponType.DAGGER:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
            break;

            case WeaponType.HANDGUN:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(2, 0, 2));
                upDirectionATS.Add(new AttackTileSettings(3, 0, 3));
                if (weaponLevel >= 1)
                {
                    upDirectionATS.Add(new AttackTileSettings(4, 0, 4));
                }
                break;
            
            case WeaponType.GRIMOIRE:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                upDirectionATS.Add(new AttackTileSettings(1, -1, 1));
                upDirectionATS.Add(new AttackTileSettings(1, 1, 1));
                if(weaponLevel >= 1)
                {
                    upDirectionATS.Add(new AttackTileSettings(1, 0, -1));
                }
                Debug.Log("GRIMOIRE");
                break;
            default:
                upDirectionATS.Add(new AttackTileSettings(1, 0, 1));
                break;
        }
    }

    


    public void ApplyEffect(Entity _attacker, int _bonusDamage)
    {
        List<Entity> enemiesInRange = new List<Entity>();
        enemiesInRange = _attacker.GetEntityInRange(_attacker.ConvertPattern(upDirectionATS, _attacker.direction), true);

        switch(this.typeOfWeapon)
        {
            case WeaponType.DAGGER:

                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] is Enemy)
                    {
                        if (this.weaponLevel >= 2 && enemiesInRange[i].hp <= this.weaponDamage + _bonusDamage)
                        {
                            Debug.Log("BONUS MOVE");
                            _attacker.GetComponent<Player>().mobility++;
                            _attacker.GetComponent<Player>().hasPlay = false;
                        }
                        _attacker.Damage(this.weaponDamage + _bonusDamage, enemiesInRange[i]);
                        if(this.weaponLevel >= 1)
                        {
                            enemiesInRange[i].ApplyDebuff(Debuff.Status.BLEED, bleedingCD);
                        }
                    }
                }
            break;
            

            case WeaponType.HANDGUN:

                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] is Enemy)
                    {
                        if(this.weaponLevel < 2)
                        {
                            _attacker.Damage(weaponDamage + _bonusDamage, enemiesInRange[0]);
                            break;
                        }
                        else
                        {
                            _attacker.Damage(weaponDamage + _bonusDamage, enemiesInRange[i]);
                        }
                    }
                }
                if (enemiesInRange.Count == 2 && enemiesInRange[0].hp == 0 && enemiesInRange[1].hp == 0)
                {
                    AchievementManager.instanceAM.UpdateHandgunDoubleKill();
                }

                break;

            case WeaponType.GRIMOIRE:


                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] is Enemy)
                    {
                        _attacker.Damage(weaponDamage + _bonusDamage, enemiesInRange[i]);
                        Debug.Log("GRIMOIRE ATTACK !");
                    }
                }

                List<Tile> tileOnFire = _attacker.GetTileInRange(_attacker.ConvertPattern(upDirectionATS, _attacker.direction), false);
                for(int j = 0; j < tileOnFire.Count; j++)
                {
                    if(this.weaponLevel >= 2)
                    {
                        Debug.Log("FIRE !");
                        if(tileOnFire[j].fireCD <= 0)
                        {
                            tileOnFire[j].fireCD = 2;
                            _attacker.GetComponent<Player>().tilesOnFire.Add(tileOnFire[j]);
                        }
                    }

                }

                if(enemiesInRange.Count == 2 && enemiesInRange[0].hp == 0 && enemiesInRange[1].hp == 0)
                {
                    AchievementManager.instanceAM.UpdateGrimoireDoubleKill();
                }
                else if (enemiesInRange.Count == 3 && enemiesInRange[0].hp == 0 && enemiesInRange[1].hp == 0 && enemiesInRange[1].hp == 0)
                {
                    AchievementManager.instanceAM.UpdateGrimoireTripleKill();
                }

                break;
        }
    }
}


[System.Serializable]
public struct AttackTileSettings
{
    public int order, offsetX, offsetY;

    public AttackTileSettings(int _order, int _offsetX, int _offsetY)
    {
        this.order = _order;
        this.offsetX = _offsetX;
        this.offsetY = _offsetY;
    }
}
