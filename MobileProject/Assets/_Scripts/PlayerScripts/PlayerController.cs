using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {
    public bool isMoving;
    public bool dash;
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

    public void Dash(int direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
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
            GameManager.instance.SaveGame();
            GameManager.instance.LoadScene("TitleScene");
        }
    }
}
