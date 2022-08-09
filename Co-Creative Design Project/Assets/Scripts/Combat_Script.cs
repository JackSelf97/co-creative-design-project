using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Script : MonoBehaviour
{
    public enum UnitType { playerArcher, playerSpear, playerDagger, enemyMelee, enemyRanged, enemyHealer};
    [SerializeField] UnitType unitType;
    [SerializeField] GameObject projectile;
    [SerializeField] float damageValue = 3;
    private ParticleSystem deathParticle;
    public ParticleSystem damageParticle;
    public float healthValue = 10;
    public float maxHealth;
    public bool canTeleport = false;
   

    private void Start()
    {
        maxHealth = healthValue;
        deathParticle = this.transform.Find("Dust_Particles").GetComponent<ParticleSystem>();
        damageParticle = this.transform.Find("Boom_Particle").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (healthValue <= 0)
        {
            GameManager.gMan.enemyList.Remove(gameObject); // remove enemy from list
            //play death particles. Destroy unit after a short delay;
            if (!deathParticle.isPlaying)
            {
                deathParticle.Play();
            }
            StartCoroutine(UnitDeath());
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
                    case UnitType.playerDagger:
                        MeleeAttack(target);
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
        target.GetComponent<Combat_Script>().damageParticle.Play();
        target.GetComponent<Combat_Script>().healthValue -= damageValue;
    }

    public void Teleport(GameObject target)
    {
        if (target != null)
        {
            Vector3 offset = new Vector3(1, 0, 0);
            gameObject.transform.position = target.transform.position + offset;
            canTeleport = false;
        }
    }

    IEnumerator UnitDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
