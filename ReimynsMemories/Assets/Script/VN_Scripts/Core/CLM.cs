using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLM : MonoBehaviour
{
    public static Line Interpret(string rawLine)
    {
        return new Line(rawLine);
    }

    public class Line
    {
        public string speaker = "";
        public List<Segment> segments = new List<Segment>();
        public List<string> actions = new List<string>();

        public Line(string rawLine)
        {
            string[] dialogueAndAction = rawLine.Split('"');
            char actionSpliter = ' ';
            string[] actionsArr = dialogueAndAction.Length == 3 ? dialogueAndAction[2].Split(actionSpliter) : dialogueAndAction[0].Split(actionSpliter);

            if (dialogueAndAction.Length == 3)
            {
                speaker = dialogueAndAction[0] == "" ? NovelController.instance.lastSpeaker : dialogueAndAction[0];
                if (speaker[speaker.Length - 1] == ' ')
                    speaker = speaker.Remove(speaker.Length - 1);

                NovelController.instance.lastSpeaker = speaker;
                SegmentDialogue(dialogueAndAction[1]);
            }
            for (int i = 0; i < actionsArr.Length; i ++)
            {
                actions.Add(actionsArr[i]);
            }
        }


        void SegmentDialogue(string dialogue)
        {
            segments.Clear();
            string[] parts = dialogue.Split('{', '}');

            for (int i = 0; i < parts.Length; i ++)
            {
                Segment segment = new Segment();
                bool isOdd = i % 2 != 0;

                if (isOdd)
                {
                    string[] commandData = parts[i].Split(' ');
                    switch(commandData[0])
                    {
                        case "c": //wait for input and clear
                            segment.trigger = Segment.Trigger.waitClickClear;
                            break;
                        case "a": //wait for input and appand
                            segment.trigger = Segment.Trigger.waitClickClear;
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                        case "w": //wait for set time and clear
                            segment.trigger = Segment.Trigger.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            break;
                        case "wa": //wait for set time and clear
                            segment.trigger = Segment.Trigger.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                    }
                    i++;                   
                }
                segment.dialogue = parts[i];
                segment.line = this;

                segments.Add(segment);
            }
        }

        public class Segment
        {
            public Line line;
            public string dialogue = "";
            public string pretext = "";
            public enum Trigger { waitClick, waitClickClear, autoDelay }
            public Trigger trigger = Trigger.waitClickClear;

            public float autoDelay = 0;

            public void Run()
            {


                if (running != null)
                    NovelController.instance.StopCoroutine(running);
                running = NovelController.instance.StartCoroutine(Running());
            }

            public bool isRunning { get { return running != null; } }
            Coroutine running = null;
            IEnumerator Running()
            {
                if (line.speaker != "narrator")
                {
                    //TODO character management
                    // Character character = CharacterManager.instance.GetCharacter(speaker);
                    // character.Say(dialogue, additive)

                    //Glue before TODO
                    DialogueSystem.instance.Say(dialogue, line.speaker, pretext != "");
                }
                else
                {
                    DialogueSystem.instance.Say(dialogue, line.speaker, pretext != "");
                }

                while (DialogueSystem.instance.isSpeaking)
                    yield return new WaitForEndOfFrame();

                running = null;
                
            }
        }
    }
}
