//using System.Text;
using UnityEngine;

/// <summary>
/// <para>Week 2 - Mobile Input</para>
/// Example Touch Script going over possible forms of input for devices.
/// <author>Rocco Briganti</author>
/// </summary>
public class TouchScript : MonoBehaviour
{
	private GUIStyle _guiStyle = new GUIStyle();
	void Awake()
	{
		_guiStyle.fontSize = 35;
		_guiStyle.normal.textColor = Color.white;
	}

	/// <summary>
	/// Normally this is done in an Update method but we are using OnGUI
	/// just to quickly render some information without Canvas setup.
	/// </summary>
	void OnGUI()
	{
#if UNITY_EDITOR
		for (int i = 0; i < 3; i++)
		{
			if (Input.GetMouseButton(i))
			{
				PrintTouchInfo(i, "Pressed", 1, Input.mousePosition);
			}
			else
			{
				PrintTouchInfo(i, "Released", 1, Input.mousePosition);
			}
		}
#endif

#if UNITY_IOS || UNITY_ANDROID
		foreach (Touch touch in Input.touches)
		{
			PrintTouchInfo(touch.fingerId, touch.phase.ToString(), touch.tapCount, touch.position);
		}
#endif
	}

	/// <summary>
	/// Prints the information associated with our touch input to the screen.
	/// <paramref name="pTouchId">The ID associated with the touch.</paramref>
	/// <paramref name="pTouchPhase">The current phase of your touch.</paramref>
	/// <paramref name="pTapCount">The amount of times you tapped within a certain time. I.E. Single/Double/Triple tap.</paramref>
	/// <paramref name="pTouchPos">The current position of your touch input.</paramref>
	/// </summary>
	private void PrintTouchInfo(int pTouchId, string pTouchPhase, int pTapCount, Vector2 pTouchPos)
	{
		// Can import System.Text at the top to avoid typing it every time.
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append(string.Format("ID: {0}{1}", pTouchId, System.Environment.NewLine));
		sb.Append(string.Format("Phase: {0}{1}", pTouchPhase, System.Environment.NewLine));
		sb.Append(string.Format("TapCount: {0}{1}", pTapCount, System.Environment.NewLine));
		sb.Append(string.Format("Pos X: {0}{1}", pTouchPos.x, System.Environment.NewLine));
		sb.Append(string.Format("Pos Y: {0}{1}", pTouchPos.y, System.Environment.NewLine));

		GUI.Label(new Rect(280 * pTouchId, 0, 120, 100), sb.ToString(), _guiStyle);
	}
}
