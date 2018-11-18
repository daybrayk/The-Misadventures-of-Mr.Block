﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
	// Use this for initialization
	void Start () {
		if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }else
        {
            DestroyImmediate(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*************** Getters and Setters ***************/
    public static GameManager instance
    {
        get { return _instance; }
    }

}
