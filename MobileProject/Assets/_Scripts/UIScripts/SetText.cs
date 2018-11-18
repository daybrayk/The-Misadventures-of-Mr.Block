using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetText : MonoBehaviour {
    public DataManager dm;
    public Text scoreText;
    public Text highScoreText;
    public Text currencyText;

    private void Start()
    {
        SetUIText();
    }

    public void SetUIText()
    {
        scoreText.text = "Score: " + dm.score;
        highScoreText.text = "HighScore: " + dm.highScore;
        currencyText.text = "Currency: " + dm.currency;
    }
}
