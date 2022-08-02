using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject interactableSlot = null;
    public GameObject activeTile = null;
    public Text roundTxt = null;
    public int roundNo = 0;
    public bool roundStart = false;
    [SerializeField] private GameObject[] playerList, enemyList;

    public GridManager_Script gridMan = null;

    #region Singleton & Awake
    public static GameManager gMan = null; // should always initilize

    private void Awake()
    {
        if (gMan == null)
        {
            DontDestroyOnLoad(gameObject);
            gMan = this;
        }
        else if (gMan != null)
        {
            Destroy(gameObject); // if its already there destroy it
        }
        Application.targetFrameRate = 144; //framerate
    }
    #endregion

    private void Update()
    {
        if (!roundStart) // This whole method is not efficient -- need to think of new approach
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < playerList.Length; i++)
            {
                playerList[i].GetComponent<Movement_Script>().enabled = false;
            }
            for (int i = 0; i < enemyList.Length; i++)
            {
                enemyList[i].GetComponent<Movement_Script>().enabled = false;
            }
        }
        if (roundStart)
        {
            playerList = GameObject.FindGameObjectsWithTag("Player");
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < playerList.Length; i++)
            {
                playerList[i].GetComponent<Movement_Script>().enabled = true;
            }
            for (int i = 0; i < enemyList.Length; i++)
            {
                enemyList[i].GetComponent<Movement_Script>().enabled = true;
            }
        }
    }

    public void AddRound(int roundNumber) // can be used to minus round
    {
        roundNo += roundNumber;
        roundTxt.text = "Round: " + roundNo;
        gridMan.StartNewRound();
        if (roundNo % 5 == 0) // every fifth round, a new enemy is spawned
        {
            gridMan.maxEnemies++;
        }
    }

    public void StartRound()
    {
        roundStart = true;
    }

}