using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=YMj2qPq9CP8&t=274s&ab_channel=Brackeys
public class UI_Manager_Script : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public Text progressTxt;

    public void SwitchScenes(string sceneName)
    {
        if (sceneName == "Quit")
        {
            Application.Quit();
            Debug.Log("Quiting game...");
            return;
        }

        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // store the status
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            loadingBar.value = progress;
            progressTxt.text = progress * 100f + "%";

            yield return null; // wait a frame
        }
    }

}
