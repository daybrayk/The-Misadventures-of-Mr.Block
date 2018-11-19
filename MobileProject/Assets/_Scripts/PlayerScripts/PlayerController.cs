using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
    public bool isMoving;
    public GameUI gameUI;
    private Rigidbody2D rb;
    private float swingForce;
	// Use this for initialization
	void Start () {
        if (swingForce <= 0)
            swingForce = 4.0f;
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Dash(int direction)
    {
        switch(direction){
            case 0:
                rb.AddRelativeForce(Vector2.right * swingForce, ForceMode2D.Impulse);
                break;
            case 1:
                rb.AddRelativeForce(Vector2.left * swingForce, ForceMode2D.Impulse);
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "KillZone" || c.tag == "Ground")
        {
            gameUI.RemoveSubscriber();
            DataManager.instance.SaveGameData();
            GameManager.instance.LoadScene("TitleScene");
        }
    }
}
