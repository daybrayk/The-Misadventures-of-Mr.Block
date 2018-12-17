using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonHead : MonoBehaviour {
    public bool touchingPlayer;
    public PlayerController cc;
    public PistonHead otherHead;
	// Use this for initialization
	void Start () {
        if (!otherHead)
            Debug.Log("<color=red>Programmer Error:</color> Reference to other piston head is missing on Game Object: " + name);
        if (!cc)
            cc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingPlayer = true;
        if(otherHead.touchingPlayer)
        {
            cc.Squish();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingPlayer = false;
    }
}
