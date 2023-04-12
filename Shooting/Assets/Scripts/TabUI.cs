using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    [SerializeField] Ranking rank;
    [SerializeField] GameObject EndWindow;
    [SerializeField] Text BossHP, PlayTime, Score, State;
    [SerializeField] BossPattern Boss;

    float time;
    bool endWindowOn = false;
    public bool PlayerDead = false;
    void Update()
    {
        time += Time.deltaTime;
        BossHP.text = $"{Boss.Hp} / 10000";
        PlayTime.text = $"{((int)time / 60 % 60).ToString("D2")} : {((int)time % 60).ToString("D2")}";
        if (EndWindow.activeSelf && !endWindowOn)
        {
            if(PlayerDead)
            {
                Score.text = $"Score : {((time * 230f) / 2f).ToString("F0")}";
                rank.ScoreSet((time * 230f) / 2f);
                State.text = "Game Over..";
            }
            else
            {
                Score.text = $"Score : {((500f - time) * 230f).ToString("F0")}";
                rank.ScoreSet((500f - time) * 230f);
                State.text = "Game Clear!";
            }
            endWindowOn = true;
            Time.timeScale = 0;
        }
    }

    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
