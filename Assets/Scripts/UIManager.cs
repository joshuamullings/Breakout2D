using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ScoreText;
    public Text LivesText;
    public Text RemainingText;

    public int Score { get; set; }

    private void Awake()
    {
        Brick.OnBrickDestruction += OnBrickDestruction;
        BrickManager.OnLevelLoaded += OnLevelLoaded;
        GameManager.OnLivesLost += OnLivesLost;
    }

    private void Start()
    {
        OnLivesLost(GameManager.Instance.AvalibleLives);
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateRemainingBricksText();
        UpdateScoreText(10);
    }

    private void UpdateRemainingBricksText()
    {
        RemainingText.text = $"{BrickManager.Instance.RemainingBricks.Count} / {BrickManager.Instance.InitialBricksCount}";
    }

    private void UpdateScoreText(int increment)
    {
        Score += increment;
        string scoreString = Score.ToString().PadLeft(6, '0');
        ScoreText.text = $"{scoreString}";
    }

    private void OnLevelLoaded()
    {
        UpdateRemainingBricksText();
        UpdateScoreText(0);
    }

    private void OnLivesLost(int remainingLives)
    {
        LivesText.text = $"{remainingLives}";
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BrickManager.OnLevelLoaded -= OnLevelLoaded;
    }
}