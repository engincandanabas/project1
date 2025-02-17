using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void ScoreChange(int score);
    public static event ScoreChange OnScoreChanged;


    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Score = 0;
    }
}
