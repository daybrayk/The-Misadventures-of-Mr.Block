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
    RopeSystem rs;
	// Use this for initialization
	void Start () {
        if (swingForce <= 0)
            swingForce = 4.0f;
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
        rs = GetComponent<RopeSystem>();
	}

    public void Dash(int direction)
    {
        Vector2 dir = (rs.ropeHingeAnchor.transform.position - transform.position).normalized;
        Vector2 perp = new Vector2(dir.y, -dir.x) * swingForce;
        //rb.velocity = new Vector2(rb.velocity.x, 0);
        switch(direction){
            case 0:
                //rb.AddRelativeForce(Vector2.right * swingForce, ForceMode2D.Impulse);
                rb.AddRelativeForce(perp, ForceMode2D.Impulse);
                break;
            case 1:
                //rb.AddRelativeForce(Vector2.left * swingForce, ForceMode2D.Impulse);
                rb.AddRelativeForce(-perp, ForceMode2D.Impulse);
                break;
        }
    }

    public void Squish()
    {
        gameUI.RemoveSubscriber();
        GameManager.instance.SaveGame();
        GameManager.instance.LoadScene("TitleScene");
    }

    public void Slice()
    {
        gameUI.RemoveSubscriber();
        GameManager.instance.SaveGame();
        GameManager.instance.LoadScene("TitleScene");
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
