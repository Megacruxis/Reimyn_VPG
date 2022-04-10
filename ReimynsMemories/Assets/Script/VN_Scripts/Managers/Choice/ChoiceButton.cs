using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    public TextMeshProUGUI tmpro;
    public string text { get { return tmpro.text; } set { tmpro.text = value; } }
    [HideInInspector]
    public int choiceIndex = -1;

    public void ButtonClicked()
    {
        Debug.Log("Clicked on button " + choiceIndex);
        ChoiceScreen.lastChoiceMade.index = choiceIndex;
        NovelController.instance.Next();
        ChoiceScreen.Hide();
    }

}
