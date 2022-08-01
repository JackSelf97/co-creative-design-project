using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Script : MonoBehaviour
{
    public enum UnitType { playerArcher, playerSpear, enemyMelee, enemyRanged};
    [SerializeField] UnitType unitType;
    [SerializeField] float damageValue = 3;
    public float healthValue = 10;

    private void Update()
    {
        if (healthValue <= 0)
        {
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
                        Debug.Log("Player Archer attack!");
                        break;
                    case UnitType.playerSpear:
                        target.GetComponent<Combat_Script>().healthValue -= damageValue;
                        break;
                    case UnitType.enemyMelee:
                        target.GetComponent<Combat_Script>().healthValue -= damageValue;
                        break;
                    case UnitType.enemyRanged:
                        Debug.Log("Enemy Ranged attack!");
                        break;
                }
            }
        }
    }
}
