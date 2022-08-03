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
    }

    private void Update()
    {
        if (target != null)
        {

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
                moveDir = (target.transform.position - gameObject.transform.position);
                transform.Translate(moveDir * speed * Time.deltaTime);
            }
        }
        else
        {
            GetPossibleTargets();
        }
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        gameObject.GetComponent<Combat_Script>().DetermineAttack(target);
        canAttack = true;


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
        if (isHealer)
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
