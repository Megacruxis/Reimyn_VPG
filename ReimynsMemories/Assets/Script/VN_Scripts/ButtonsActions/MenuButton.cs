using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //TODO go to main menu or save
        Application.Quit();
    }
}
