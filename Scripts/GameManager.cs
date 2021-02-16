using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameObject GameOverPanel;
    public GameObject fadeInPanel;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI BestText;

    public GameObject touchToStartObj;

    public GameObject instructionPanel;

    float hueValueForBackgroundColor;
    int currentScore;
    int loadManager;

    void Start()
    {
        StartCoroutine(FadeIn());
        loadManager = PlayerPrefs.GetInt("loadManager");
        if (loadManager == 0)
        {
            HowToButton();
        }
        SetBackgroundColor();
        GetBestScore();
        InitScore();
    }

    void GetBestScore()
    {
        BestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }

    void InitScore()
    {
        currentScore = 0;
        currentScoreText.text = currentScore.ToString();
    }


    void SetBackgroundColor()
    {
        hueValueForBackgroundColor = Random.Range(0, 10) / 10.0f;
        Camera.main.backgroundColor = Color.HSVToRGB(hueValueForBackgroundColor, 0.6f, 0.8f);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        currentScoreText.text = currentScore.ToString();

        CompareWithBestScore();
    }

    void CompareWithBestScore()
    {
        if (currentScore > PlayerPrefs.GetInt("BestScore", 0))
        {
            BestScoreText.text = currentScore.ToString();
            PlayerPrefs.SetInt("BestScore", currentScore);
        }
    }

    IEnumerator FadeIn()
    {
        fadeInPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        fadeInPanel.SetActive(false);
        yield break;
    }


    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("loadManager", 1);
    }


    public IEnumerator GameoverCoroutine()
    {
        currentScoreText.color = Color.white;
        BestScoreText.color = Color.white;
        BestText.color = Color.white;
        GameOverPanel.SetActive(true);
        yield break;
    }


    public void HowToButton()
    {
        instructionPanel.SetActive(true);
    }

    public void CloseButton()
    {
        instructionPanel.SetActive(false);
    }


    public void StartGame(){
        touchToStartObj.SetActive(false);
    }

}
