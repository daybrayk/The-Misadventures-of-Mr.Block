using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class InputManager : MonoBehaviour {
    public RopeSystem rs;
    public PlayerController pc;
    Animator anim;
    Vector2 begin = Vector2.zero;
    Vector2 end = Vector2.zero;
    float beginTime = 0;
    float endTime = 0;
    float minSwipeDistance;
    Touch[] m_touches;
    private void Start()
    {
        Input.multiTouchEnabled = false;
        anim = GetComponent<Animator>();
        minSwipeDistance = Screen.width * 10 / 100; //Minimum swipe distance is 10% of the screen's width
    }
    // Update is called once per frame
    void Update () {
        if (!GameManager.instance)
            return;
        if (Input.touchCount == 0 || GameManager.instance.isPaused || EventSystem.current.IsPointerOverGameObject(m_touches[0].fingerId))   //Prevents touches from being registered when interacting with UI buttons and other event objects
        {
            ClearTouches();
            return;
        }
        else
            m_touches = Input.touches;

        Touch touch = m_touches[0];
        if (touch.phase == TouchPhase.Began)
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
            else if(pc.canDash && (endTime - beginTime) < 0.25f)
            {
                if (rs.ropeAttached)
                {
                    if (dif.x > 0)
                    {
                        pc.Dash(0);
                    }
                    else
                        pc.Dash(1);
                    pc.canDash = false;
                }
                else if (!rs.ropeAttached)
                {
                    if (dif.x > 0)
                        pc.Dash(2);
                    else
                        pc.Dash(3);
                }
            }
        }
        
    }

    public void ClearTouches()
    {
        Array.Clear(m_touches, 0, m_touches.Length);
    }
}
