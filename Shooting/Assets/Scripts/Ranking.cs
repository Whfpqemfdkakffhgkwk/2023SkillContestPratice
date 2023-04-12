using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    [SerializeField] private Text RankScoreCurrent;
    [SerializeField] private Text[] RankScoreText, RankText;
    private float[] bestScore = new float[5];
    private float[] rankScore = new float[5];
    //현재 플레이어의 점수와 이름을 받아서 실행

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Main")
        {
            ScoreView();
        }
    }
    public void ScoreSet(float currentScore)
    {
        // 일단 현재에 저장하고 시작
        PlayerPrefs.SetFloat("CurrentPlayerScore", currentScore);
        float tmpScore = 0f;
        for (int i = 0; i < 5; i++)
        {
            // 저장된 최고점수와 이름을 가져오기
            bestScore[i] = PlayerPrefs.GetFloat(i + "BestScore");

            //현재 점수가 랭킹에 오를 수 있을 때
            while (bestScore[i] < currentScore)
            {
                //자리바꾸기!
                tmpScore = bestScore[i];
                bestScore[i] = currentScore;
                //랭킹에 저장
                PlayerPrefs.SetFloat(i + "BestScore", currentScore);
                //다음 반복을 위한 준비
                currentScore = tmpScore;
            }
        }
        //랭킹에 맞춰 점수와 이름 저장
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
        }
    }

    void ScoreView()
    {
        RankScoreCurrent.text = string.Format("{0:#,###}점", PlayerPrefs.GetFloat("CurrentPlayerScore"));
        //랭킹에 맞춰 불러온 점수와 이름을 표시하는 부분
        for (int i = 0; i < 5; i++)
        {
            rankScore[i] = PlayerPrefs.GetFloat(i + "BestScore");
            RankScoreText[i].text = string.Format("{0:#,###}점", rankScore[i]);
            //강조 표시
            if (RankScoreCurrent.text == RankScoreText[i].text)
            {
                Color Rank = new Color(255, 225, 0);
                RankText[i].color = Rank;
                RankScoreText[i].color = Rank;
            }
        }
    }
}
