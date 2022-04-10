using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    List<string> data = new List<string>();
    public string lastSpeaker = "";
    public bool test = false;
    private string storiesFolder;
    public List<Button> actionsButton = new List<Button>();

    [HideInInspector]
    public int progress;
    [HideInInspector]
    public bool auto = false;
    private static int autoTimerMax = 600;
    private int autoTimer = autoTimerMax;

    [HideInInspector]
    public bool skip = false;
    private static int skipTimerMax = 30;
    private int skipTimer = skipTimerMax;
    private bool interactable = true;
    [HideInInspector]
    public bool next = false;

    [HideInInspector]
    public bool inSegment = false;
    [HideInInspector]
    public int posSegment = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        storiesFolder = "";
        LoadChapterFile("chapter0_start");
    }
    public void Auto()
    {
        auto = !auto;
    }

    public void Skip()
    {
        skip = true;
    }

    public void Next()
    {
        if (interactable)
            next = true;
    }

    public void SetInteraction(bool interaction)
    {
        interactable = interaction;
        foreach(Button b in actionsButton)
        {
            b.interactable = interaction;
        }
    }

    public void Back()
    {
        if (inSegment)
        {
            if (progress == 0)
            {
                Debug.Log("Can't go back from 0");
            }
            else
            {
                if (data[progress - 1].StartsWith("choice"))
                {
                    Debug.Log("Can't go back from choice");
                }
                else
                {
                    StopAllCoroutines();
                    ChoiceScreen.Hide();
                    progress--;
                    handlingChapterFile = StartCoroutine(HandleChapterFile());
                }
            }
        } else
        {
            if (progress < 2)
            {
                Debug.Log("Can't go back from 0 or 1");
            }
            else
            {
                if (data[progress - 2].StartsWith("choice"))
                {
                    Debug.Log("Can't go back from choice");
                }
                else
                {
                    StopAllCoroutines();
                    ChoiceScreen.Hide();
                    revertSprite(data[progress - 2]);
                    progress--;
                    progress--;
                    handlingChapterFile = StartCoroutine(HandleChapterFile());
                }
            }
        }
    }

    public void revertSprite(string rawLine)
    {
        CLM.Line line = CLM.Interpret(rawLine);
        foreach(string action in line.actions)
        {
            HandleRevertAction(action);
        }
    }

    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    Coroutine handlingChapterFile = null;
    IEnumerator HandleChapterFile()
    {
        next = false;
        while (progress < data.Count)
        {
            //Next trigger
            if (next)
            {   

                string line = data[progress];
                Debug.Log("HandleChapterFile : line " + line);
                if (line.StartsWith("choice") )
                {
                    yield return HandlingChoiceLine(line);
                }
                else
                {
                    Debug.Log("HandleChapterFile : progress " + progress);
                    HandleLine(line);
                    progress++;
                    while(isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator HandlingChoiceLine(string choice)
    {
        skip = false;
        List<string> choices = new List<string>();
        List<List<string>> actions = new List<List<string>>();
        string line;
        while (true)
        {
            progress++;
            line = data[progress];

            if (line == "{")
                continue;

            if (line != "}")
            {
                line = data[progress];
                Debug.Log(line);
                choices.Add(line.Split('"')[1]);
                int i = 1;
                List<string> acts = new List<string>();
                while ( i <= int.Parse(line.Split('"')[2]))
                {
                    acts.Add(data[progress + i].Replace("\t", ""));
                    i++;
                }
                actions.Add(acts);
                progress = progress + i -1;
            } 
            else
            {
                break;
            }

        }

        Debug.Log("Choice count : " + choices.Count);
        foreach(string c in choices)
        {
            Debug.Log(c);
        }
        if (choices.Count > 0)
        {
            ChoiceScreen.Show(choices.ToArray());


            while (ChoiceScreen.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();

            Debug.Log("Choice : " + ChoiceScreen.lastChoiceMade.index);
            List<string> action = actions[ChoiceScreen.lastChoiceMade.index];
            ChoiceScreen.lastChoiceMade.index = -1;

            Next();
            for (int i = 0; i < action.Count; i ++)
            {
                while(!next)
                    yield return new WaitForEndOfFrame();
                HandleLine(action[i]);
            }
           

            while (isHandlingLine)
                yield return new WaitForEndOfFrame();
        } else
        {
            Debug.Log("Invalid choice operator no choice were found");
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        if (auto)
        {
            if (autoTimer == 0)
            {
                Next();
                autoTimer = autoTimerMax;
            }
            autoTimer--;
        }

        if (skip)
        {
            if (skipTimer == 0)
            {
                Next();
                skipTimer = skipTimerMax;
            }
            skipTimer--;
        }
        //testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }

    public void LoadChapterFile(string fileName)
    {
        Debug.Log("Load chapter : " + storiesFolder  + fileName);
        data = new List<string>(Resources.Load<TextAsset>(fileName).ToString().Split("\n"[0]));
        if (handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        if (handlingLine != null)
            StopCoroutine(handlingLine);

        progress = 0;
        Next();
        handlingChapterFile = StartCoroutine(HandleChapterFile());
    }



    void StopHandlingLine()
    {
        if (isHandlingLine)
            StopCoroutine(handlingLine);
        handlingLine = null;
        inSegment = false;
    }

    void HandleLine(string Rawline)
    {

        CLM.Line line = CLM.Interpret(Rawline);

        StopHandlingLine();
        Debug.Log("Handle Line : Line  " + Rawline);
        handlingLine = StartCoroutine(HandleLineC(line));
    }


    public bool isHandlingLine { get { return handlingLine != null;  } }
    Coroutine handlingLine = null; 
    IEnumerator HandleLineC(CLM.Line line)
    {

        int lineProgress = 0;
        inSegment = true;
        posSegment = 0;
        while (lineProgress < line.segments.Count)
        {
            posSegment++;
            next = false;       
            CLM.Line.Segment segment = line.segments[lineProgress];

            if (lineProgress > 0)
            {
                if (segment.trigger == CLM.Line.Segment.Trigger.autoDelay)
                {
                    if (skip)
                        break;

                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (next)
                            break;
                    }
                } else
                {
                    while(! next)
                    {
                        yield return new WaitForEndOfFrame();              
                    }
                }
            }
            next = false;
            segment.Run();

            while(segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
            }

            lineProgress++;
            yield return new WaitForEndOfFrame();
        }
        inSegment = false;
        foreach (string action in line.actions)
        {
            HandleAction(action);
        }
        handlingLine = null;
        
    }



    public void HandleRevertAction(string action)
    {
        string[] data = action.Split('(', ')');

        //switch actions types
        switch (data[0])
        {
            case "displaySprite":
                HandleHideSprite(data[1]);
                break;
            case "hideSprite":
                HandleDisplaySprite(data[1]);
                break;

            case "load":
            case "setBackground":
            case "playSong":
            case "stopSong":
            case "startMemory":
            case "setInfos":
                //Nothing no reveartable
                break;
            default:
                Debug.Log("Unknow command or non revertable " + data[0]);
                break;
        }
    }

    public void HandleAction(string action)
    {
        string[] data = action.Split('(', ')');

        //switch actions types
        switch (data[0])
        {
            case "load":
                HandleLoad(data[1]);
                break;
            case "setBackground":
                HandleSetBackground(data[1]);
                break;
            case "displaySprite":
                HandleDisplaySprite(data[1]);
                break;
            case "hideSprite":
                HandleHideSprite(data[1]);
                break;
            case "playSong":
                HandlePlaySong(data[1]);
                break;
            case "stopSong":
                HandleStopSong(data[1]);
                break;
            case "startMemory":
                HandleStartMemory(data[1]);
                break;
            case "setInfos":
                HandleSetInfos(data[1]);
                break;
            default:
                Debug.Log("Unknow command " + data[0]);
                break;
        }
    }

    void HandleSetInfos(string infos)
    {
        Infos.starterDeck = infos;
    }

    void HandleStartMemory(string infos)
    {
        SceneManager.LoadScene("Scenes/" + infos);
    }

    void HandlePlaySong(string infos)
    {
        MusicManager.instance.PlayMusic(int.Parse(infos));
    }

    void HandleStopSong(string infos)
    {
        MusicManager.instance.StopMusic(int.Parse(infos));
    }

    void HandleSetBackground(string infos)
    {
        BackgroundManager.instance.SetBackground(int.Parse(infos));
    }

    void HandleDisplaySprite(string infos)
    {
        SpriteManager.instance.DisplaySprite(int.Parse(infos));
    }

    void HandleHideSprite(string infos)
    {
        if (infos.ToLower() == "reimyn")
            SpriteManager.instance.HideReyminSprite();
        else if (infos.ToLower() == "aleebu")
            SpriteManager.instance.HideAleebuSprite();
        else
            SpriteManager.instance.HideSprite(int.Parse(infos));
    }

    void HandleLoad(string infos)
    {
        LoadChapterFile(infos);
    }
}
