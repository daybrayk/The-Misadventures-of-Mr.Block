using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour {
    public Text scoreText;
    public Text highScoreText;
    public Text currencyText;
    public Button pauseButton;
    public Button unPauseButton;
    public Toggle musicMute;
    public Toggle sfxMute;
    public Slider musicVolume;
    public Slider sfxVolume;
    public GameObject pausePanel;

    private void Awake()
    {
        musicMute.isOn = !AudioManager.instance.musicSource.mute;
        sfxMute.isOn = !AudioManager.instance.sfxSource.mute;
        musicVolume.value = AudioManager.instance.musicSource.volume;
        sfxVolume.value = AudioManager.instance.sfxSource.volume;
        DataManager.scoreChange += SetUIText;
    }

    private void Start()
    {
        DataManager.instance.LoadGameData();
        musicMute.isOn = AudioManager.instance.musicSource.mute;
        sfxMute.isOn = AudioManager.instance.sfxSource.mute;
        AudioManager.instance.musicSource.Play();
        SetUIText();
        AudioManager.instance.musicSource.Play();
        //Setup Listeners
        pauseButton.onClick.AddListener(PauseGame);
        unPauseButton.onClick.AddListener(PauseGame);
        musicMute.onValueChanged.AddListener(delegate { AudioManager.instance.MuteMusic(); });
        sfxMute.onValueChanged.AddListener(delegate { AudioManager.instance.MuteSFX(); });
        musicVolume.onValueChanged.AddListener(delegate { AudioManager.instance.MusicVolume(musicVolume.value); });
        sfxVolume.onValueChanged.AddListener(delegate { AudioManager.instance.SFXVolume(sfxVolume.value); });
    }

    public void SetUIText()
    {
        scoreText.text = "Score: " + DataManager.instance.score;
        highScoreText.text = "HighScore: " + DataManager.instance.highScore;
        //currencyText.text = "Currency: " + .currency;
    }

    public void RemoveSubscriber()
    {
        DataManager.scoreChange -= SetUIText;
    }

    public void PauseGame()
    {
        if (!GameManager.instance.isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            GameManager.instance.isPaused = true;
            AudioManager.instance.musicSource.Pause();
            AudioManager.instance.sfxSource.Pause();
            return;
        }
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.isPaused = false;
        AudioManager.instance.musicSource.UnPause();
        AudioManager.instance.sfxSource.UnPause();
    }
}
