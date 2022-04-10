using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceScreen : MonoBehaviour
{
    public static ChoiceScreen instance;

    public GameObject root;

    public ChoiceButton choicePrefab;

    static List<ChoiceButton> choices = new List<ChoiceButton>();

    public VerticalLayoutGroup layoutGroup;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    public static void Show(params string[] choices)
    {
        instance.root.SetActive(true);
        if (isShowingChoices)
            instance.StopCoroutine(showingChoices);

        showingChoices = instance.StartCoroutine(ShowingChoices(choices));
    }

    public static void Hide()
    {
        if (isShowingChoices)
            instance.StopCoroutine(showingChoices);
        showingChoices = null;

        ClearAllCurentChoices();

        instance.root.SetActive(false);
    }

    static void ClearAllCurentChoices()
    {
        foreach(ChoiceButton b in choices)
        {
            DestroyImmediate(b.gameObject);
        }
        choices.Clear();
    }

    public static bool isWaitingForChoiceToBeMade { get { return isShowingChoices && !lastChoiceMade.hasBeenMade; } }
    public static bool isShowingChoices { get { return showingChoices != null; } }
    static Coroutine showingChoices = null;
    public static IEnumerator ShowingChoices(string[] choices)
    {
        yield return new WaitForEndOfFrame();//For header not done yet and will probably not done by the end
        lastChoiceMade.Reset();

        for (int i = 0; i < choices.Length; i ++)
        {
            CreateChoice(choices[i]);
        }

        while (isWaitingForChoiceToBeMade)
            yield return new WaitForEndOfFrame();

        Hide();
        
    }

    static void CreateChoice(string choice)
    {
        GameObject ob = Instantiate(instance.choicePrefab.gameObject, instance.choicePrefab.transform.parent);
        ob.SetActive(true);
        ChoiceButton b = ob.GetComponent<ChoiceButton>();

        b.text = choice;
        b.choiceIndex = choices.Count;

        choices.Add(b);
    }

    [System.Serializable]
    public class Choice
    {
        public bool hasBeenMade { get { return title != null && index != -1;  } }
        public string title = "";
        public int index = -1;

        public void Reset()
        {
            title = "";
            index = -1;
        }
    }
    public Choice choice = new Choice();
    public static Choice lastChoiceMade { get { return instance.choice; } }

    public void MakeChoice(ChoiceButton button)
    {
        choice.index = button.choiceIndex;
        choice.title = button.text;

    }

    public void MakeChoice(string choiceTitle)
    {
        foreach(ChoiceButton b in choices)
        {
            if (b.text.ToLower() == choiceTitle.ToLower())
            {
                MakeChoice(b);
                return;
            }

        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (choices.Count > choiceIndex)
            MakeChoice(choices[choiceIndex]);
    }
}
