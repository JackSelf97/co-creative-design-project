using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float attackRange;
    [SerializeField] float attackDelay = 1f;
    float targetDistance;
    float distance;
    string unitTag;
    string playerTag = "Player";
    string enemyTag = "Enemy";
    bool canAttack = true;
    Vector3 moveDir;
    GameObject target;
    GameObject[] opposingUnits;

    



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
                    StartCoroutine(AttackDelay());
                    canAttack = false;
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
        foreach (GameObject opposingUnit in opposingUnits)
        {
            float distance = (gameObject.transform.position - opposingUnit.transform.position).sqrMagnitude;
            targetDistances.Add(distance, opposingUnit);
        }
        List<float> distances = targetDistances.Keys.ToList();
        targetDistance = distances.Min();
        target = targetDistances[targetDistance];
        
    }

    //find objects with opposite tag
    private void GetPossibleTargets()
    {
        if (unitTag == playerTag)
        {
            opposingUnits = GameObject.FindGameObjectsWithTag(enemyTag);
        }
        else if (unitTag == enemyTag)
        {
            opposingUnits = GameObject.FindGameObjectsWithTag(playerTag);
        }
        if (opposingUnits.Length != 0)
        {
            DetermineTarget();
        }
    }
}
