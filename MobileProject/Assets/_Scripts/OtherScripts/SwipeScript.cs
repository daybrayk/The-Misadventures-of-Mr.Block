using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Week 2 - Mobile Input</para>
/// Example of Swiping to add Torque to a cube. Set the Angular drag on the Rigidbody to 1.
/// <author>Rocco Briganti</author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SwipeScript : MonoBehaviour
{
	private const float MIN_SWIPE_LENGTH = 50f;
	private const float MAX_SWIPE_TIME = 0.35f;

	private Rigidbody _rb;
	private Vector3 _mouseStartPos;
	private float _elapsedTime;
	private bool _startTimer;

	public bool _enableFreeSwipe = true;

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_mouseStartPos = new Vector3(0f, 0f, 0f);
		_elapsedTime = 0f;
	}

	void Update()
	{
#if UNITY_EDITOR
		SimulateMouseSwipe();
#endif

#if UNITY_ANDROID || UNITY_IOS
		if (Input.touchCount == 1)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				_startTimer = true;
				_elapsedTime = 0f;
			}

			if (touch.phase == TouchPhase.Ended && _elapsedTime < MAX_SWIPE_TIME)
			{
				//SpinCube(touch.deltaPosition);
			}
		}
#endif

		if (_startTimer)
		{
			if (_elapsedTime < MAX_SWIPE_TIME)
			{
				_elapsedTime += Time.deltaTime;
			}

			if (_elapsedTime >= MAX_SWIPE_TIME)
			{
				_startTimer = false;
			}
		}
	}

	/// <summary>
	/// This will simulate both directional and free swipe using the Left mouse button.
	/// </summary>
	private void SimulateMouseSwipe()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_mouseStartPos = Input.mousePosition;
			_startTimer = true;
			_elapsedTime = 0f;
		}

		if (Input.GetMouseButtonUp(0) && _elapsedTime < MAX_SWIPE_TIME)
		{
			_startTimer = false;

			Vector3 mouseEndPos = Input.mousePosition;
			Vector3 direction = (mouseEndPos - _mouseStartPos);

			//SpinCube(direction);
		}
	}

    void Dash()
    {

    }
}
