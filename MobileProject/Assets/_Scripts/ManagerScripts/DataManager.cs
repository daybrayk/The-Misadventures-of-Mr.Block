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
        PlayerPrefs.SetFloat("musicVol", AudioManager.instance.musicSource.volume);
        PlayerPrefs.SetFloat("sfxVol", AudioManager.instance.sfxSource.volume);
        PlayerPrefs.SetInt("musicOnOff", AudioManager.instance.musicSource.mute ? 0 : 1);
        PlayerPrefs.SetInt("sfxOnOff", AudioManager.instance.sfxSource.mute ? 0 : 1);
    }

    private void LoadGameData()
    {
        if (PlayerPrefs.HasKey("highScore"))
            _highScore = PlayerPrefs.GetInt("highScore");

        if (PlayerPrefs.HasKey("currency"))
            _currency = PlayerPrefs.GetInt("currency");

        if (PlayerPrefs.HasKey("musicOnOff"))
            AudioManager.instance.musicSource.mute = PlayerPrefs.GetInt("musicOnOff") != 0;

        if (PlayerPrefs.HasKey("musicVol"))
            AudioManager.instance.musicSource.volume = PlayerPrefs.GetFloat("musicVol");

        if(PlayerPrefs.HasKey("sfxOnOff"))
            AudioManager.instance.sfxSource.mute = PlayerPrefs.GetInt("sfxOnOff") != 0;

        if (PlayerPrefs.HasKey("sfxVol"))
            AudioManager.instance.sfxSource.volume = PlayerPrefs.GetFloat("sfxVol");
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
