using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=YMj2qPq9CP8&t=274s&ab_channel=Brackeys
public class UI_Manager_Script : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public Text progressTxt;
    [SerializeField] private float timer = 0, timeLimit = 0.01f;
    [SerializeField] private bool load = false, fakeLoad = false;

    private void Update()
    {
        if (load)
            FakeGameLoad(); // this is restricted to the "Game" scene only
    }

    public void SwitchScenes(string sceneName)
    {
        if (sceneName == "Quit")
        {
            Application.Quit();
            Debug.Log("Quiting game...");
            return;
        }

        if (!fakeLoad)
        {
            StartCoroutine(LoadAsynchronously(sceneName)); // remember to switch the boolean in the inspector to change real/fake load
        }
        else if (fakeLoad)
        {
            load = true;
        }
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

    void FakeGameLoad()
    {
        loadingScreen.SetActive(true);
        timer += Time.deltaTime;
        if (timer >= timeLimit)
        {
            loadingBar.value += .01f;
            progressTxt.text = Mathf.Round(loadingBar.value * 100).ToString() + "%";
            timer = 0;
        }
        if (loadingBar.value == 1)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
