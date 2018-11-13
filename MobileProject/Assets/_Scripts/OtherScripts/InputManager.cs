using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public RopeSystem rs;
    Vector2 begin = Vector2.zero;
    Vector2 end = Vector2.zero;
    float beginTime = 0;
    float endTime = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                begin = touch.position;
                beginTime = Time.time;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                end = touch.position;
                endTime = Time.time;
                Debug.Log("Touch Time: " + (end - begin).magnitude);
                if ((end - begin).magnitude < 1f)
                {
                    rs.ShootHook(touch);
                }
                else
                {
                }
            }
        }
	}
}
