using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleScene : MonoBehaviour {
    #region Main Menu
    [Header("Main Menu")]
    public Button mainMenuPlayBtn;
    public Button settingsMenuBtn;
    public Button instructionsMenuBtn;
    #endregion

    #region Settings Menu
    [Header("Settings Menu")]
    public GameObject settingsMenu;
    public Button settingsMenuPlayBtn;
    public Button settingsMenuCreditBtn;
    public Button settingsMenuBackBtn;
    public Button settingsMenuNoAddsBtn;
    #endregion

    #region Instructions Menu
    [Header("Instructions Menu")]
    public GameObject instructionPanel;
    public Button instructionMenuPlayBtn;
    public Button instructionMenuBackBtn;
    #endregion
    
    #region Credit Menu
    public GameObject creditPanel;
    public Button creditBack;
    #endregion
	// Use this for initialization
	void Start () {
        #region Main Menu Delegates
        mainMenuPlayBtn.onClick.AddListener(delegate { GameManager.instance.LoadScene("GameScene"); });
        settingsMenuBtn.onClick.AddListener(ShowSettings);
        instructionsMenuBtn.onClick.AddListener(ShowInstructions);
        #endregion

        #region Settings Menu Delegates
        settingsMenuPlayBtn.onClick.AddListener(delegate { GameManager.instance.LoadScene("GameScene"); });
        settingsMenuCreditBtn.onClick.AddListener(ShowCredits);
        settingsMenuBackBtn.onClick.AddListener(HideSettings);
        #endregion

        #region Instructions Menu Delegates
        instructionMenuPlayBtn.onClick.AddListener(delegate { GameManager.instance.LoadScene("GameScene"); });
        instructionMenuBackBtn.onClick.AddListener(HideInstructions);
        #endregion

        #region Credit Menu Delegates
        creditBack.onClick.AddListener(HideCredits);

        #endregion
    }
	
	public void ShowCredits()
    {
        creditPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void HideSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionPanel.SetActive(false);
    }

}
