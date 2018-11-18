using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleScene : MonoBehaviour {
    public Button play;
    public Button credits;
    public Button instructions;
    public Button creditBack;
    public Button instructionBack;
    public GameObject creditPanel;
    public GameObject instructionPanel;
	// Use this for initialization
	void Start () {
        play.onClick.AddListener(delegate { GameManager.instance.LoadScene("GameScene"); });
        credits.onClick.AddListener(ShowCredits);
        instructions.onClick.AddListener(ShowInstructions);
        creditBack.onClick.AddListener(HideCredits);
        instructionBack.onClick.AddListener(HideInstructions);
    }
	
	public void ShowCredits()
    {
        creditPanel.SetActive(true);
    }

    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditPanel.SetActive(false);
    }

    public void HideInstructions()
    {
        instructionPanel.SetActive(false);
    }
}
