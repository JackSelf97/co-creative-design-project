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
}
