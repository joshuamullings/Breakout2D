using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton

    public static BrickManager Instance => _instance;

    private static BrickManager _instance;

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
    
    public Sprite[] Sprites;
    public Brick BrickPrefab;
    public Color[] BrickColours;
    public int CurrentLevel;

    private GameObject _bricksContainer;
    private int _maxRows = 17;
    private int _maxColumns = 12;
    private float _initialBrickSpawnPositionX = -1.96f;
    private float _initialBrickSpawnPositionY = 3.325f;
    private float _shiftAmount = 0.365f;

    public static event Action OnLevelLoaded;

    public List<int[,]> LevelsData { get; set; }
    public List<Brick> RemainingBricks { get; set; }
    public int InitialBricksCount { get; set; }

    public void LoadLevel(int level)
    {
        CurrentLevel = level;
        ClearRemainingBricks();
        GenerateBricks();
    }

    public void LoadNextLevel()
    {
        CurrentLevel++;

        if (CurrentLevel >= LevelsData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            LoadLevel(CurrentLevel);
        }
    }

    private void Start()
    {
        _bricksContainer = new GameObject("BricksContainer");
        LevelsData = LoadLevelsData();
        GenerateBricks();
    }

    private void ClearRemainingBricks()
    {
        foreach (Brick brick in RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset text = Resources.Load("Levels") as TextAsset;

        // split text block into rows
        string[] rows = text.text.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.RemoveEmptyEntries
        );

        List<int[,]> levelsData = new List<int[,]>();

        int[,] currentLevel = new int[_maxRows, _maxColumns];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            {
                // split text row into characters
                string[] bricks = line.Split(
                    new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries
                );

                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }

                currentRow++;
            }
            else
            {
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[_maxRows, _maxColumns];
            }
        }

        return levelsData;
    }

    private void GenerateBricks()
    {
        RemainingBricks = new List<Brick>();
        
        int[,] currentLevelData = LevelsData[CurrentLevel];
        float currentSpawnX = _initialBrickSpawnPositionX;
        float currentSpawnY = _initialBrickSpawnPositionY;
        float zShift = 0;

        for (int row = 0; row < _maxRows; row++)
        {
            for (int col = 0; col < _maxColumns; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(
                        BrickPrefab,
                        new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift),
                        Quaternion.identity
                    ) as Brick;

                    newBrick.Initialize(
                        _bricksContainer.transform,
                        Sprites[brickType - 1],
                        BrickColours[brickType],
                        brickType
                    );

                    RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += _shiftAmount;

                if (col + 1 == _maxColumns)
                {
                    currentSpawnX = _initialBrickSpawnPositionX;
                }
            }

            currentSpawnY -= _shiftAmount;
        }

        InitialBricksCount = RemainingBricks.Count;
        OnLevelLoaded?.Invoke();
    }
}