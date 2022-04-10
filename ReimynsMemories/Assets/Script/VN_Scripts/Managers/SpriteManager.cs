using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public List<GameObject> sprites = new List<GameObject>();
    public static SpriteManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void HideReyminSprite()
    {
        for (int i = 5; i < 14; i ++)
            SetSpriteActive(i, false);
    }

    public void HideAleebuSprite()
    {
        for (int i = 0; i < 5; i++)
            SetSpriteActive(i, false);
    }

    public void DisplaySprite(int idSprite)
    {
        SetSpriteActive(idSprite, true);
    }

    public void HideSprite(int idSprite)
    {
        SetSpriteActive(idSprite, false);
    }

    private void SetSpriteActive(int idSprite, bool active)
    {
        if (sprites.Count > idSprite)
            sprites[idSprite].SetActive(active);
        else
            Debug.Log("No sprite with id " + idSprite);
    }
}
