using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public GameObject occupiedTile = null;
    private const int tileLayer = 8;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == tileLayer)
        {
            if (occupiedTile == null)
            {
                occupiedTile = other.gameObject;
                occupiedTile.GetComponent<Tile_Script>().gameObject.SetActive(false);
            }
        }
    }
}
