using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ButtonClicked();
        }

    }

    public void ButtonClicked()
    {
        NovelController.instance.Skip();
    }
}
