using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SettingsMenu;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingsMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickSettings()
    {
        
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }
    public void Back()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}

