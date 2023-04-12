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
    //���� �÷��̾��� ������ �̸��� �޾Ƽ� ����

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Main")
        {
            ScoreView();
        }
    }
    public void ScoreSet(float currentScore)
    {
        // �ϴ� ���翡 �����ϰ� ����
        PlayerPrefs.SetFloat("CurrentPlayerScore", currentScore);
        float tmpScore = 0f;
        for (int i = 0; i < 5; i++)
        {
            // ����� �ְ������� �̸��� ��������
            bestScore[i] = PlayerPrefs.GetFloat(i + "BestScore");

            //���� ������ ��ŷ�� ���� �� ���� ��
            while (bestScore[i] < currentScore)
            {
                //�ڸ��ٲٱ�!
                tmpScore = bestScore[i];
                bestScore[i] = currentScore;
                //��ŷ�� ����
                PlayerPrefs.SetFloat(i + "BestScore", currentScore);
                //���� �ݺ��� ���� �غ�
                currentScore = tmpScore;
            }
        }
        //��ŷ�� ���� ������ �̸� ����
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat(i + "BestScore", bestScore[i]);
        }
    }

    void ScoreView()
    {
        RankScoreCurrent.text = string.Format("{0:#,###}��", PlayerPrefs.GetFloat("CurrentPlayerScore"));
        //��ŷ�� ���� �ҷ��� ������ �̸��� ǥ���ϴ� �κ�
        for (int i = 0; i < 5; i++)
        {
            rankScore[i] = PlayerPrefs.GetFloat(i + "BestScore");
            RankScoreText[i].text = string.Format("{0:#,###}��", rankScore[i]);
            //���� ǥ��
            if (RankScoreCurrent.text == RankScoreText[i].text)
            {
                Color Rank = new Color(255, 225, 0);
                RankText[i].color = Rank;
                RankScoreText[i].color = Rank;
            }
        }
    }
}
