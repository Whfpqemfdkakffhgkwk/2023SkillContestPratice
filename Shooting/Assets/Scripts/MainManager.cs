using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject Popup;
    private void Start()
    {
        Time.timeScale = 1;
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Ingame");
    }    

    public void HowToPlay()
    {
        Popup.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
