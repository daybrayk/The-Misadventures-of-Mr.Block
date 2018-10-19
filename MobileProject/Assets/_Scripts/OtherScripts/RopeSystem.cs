using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour {
    /*************** Constant Variables ***************/
    private const float ROPEMAXDISTANCE = 20.0f;

    /*************** Public Variables ***************/
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    /*public Transform crosshair;
    public SpriteRenderer crosshairSprite;*/
    public PlayerController playerController;
    public Camera main;
    public LayerMask grappleMask;
    public LineRenderer ropeRenderer;

    /*************** Member Variables ***************/
    private bool _ropeAttached;
    private Vector2 _playerPosition;
    private Rigidbody2D _ropeHingeAnchorRb;
    private SpriteRenderer _ropeHingeAnchorSprite;
    //private List<Vector2> ropePositions = new List<Vector2>();

    private void Awake()
    {
        ropeJoint.enabled = false;
        _playerPosition = transform.position;
        _ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        _ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector2 touchPosition = main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 aimDirection = touchPosition - new Vector2(transform.position.x, transform.position.y);
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);

            if (aimAngle < 0f)
                aimAngle = Mathf.PI * 2 + aimAngle;
            Vector3 shootDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
            _playerPosition = transform.position;

            if (!_ropeAttached)
            {
                HandleInput(aimDirection);
            }
            else
            {
                ResetRope();
            }

        }
        else if (!_ropeAttached)
            ResetRope();
    }

    private void HandleInput(Vector2 aimDirection)
    {
        RaycastHit2D hit;

        if (_ropeAttached)
            return;

        ropeRenderer.enabled = true;

        if(hit = Physics2D.Raycast(_playerPosition, aimDirection, ROPEMAXDISTANCE, grappleMask))
        {
            _ropeAttached = true;
            ropeJoint.distance = Vector2.Distance(_playerPosition, hit.point);
            ropeJoint.enabled = true;
            _ropeHingeAnchorSprite.enabled = true;
        }
        else
        {
            ropeRenderer.enabled = false;
            _ropeAttached = false;
            ropeJoint.enabled = false;
        }
    }

    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeRenderer.enabled = false;
        playerController.isMoving = false;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        _ropeHingeAnchorSprite.enabled = false;
        _ropeAttached = false;
    }
}
