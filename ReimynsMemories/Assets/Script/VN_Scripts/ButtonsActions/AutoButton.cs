using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ButtonClicked();
        }

    }

    public void ButtonClicked()
    {
        NovelController.instance.Auto();
    }

}
