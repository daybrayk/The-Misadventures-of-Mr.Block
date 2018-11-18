using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    private int _highScore;
    private int _currency;
    private int _score;
    // Use this for initialization
    void Start () {
        LoadGameData();
        _score = 0;
	}

    public void CheckScore(int score)
    {
        if (score > highScore)
            SaveGameData();
    }

    /*************** Data Persistance ***************/
    private void SaveGameData()
    {
        if(score > highScore)
            PlayerPrefs.SetInt("highScore", score);
        PlayerPrefs.SetInt("currency", currency);
    }

    private void LoadGameData()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            _highScore = PlayerPrefs.GetInt("highScore");
        }
        if (PlayerPrefs.HasKey("currency"))
        {
            _currency = PlayerPrefs.GetInt("currency");
        }
    }

    /*************** Getters and Setters ***************/
    public int highScore
    {
        get { return _highScore; }
    }

    public int currency
    {
        get { return _currency; }
        set { _currency += value; }
    }

    public int score
    {
        get { return _score; }
        set { score += value; }
    }
}
