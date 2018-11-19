using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    private static DataManager _instance = null;
    public delegate void ScoreChange();
    public static event ScoreChange scoreChange;
    public bool scoreStart;
    private int _currency;
    private int _score;
    private float _timer;
    private float _scoreTimer;

    // Use this for initialization
    void Awake () {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
            DestroyImmediate(gameObject);
        scoreStart = false;
        LoadGameData();
        _score = 0;
	}

    private void Update()
    {
        _timer += Time.deltaTime;
        _scoreTimer += Time.deltaTime;
        if (_timer > 30f)
            SaveGameData();
        if (_scoreTimer > 2.0f && scoreStart)
        {
            score += 1;
            _scoreTimer = 0;
        }
    }

    /*************** Data Persistance ***************/
    public void SaveGameData()
    {
        if(score > highScore)
            PlayerPrefs.SetInt("highScore", score);
        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.SetFloat("musicVol", AudioManager.instance.musicSource.volume);
        PlayerPrefs.SetFloat("sfxVol", AudioManager.instance.sfxSource.volume);
        PlayerPrefs.SetInt("musicOnOff", AudioManager.instance.musicSource.mute ? 0 : 1);
        PlayerPrefs.SetInt("sfxOnOff", AudioManager.instance.sfxSource.mute ? 0 : 1);
        PlayerPrefs.Save();
        _timer = 0f;
    }

    public void LoadGameData()
    {
        Debug.Log("Programmer Log: Loading Player Data");
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            Debug.Log("Loaded High Score: " + highScore);
        }

        if (PlayerPrefs.HasKey("currency"))
        {
            _currency = PlayerPrefs.GetInt("currency");
            Debug.Log("Loaded Currency: " + _currency);
        }

        if (PlayerPrefs.HasKey("musicOnOff"))
        {
            AudioManager.instance.musicSource.mute = PlayerPrefs.GetInt("musicOnOff") != 0;
            Debug.Log("Loaded Music On/Off: " + AudioManager.instance.musicSource.mute);
        }

        if (PlayerPrefs.HasKey("musicVol"))
        {
            AudioManager.instance.musicSource.volume = PlayerPrefs.GetFloat("musicVol");
            Debug.Log("Music Volume: " + AudioManager.instance.musicSource.volume);
        }

        if(PlayerPrefs.HasKey("sfxOnOff"))
        {
            AudioManager.instance.sfxSource.mute = PlayerPrefs.GetInt("sfxOnOff") != 0;
            Debug.Log("Loaded SFX On Off: " + AudioManager.instance.sfxSource.mute);
        }

        if (PlayerPrefs.HasKey("sfxVol"))
        {
            AudioManager.instance.sfxSource.volume = PlayerPrefs.GetFloat("sfxVol");
            Debug.Log("Loaded SFX Volume: " + AudioManager.instance.sfxSource.volume);
        }
    }

    /*************** Getters and Setters ***************/
    public static DataManager instance{
        get { return _instance; }
    }

    public int highScore { get; private set; }

    public int currency
    {
        get { return _currency; }
        set { _currency += value; }
    }

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (score > highScore)
                highScore = score;
            if(scoreChange != null)
                scoreChange();
        }
    }
}
