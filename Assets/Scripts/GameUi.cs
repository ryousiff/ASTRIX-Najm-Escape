using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUi : MonoBehaviour
{
    private Label scoreLabel;
    private Label highestScoreLabel;
    private Button restartButton;
    private VisualElement gameOverContainer;

    private int score = 0;
    private int highestScore = 0;
    private bool isGameOver = false;

    private void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        VisualElement root = uiDocument.rootVisualElement;

        scoreLabel = root.Q<Label>("ScoreLabel");
        highestScoreLabel = root.Q<Label>("HighestScore");
        restartButton = root.Q<Button>("RestartButton");
        gameOverContainer = root.Q<VisualElement>("GameOverContainer");

        Debug.Log(scoreLabel == null ? "ScoreLabel NOT found" : "ScoreLabel found");
        Debug.Log(highestScoreLabel == null ? "HighestScore NOT found" : "HighestScore found");
        Debug.Log(restartButton == null ? "RestartButton NOT found" : "RestartButton found");
        Debug.Log(gameOverContainer == null ? "GameOverContainer NOT found" : "GameOverContainer found");

        highestScore = PlayerPrefs.GetInt("HighestScore", 0);

        UpdateScore();
        UpdateHighestScore();

        if (gameOverContainer != null)
            gameOverContainer.style.display = DisplayStyle.None;

        if (restartButton != null)
        {
            restartButton.style.display = DisplayStyle.Flex;
            restartButton.clicked += RestartGame;
        }
    }

    public void UpdateScore(int newScore)
    {
        if (isGameOver) return;

        score = newScore;

        if (scoreLabel != null)
            scoreLabel.text = "Score: " + score;
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (score > highestScore)
        {
            highestScore = score;
            PlayerPrefs.SetInt("HighestScore", highestScore);
            PlayerPrefs.Save();
        }

        UpdateHighestScore();

        if (gameOverContainer != null)
            gameOverContainer.style.display = DisplayStyle.Flex;

        if (restartButton != null)
            restartButton.style.display = DisplayStyle.Flex;

        Debug.Log("ShowGameOver ran");
    }

    private void UpdateScore()
    {
        if (scoreLabel != null)
            scoreLabel.text = "Score: " + score;
    }

    private void UpdateHighestScore()
    {
        if (highestScoreLabel != null)
            highestScoreLabel.text = "Highest Score: " + highestScore;
    }

    private void RestartGame()
    {
        Debug.Log("Restart clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}