using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
    private bool _isPaused;
	// Use this for initialization
	void Awake () {
		if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }else
        {
            DestroyImmediate(gameObject);
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
        DataManager.instance.scoreStart = false;
        DataManager.instance.score = 0;
    }

    /*************** Getters and Setters ***************/
    public static GameManager instance
    {
        get { return _instance; }
    }

    public bool isPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

}
