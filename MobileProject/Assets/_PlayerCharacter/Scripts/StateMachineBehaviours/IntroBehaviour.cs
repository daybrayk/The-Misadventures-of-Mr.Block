using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehaviour : StateMachineBehaviour {
    public float distance;
    public float time;
    public float force;
    private Transform m_player;
    private Rigidbody2D m_rb;
    private InputManager m_playerInput;
    private bool m_runOnce;

	  //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(!m_runOnce)
        {
            m_playerInput = animator.GetComponent<InputManager>();
            m_player = animator.gameObject.transform;
            m_rb = animator.gameObject.GetComponent<Rigidbody2D>();
            m_rb.bodyType = RigidbodyType2D.Kinematic;
            m_runOnce = true;
        }
        m_playerInput.enabled = false;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        m_rb.MovePosition(new Vector2(m_player.position.x, m_player.position.y) + (Vector2.right * (distance / time) * Time.deltaTime));
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        m_rb.bodyType = RigidbodyType2D.Dynamic;
        m_rb.AddForce(new Vector2(1.25f, 0.3f) * force, ForceMode2D.Impulse);
        m_playerInput.enabled = true;
        animator.Play("Player_Smile");
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
