using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager _instance = null;
	// Use this for initialization
	void Start () {
		if(_instance == null)
        {
            _instance = this;
        }else
        {
            DestroyImmediate(this);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameManager instance
    {
        get { return _instance; }
    }
}
