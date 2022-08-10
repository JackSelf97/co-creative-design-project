using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interaction_Script : MonoBehaviour
{
    public GameObject panel;
    public bool usingPanel;
    public GameObject buttons;

    void Start()
    {
        usingPanel = !usingPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel()
    {
        usingPanel = true;
        buttons.SetActive(false);
        panel.SetActive(true);
    }

    public void ClosePanel()
    {

        usingPanel = false;
        buttons.SetActive(true);
        panel.SetActive(false);
    }
}
