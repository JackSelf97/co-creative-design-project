using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Script : MonoBehaviour
{
    public enum UnitType { playerArcher, playerSpear, enemyMelee, enemyRanged, enemyHealer};
    [SerializeField] UnitType unitType;
    [SerializeField] GameObject projectile;
    [SerializeField] float damageValue = 3;
    public float healthValue = 10;
    public float maxHealth;

    private void Start()
    {
        maxHealth = healthValue;
    }

    private void Update()
    {
        if (healthValue <= 0)
        {
            GameManager.gMan.enemyList.Remove(gameObject); // remove enemy from list
            Destroy(gameObject);
        }
    }

    public void DetermineAttack(GameObject target)
    {
        if (target != null)
        {
            if (target.GetComponent<Combat_Script>().healthValue > 0)
            {
                //check type of unit
                //call Attack script attached to the gameObject based on the unit type
                switch (unitType)
                {
                    case UnitType.playerArcher:
                        FireProjectile(target);
                        break;
                    case UnitType.playerSpear:
                        MeleeAttack(target);
                        break;
                    case UnitType.enemyMelee:
                        MeleeAttack(target);
                        break;
                    case UnitType.enemyRanged:
                        FireProjectile(target);
                        break;
                    case UnitType.enemyHealer:
                        HealUnit(target);
                        break;
                }
            }
        }
    }

    private void HealUnit(GameObject target)
    {
            target.GetComponent<Combat_Script>().healthValue += damageValue;
            Debug.Log("Enemy Healed");
    }

    private void FireProjectile(GameObject target)
    {
        projectile.GetComponent<Projectile_Script>().target = target;
        Instantiate(projectile, this.gameObject.transform);
    }

    private void MeleeAttack(GameObject target)
    {
        target.GetComponent<Combat_Script>().healthValue -= damageValue;
    }
}
