using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public RopeSystem rs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Ended)
            {
                if(touch.tapCount == 1 && !rs.ropeIsCut)
                {

                }else
                {

                }
            }
        }
	}
}
