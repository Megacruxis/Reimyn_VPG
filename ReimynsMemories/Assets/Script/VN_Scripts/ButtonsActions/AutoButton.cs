using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoButton : MonoBehaviour
{
    private bool active = false;
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
        active = !active;
        ColorBlock newC = gameObject.GetComponent<Button>().colors;
        if (active)
            newC.normalColor = Color.red;
        else
            newC.normalColor = Color.white;

        gameObject.GetComponent<Button>().colors = newC;
        transform.Translate(Vector2.zero);
        NovelController.instance.Auto();
    }

}
