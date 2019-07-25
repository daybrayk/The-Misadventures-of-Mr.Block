using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour {
    #region Game UI
    [Header("Game UI")]
    public Button pauseButton;
    public Text scoreText;
    public Text highScoreText;
    #endregion

    #region Pause Menu
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    [SerializeField] private Button m_pauseMenuContinueBtn;
    [SerializeField] private Button m_pauseMenuSettingsBtn;
    [SerializeField] private Button m_pauseMenuBackBtn;
    [SerializeField] private Text m_pauseMenuDisplayTxt;
    #endregion

    #region Settings Menu
    [Header("Settings Menu")]
    [SerializeField] private GameObject m_settingsMenu;
    public Toggle musicMute;
    public Toggle sfxMute;
    public Slider musicVolume;
    public Slider sfxVolume;
    [SerializeField] private Button m_settingsMenuPlayBtn;
    [SerializeField] private Button m_settingsMenuBackBtn;
    [SerializeField] private Button m_settingsMenuCreditsBtn;
    #endregion

    [SerializeField] InputManager m_inputManager;
    private void Awake()
    {
        if (!GameManager.instance)
        {
            Debug.LogError("GameManager static instance does not exist");
            return;
        }
        GameManager.scoreChange += SetUIText;

        if (!AudioManager.instance)
        {
            Debug.LogError("AudioManager static instance does not exist");
            return;
        }
    }

    private void Start()
    {
        if (!GameManager.instance || !AudioManager.instance)
            return;
        GameManager.instance.LoadGame();

        #region Audio Settings Setup
        musicMute.isOn = AudioManager.instance.musicSource.mute;
        sfxMute.isOn = AudioManager.instance.sfxSource.mute;
        musicVolume.value = AudioManager.instance.musicSource.volume;
        sfxVolume.value = AudioManager.instance.sfxSource.volume;

        AudioManager.instance.musicSource.Play();

        #endregion

        SetUIText();

        #region Game UI Event Listeners
        pauseButton.onClick.AddListener(PauseGame);
        #endregion

        #region Pause Menu Event Listeners
        //m_pauseMenuContinueBtn.onClick.AddListener(PauseGame);
        m_pauseMenuSettingsBtn.onClick.AddListener(ShowSettings);
        m_pauseMenuBackBtn.onClick.AddListener(Exit);
        #endregion

        #region Settings Menu Event Listeners
        m_settingsMenuBackBtn.onClick.AddListener(HideSettings);
        musicMute.onValueChanged.AddListener(delegate { AudioManager.instance.MuteMusic(); });
        sfxMute.onValueChanged.AddListener(delegate { AudioManager.instance.MuteSFX(); });
        musicVolume.onValueChanged.AddListener(delegate { AudioManager.instance.MusicVolume(musicVolume.value); });
        sfxVolume.onValueChanged.AddListener(delegate { AudioManager.instance.SFXVolume(sfxVolume.value); });
        #endregion
    }

    public void SetUIText()
    {
        scoreText.text = "Score: " + GameManager.instance.score;
        highScoreText.text = "HighScore: " + GameManager.instance.highScore;
        //currencyText.text = "Currency: " + .currency;
    }

    public void RemoveSubscriber()
    {
        //GameManager.scoreChange -= SetUIText;
    }

    public void PauseGame()
    {
        if (!GameManager.instance || !AudioManager.instance)
            return;
        if (!GameManager.instance.isPaused)
        {
            pauseMenu.SetActive(true);
            GameManager.instance.isPaused = true;
            AudioManager.instance.musicSource.Pause();
            AudioManager.instance.sfxSource.Pause();
            Time.timeScale = 0;
            return;
        }
        Time.timeScale = 1;
        GameManager.instance.isPaused = false;
        AudioManager.instance.musicSource.UnPause();
        AudioManager.instance.sfxSource.UnPause();
        GameManager.instance.SaveGame();
        m_inputManager.ClearTouches();
        pauseMenu.SetActive(false);
    }

    private void ShowSettings()
    {
        m_settingsMenu.SetActive(true);
    }

    private void HideSettings()
    {
        m_settingsMenu.SetActive(false);
    }

    private void Exit()
    {
        if (!GameManager.instance)
            return;
        PauseGame();    //Unpause before loading new scene because static variables (i.e. Time.timeScale) hold their value between scenes
        GameManager.instance.SaveGame();
        GameManager.instance.LoadScene("TitleScene 1");
    }

    private void OnDestroy()
    {
        Debug.Break();
        GameManager.scoreChange -= SetUIText;
    }
}
