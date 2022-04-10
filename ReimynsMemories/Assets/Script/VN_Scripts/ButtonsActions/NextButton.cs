using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ButtonClicked();
        }

    }

    public void ButtonClicked()
    {
        NovelController.instance.Next();
    }
}
