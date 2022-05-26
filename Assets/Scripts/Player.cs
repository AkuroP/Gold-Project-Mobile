using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    //Number of essences (= points of action)
    private int numEssence = 100;
    private int attackCost = 2;

    [SerializeField] private int weaponDamage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Attack(weaponType, weaponRange, weaponDamage, weaponEffect, attackCost);
        }
    }

    protected override void Attack(WeaponType weapon, int range, int damage, WeaponEffect effect, int cout)
    {
        numEssence -= cout;
        List<Ennemy> ennemiesInRange = GetEnnemiesInRange();
        for(int i = 0 ; i < ennemiesInRange.Count - 1 ; i++)
        {
            ennemiesInRange[i].Damage(weaponDamage);
        }
    }

    private List<Ennemy> GetEnnemiesInRange()
    {
        List<Ennemy> ennemiesInRange;
        switch (weaponType)
        {
            case WeaponType.SWORD:
                Debug.Log("épée");
                //fonction de calcul position
                ennemiesInRange = null;
                return ennemiesInRange;
            case WeaponType.GRIMOIRE:
                Debug.Log("grimoire");
                //fonction de calcul position
                ennemiesInRange = null;
                return ennemiesInRange;
            case WeaponType.GUN:
                Debug.Log("flingue");
                //fonction de calcul position
                ennemiesInRange = null;
                return ennemiesInRange;
        }
        return null;
    }

    //function to take damage / die
    public override void Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
