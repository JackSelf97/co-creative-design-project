using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement_Script : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float attackRange;
    float targetDistance;
    float distance;
    string unitTag;
    string playerTag = "Player";
    string enemyTag = "Enemy";
    Vector3 moveDir;
    GameObject target;
    GameObject[] opposingUnits;
    Dictionary<float, GameObject> targetDistances = new Dictionary<float, GameObject>();



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
                Debug.Log("Attack made!");
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

    //compare distance to find nearest unit and store as target
    private void DetermineTarget()
    {
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
        DetermineTarget();
    }
}
