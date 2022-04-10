using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tuto;
    public GameObject credit;
    public string startGameScene;
    
    public void Exit()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        tuto.SetActive(false);
        credit.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GoToTuto()
    {
        tuto.SetActive(true);
        credit.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void GoToCredit()
    {
        tuto.SetActive(false);
        credit.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startGameScene);
    }


}
