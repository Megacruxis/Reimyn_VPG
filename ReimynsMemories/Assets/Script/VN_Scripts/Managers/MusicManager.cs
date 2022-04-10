using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public List<AudioSource> musics = new List<AudioSource>();
    public static MusicManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMusic(int idMusic)
    {
        Debug.Log(idMusic);
        SetMusicActive(idMusic, true);
    }

    public void StopMusic(int idMusic)
    {
        SetMusicActive(idMusic, false);
    }

    private void SetMusicActive(int idMusic, bool active)
    {
        if (musics.Count > idMusic)
        {        
            if (active)
                musics[idMusic].Play();
            else
                musics[idMusic].Stop();
        }
            
        else
            Debug.Log("No song with id " + idMusic);
    }
}
