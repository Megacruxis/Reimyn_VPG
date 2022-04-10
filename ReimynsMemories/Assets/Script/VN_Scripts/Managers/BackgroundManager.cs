using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public List<GameObject> backgrounds = new List<GameObject>();
    public static BackgroundManager instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Set the background to display if idBackground correspond to no background inactive all background  
    /// </summary>
    /// <param name="idBackground"></param>
    public void SetBackground(int idBackground)
    {
        foreach(GameObject background in backgrounds)
        {
            background.SetActive(false);
        }

        if (backgrounds.Count > idBackground)
            backgrounds[idBackground].SetActive(true);
    }


}
