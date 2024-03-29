﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public GameObject occupiedTile = null;
    private const int tileLayer = 8;

    private void Start()
    {
        GameManager.gMan.enemyList.Add(gameObject); // add enemy into enemyList
        GameManager.gMan.roundList.Add(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameManager.gMan.roundInProgress)
        {
            if (other.gameObject.layer == tileLayer)
            {
                if (occupiedTile == null)
                {
                    occupiedTile = other.gameObject;
                    //occupiedTile.GetComponent<Tile_Script>().gameObject.SetActive(false);
                }
            }
        }
    }
}
