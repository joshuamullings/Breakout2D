using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

        public static GameManager Instance => _instance;
        
        private static GameManager _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

    #endregion

    public GameObject GameOverScreen;
    public GameObject VictoryScreen;
    public int AvalibleLives = 3;

    public static event Action<int> OnLivesLost;

    public bool IsGameStarted { get; set; }
    public int Lives { get; set; }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowVictoryScreen()
    {
        VictoryScreen.SetActive(true);
    }

    private void Start()
    {
        Lives = AvalibleLives;
        Screen.SetResolution(600, 900, false);
        Ball.OnBallKill += OnBallKill;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBallKill(Ball obj)
    {
        if (BallManager.Instance.Balls.Count <= 0)
        {
            Lives--;

            if (Lives < 1)
            {
                OnLivesLost(0);
                GameOverScreen.SetActive(true);
            }
            else
            {
                OnLivesLost?.Invoke(Lives);
                BallManager.Instance.ResetBall();
                IsGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance.CurrentLevel);
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallKill -= OnBallKill;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BrickManager.Instance.RemainingBricks.Count <= 0)
        {
            BallManager.Instance.ResetBall();
            GameManager.Instance.IsGameStarted = false;
            BrickManager.Instance.LoadNextLevel();
        }
    }
}