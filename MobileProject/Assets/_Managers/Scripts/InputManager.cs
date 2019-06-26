using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
    public RopeSystem rs;
    public PlayerController pc;
    Animator anim;
    Vector2 begin = Vector2.zero;
    Vector2 end = Vector2.zero;
    float beginTime = 0;
    float endTime = 0;
    float minSwipeDistance;
    private void Start()
    {
        anim = GetComponent<Animator>();
        minSwipeDistance = Screen.width * 10 / 100; //Minimum swipe distance is 10% of the screen's width
    }
    // Update is called once per frame
    void Update () {
        if (!GameManager.instance.isPaused)
        {
            if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    begin = touch.position;
                    beginTime = Time.time;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    end = touch.position;
                    endTime = Time.time;
                    Vector2 dif = end - begin;
                    if (dif.magnitude < minSwipeDistance)
                    {
                        rs.ShootHook(touch);
                        pc.canDash = true;
                    }
                    else if (pc.canDash && rs.ropeAttached)
                    {
                        if (dif.x > 0)
                        {
                            pc.Dash(0);
                        }
                        else
                            pc.Dash(1);
                        pc.canDash = false;
                    } 
                    else if(!rs.ropeAttached)
                    {
                        if (dif.x > 0)
                            pc.Dash(2);
                        else
                            pc.Dash(3);
                    }
                }
            }
        }
        else
            Input.ResetInputAxes();
	}
}
