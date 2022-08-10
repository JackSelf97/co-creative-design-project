using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] bool isHealer = false;
    [SerializeField] bool isDaggerUnit = false;
    private ParticleSystem assassinParticles;
    float targetDistance;
    float distance;
    string unitTag;
    string playerTag = "Player";
    string enemyTag = "Enemy";
    bool canAttack = true;
    Vector3 moveDir;
    GameObject target;
    GameObject[] potentialTargets;

    



    void Start()
    {
        //denotes tag attached to current gameObject
        unitTag = gameObject.tag;
        GetPossibleTargets();
        if(isDaggerUnit)
        {
            assassinParticles = this.transform.Find("Assassin_Particles").GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (target != null)
        {
            //if the target unit is in range, trigger the attack
            distance = (target.transform.position - gameObject.transform.position).sqrMagnitude;
            if (distance <= attackRange)
            {
                if (canAttack)
                {
                    //if the unit is a healer, only heal units if they are below their maximum health
                    if (isHealer)
                    {
                        if (target.GetComponent<Combat_Script>().healthValue < target.GetComponent<Combat_Script>().maxHealth)
                        {
                            StartCoroutine(AttackDelay());
                            canAttack = false;
                        }
                    }
                    else
                    {
                        StartCoroutine(AttackDelay());
                        canAttack = false;
                    }
                }
            }

            else
            {
                //if unit is a dagger unit, use teleport ability
                if (isDaggerUnit)
                {
                    if (GetComponent<Combat_Script>().canTeleport)
                    {
                        if (!assassinParticles.isPlaying)
                        {
                            assassinParticles.Play();
                        }
                        StartCoroutine(TeleportDelay());
                    }
                    else
                    {
                        //move towards target unit
                        moveDir = (target.transform.position - gameObject.transform.position);
                        transform.Translate(moveDir * speed * Time.deltaTime);
                    }
                }
                else
                {
                    //move towards target unit
                    moveDir = (target.transform.position - gameObject.transform.position);
                    transform.Translate(moveDir * speed * Time.deltaTime);
                }
            }
            //make unit face backwards if the target is behind it

                if (target.transform.position.x < gameObject.transform.position.x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
        }
        else
        {
            GetPossibleTargets();
        }
    }

    private IEnumerator AttackDelay()
    {
        GetComponent<Combat_Script>().animator.ResetTrigger("Idle");
        GetComponent<Combat_Script>().animator.ResetTrigger("Hit");
        GetComponent<Combat_Script>().animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);
        gameObject.GetComponent<Combat_Script>().DetermineAttack(target);
        canAttack = true;
    }

    private IEnumerator TeleportDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        GetComponent<Combat_Script>().Teleport(target);
    }

    //compare distance to find nearest unit and store as target
    private void DetermineTarget()
    {
        Dictionary<float, GameObject> targetDistances = new Dictionary<float, GameObject>();
        foreach (GameObject opposingUnit in potentialTargets)
        {
            float distance = (gameObject.transform.position - opposingUnit.transform.position).sqrMagnitude;
            targetDistances.Add(distance, opposingUnit);
        }
        List<float> distances = targetDistances.Keys.ToList();
        //if unit is a healer, get the furthest possible target
        if (isHealer || isDaggerUnit)
        {
            targetDistance = distances.Max();
        }
        else
        {
            targetDistance = distances.Min();
        }
        target = targetDistances[targetDistance];
        
    }


    private void GetPossibleTargets()
    {
        //if unit is a healer, find objects with the same tag
        if (isHealer)
        {
            if (unitTag == playerTag)
            {
                potentialTargets = GameObject.FindGameObjectsWithTag(playerTag);
            }
            else if (unitTag == enemyTag)
            {
                potentialTargets = GameObject.FindGameObjectsWithTag(enemyTag);
            }
        }
        else
        {
            //find objects with opposite tag
            if (unitTag == playerTag)
            {
                //if the unit is a dagger unit, enable their teleport when selecting a target
                if (isDaggerUnit)
                {
                    GetComponent<Combat_Script>().canTeleport = true;
                }
                potentialTargets = GameObject.FindGameObjectsWithTag(enemyTag);
            }
            else if (unitTag == enemyTag)
            {
                potentialTargets = GameObject.FindGameObjectsWithTag(playerTag);
            }
        }
        if (potentialTargets.Length != 0)
        {
            DetermineTarget();
        }
    }
}
