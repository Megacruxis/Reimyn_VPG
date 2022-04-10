using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ButtonClicked();
        }

    }

    public void ButtonClicked()
    {
        SceneManager.LoadScene("Scenes/WIP_Scene/MainMenu");
    }
}
