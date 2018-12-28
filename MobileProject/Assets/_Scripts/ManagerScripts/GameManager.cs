using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public delegate void ScoreChange();
    public static event ScoreChange scoreChange;
    private float _timer, _scoreTimer;
    public bool scoreStart;
    private int _score;
    /*************** Persistent Data ***************/
    public int highScore;

    // Use this for initialization
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    private void Start()
    {
        LoadGame();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _scoreTimer += Time.deltaTime;
        if (_timer > 30f)
        {
            DataManager.SaveGame();
            _timer = 0;
        }
        if (_scoreTimer > 2.0f && scoreStart)
        {
            score += 1;
            _scoreTimer = 0;
        }
    }

    public void LoadScene(string sceneName)
    {
        ResetVariables();
        SceneManager.LoadScene(sceneName);
    }

    //Manager variables to be reset on scene reload
    public void ResetVariables()
    {
        //DataManager.instance.scoreStart = false;
        scoreStart = false;
        score = 0;
        AudioManager.instance.musicSource.Stop();
    }

    public void SaveGame()
    {
        DataManager.SaveGame();
    }

    public void LoadGame()
    {
        PersistentData data = DataManager.LoadGame();
        highScore = data.highScore;
        if (AudioManager.instance)
        {
            AudioManager.instance.musicSource.volume = data.musicVolume;
            AudioManager.instance.sfxSource.volume = data.sfxVolume;
            AudioManager.instance.musicSource.mute = data.musicOnOff;
            AudioManager.instance.sfxSource.mute = data.sfxOnOff;
        }
        else
            Debug.Log("<color=red>AudioManager instance does not exist</color>");

    }

    /*************** Getters and Setters ***************/
    public static GameManager instance
    {
        get { return _instance; }
    }

    public bool isPaused { get; set; }

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (score > highScore)
            {
                highScore = score;
                DataManager.SaveGame();
            }
            if (scoreChange != null)
                scoreChange();
        }

    }
}