using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ButtonClicked();
        }
        
    }

    public void ButtonClicked()
    {
        NovelController.instance.Back();
    }
}
