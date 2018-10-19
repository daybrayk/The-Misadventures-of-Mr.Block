using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
    private bool _distanceSet;
    private Vector2 _playerPosition;
    private Rigidbody2D _ropeHingeAnchorRb;
    private SpriteRenderer _ropeHingeAnchorSprite;
    private List<Vector2> ropePositions = new List<Vector2>();

    private void Awake()
    {
        ropeJoint.enabled = false;
        _playerPosition = transform.position;
        _ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        _ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Input.GetTouch(0).phase == TouchPhase.Began
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 aimDirection = touchPosition - new Vector2(transform.position.x, transform.position.y);
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);

            if (aimAngle < 0f)
                aimAngle = Mathf.PI * 2 + aimAngle;
            Vector3 shootDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
            _playerPosition = transform.position;

            HandleInput(aimDirection);
        }
        UpdateRopePositions();
    }

    private void HandleInput(Vector2 aimDirection)
    {
        RaycastHit2D hit;

        if (_ropeAttached)
        {
            ResetRope();
            return;
        }
            
        ropeRenderer.enabled = true;

        if (hit = Physics2D.Raycast(_playerPosition, aimDirection, ROPEMAXDISTANCE, grappleMask))
        {
            _ropeAttached = true;
            if (!ropePositions.Contains(hit.point))
            {
                ropePositions.Add(hit.point);
                ropeJoint.distance = Vector2.Distance(_playerPosition, hit.point);
                ropeJoint.enabled = true;
                _ropeHingeAnchorSprite.enabled = true;
            }
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
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        _ropeHingeAnchorSprite.enabled = false;
        _ropeAttached = false;
    }

    private void UpdateRopePositions()
    {
        if(!_ropeAttached)
            return;

        ropeRenderer.positionCount = ropePositions.Count + 1;

        for (int i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if(i != ropeRenderer.positionCount - 1) //if it is not the last point of the line renderer
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                if(i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    Vector2 ropePosition = ropePositions[ropePositions.Count - 1];
                    /*if (ropePositions.Count == 1)
                    {
                        _ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!_distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            _distanceSet = true;
                        }
                    }
                    else
                    {
                        _ropeHingeAnchorRb.transform.position = ropePosition;
                        if(!_distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            _distanceSet = true;
                        }
                    }*/
                    _ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!_distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        _distanceSet = true;
                    }

                }
                else if(i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    Vector2 ropePosition = ropePositions.Last();
                    _ropeHingeAnchorRb.transform.position = ropePosition;
                    if(!_distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        _distanceSet = true;
                    }
                }
            }
            else
            {
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.collider.tag == "Ground")
            SceneManager.LoadScene("TitleScene");
    }

}
