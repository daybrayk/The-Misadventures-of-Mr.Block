using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager _instance = null;
    public bool gameStart;
    public float time;
	// Use this for initialization
	void Start () {
		if(_instance == null)
        {
            _instance = this;
        }else
        {
            DestroyImmediate(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
	}

    public GameManager instance
    {
        get { return _instance; }
    }
}
