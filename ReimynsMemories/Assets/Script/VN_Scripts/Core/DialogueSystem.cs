using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    public Elements elements;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Say something new to be added to the text box
    /// </summary>
    /// <param name="s"></param>
    public void SayNew(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        if (parts.Length >= 3)
            ChoiceScreen.Show(parts[2].Split('|'));

        Say(speech, speaker);
    }

    /// <summary>
    /// Say something to be added to what is already on the text box
    /// </summary>
    /// <param name="s"></param>
    public void SayAdd(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        Say(speech, speaker, true);
    }

    /// <summary>
    /// Say something and show it on the speech box
    /// </summary>
    /// <param name="speech"></param> the text to say
    /// <param name="speaker"></param> the text for the name of the speaker
    /// <param name="additive"></param> if you replace or continue the last text
    public void Say(string speech, string speaker =  "", bool additive = false)
    {
        StopSpeaking();
        speaking = StartCoroutine(Speaking(speech, speaker, additive));
    }

    public void StopSpeaking()
    {
        if ( isSpeaking)
        {
            StopCoroutine(speaking); 
        }
        speaking = null;
    }

    private string DetermineSpeaker(string s)
    {
        string retVal = speakerNameText.text;//Default return the curent name
        if (s != speakerNameText.text && s != "")
        {
            retVal = (s.ToLower().Contains("narrator"))? "": s;
        }

        return retVal;
            
    }

    public bool isSpeaking { get { return speaking != null; } }
    [HideInInspector] public bool isWaitingForUserInput = false;
    Coroutine speaking = null;
    IEnumerator Speaking(string speech, string speaker = "", bool additive = false)
    {
        speechPanel.SetActive(true);
        string targetSpeech = speech;
        if (additive)
        {
            targetSpeech = speechText.text + targetSpeech;
        } else
        {
            speechText.text = "";
        }

        speechText.text = "";
        speakerNameText.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = false;

        while(speechText.text != targetSpeech)
        {
            speechText.text += targetSpeech[speechText.text.Length];
            yield return new WaitForEndOfFrame();
        }

        //Text Finished
        //isWaitingForUserInput = true;
        //while (isWaitingForUserInput)
            //yield return new WaitForEndOfFrame();

        StopSpeaking();
    }

    [System.Serializable]
    public class Elements
    {
        /// <summary>
        /// The main panel containing all dialogue related element on the UI
        /// </summary>
        public GameObject speechPanel;
        public Text speakerNameText;
        public Text speechText;
    }
    public GameObject speechPanel { get{return elements.speechPanel; } }
    public Text speakerNameText { get{return elements.speakerNameText; } }
    public Text speechText { get{return elements.speechText; } }
}
