using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public struct JointInfo
{
    public Vector2 position;
    public float joinDistance;
    public JointInfo(Vector2 p, float jD)
    {
        position = p;
        joinDistance = jD;
    }
};

public class RopeSystem : MonoBehaviour {
    /*************** Constant Variables ***************/
    private const float ROPEMAXDISTANCE = 20.0f;

    /*************** Public Variables ***************/
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    //public SpringJoint2D ropeJoint;
    /*public Transform crosshair;
    public SpriteRenderer crosshairSprite;*/
    public PlayerController playerController;
    public Camera main;
    public LayerMask grappleMask;
    public LineRenderer ropeRenderer;

    /*************** Member Variables ***************/
    private bool _ropeAttached;
    private bool _distanceSet;
    private int ropeWrapCount;
    private float baseAnchorDistance;
    private Vector2 _playerPosition;
    private Rigidbody2D _ropeHingeAnchorRb;
    private SpriteRenderer _ropeHingeAnchorSprite;
    private List<Vector2> ropePositions = new List<Vector2>();
    private Dictionary<int, JointInfo> wrapPointsLookup = new Dictionary<int, JointInfo>();

    private void Awake()
    {
        ropeJoint.enabled = false;
        _playerPosition = transform.position;
        _ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        _ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //update reference to players position
        _playerPosition = transform.position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 aimDirection = touchPosition - new Vector2(transform.position.x, transform.position.y);
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);

            if (aimAngle < 0f)
                aimAngle = Mathf.PI * 2 + aimAngle;
            Vector3 shootDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

            ShootHook(shootDirection);
        }

        if (_ropeAttached)
        {
            CheckRopeWrap();
            CheckRopeUnwrap();
            //ropeJoint.distance = currentAnchorDistance;
        }
        UpdateRopePositions();

    }
    /// <summary>
    /// Checks if there is a collider between the player and the last anchor point, 
    /// if so create a new anchor point for the rope wrap around the collider
    /// </summary>
    private void CheckRopeWrap()
    {
        if (ropePositions.Any())
        {
            //Get the last anchor point the rope is connected to
            Vector2 lastRopePoint = ropePositions.Last();
            /*
             * Cast a ray from the player in the direction of the last anchor point for the distance between the player and
             * the anchor point
            */
            RaycastHit2D playerToNextAnchor = Physics2D.Raycast(_playerPosition, (lastRopePoint - _playerPosition).normalized,
                                                         Vector2.Distance(_playerPosition, lastRopePoint) - 0.1f, grappleMask);
            //if the raycast hit something retrieve the new closest point and add an anchor point there
            if (playerToNextAnchor.collider != null)
            {
                //Get a reference to the collider that was hit
                PolygonCollider2D nextColliderHit = playerToNextAnchor.collider as PolygonCollider2D;
                if (nextColliderHit != null)
                {
                    //Get the point closest to the character on the collider that was hit
                    Vector2 closestColliderPoint = GetClosestColliderPoint(playerToNextAnchor, nextColliderHit);
                    /*if (wrapPointsLookup.ContainsValue(closestColliderPoint))
                    {
                        ResetRope();
                        return;
                    }*/
                    //Add the new point to the ropePositions list and the wrapPoints list
                    ropePositions.Add(closestColliderPoint);
                    JointInfo wrapPoint = new JointInfo(closestColliderPoint, Vector2.Distance(_playerPosition, closestColliderPoint));
                    wrapPointsLookup.Add(ropeWrapCount, wrapPoint);
                    ropeWrapCount++;
                    //Reset distanceSet so it can be updated in the UpdateRopePositions function
                    ropeJoint.distance = wrapPoint.joinDistance;
                }
            }
        }
    }

    private void CheckRopeUnwrap()
    {
        if(wrapPointsLookup.Count == 1)
        {
            Vector2 secondLastPoint = ropePositions[0];
            if(!Physics2D.Raycast(_playerPosition, (secondLastPoint - _playerPosition).normalized,
                                 Vector2.Distance(secondLastPoint, _playerPosition) - 0.1f, grappleMask))
            {
                ropePositions.RemoveAt(ropePositions.Count() - 1);
                wrapPointsLookup.Remove(ropeWrapCount-1);
                ropeWrapCount--;
                ropeJoint.distance = baseAnchorDistance;
            }
        }
        else if(wrapPointsLookup.Count > 1)
        {
            Vector2 secondLastPoint = wrapPointsLookup[ropeWrapCount-2].position;
            if (!Physics2D.Raycast(_playerPosition, (secondLastPoint - _playerPosition).normalized,
                                 Vector2.Distance(secondLastPoint, _playerPosition) - 0.1f, grappleMask))
            {
                ropePositions.RemoveAt(ropePositions.Count() - 1);
                wrapPointsLookup.Remove(ropeWrapCount-1);
                ropeWrapCount--;
                ropeJoint.distance = wrapPointsLookup[ropeWrapCount - 1].joinDistance;
            }
        }
    }

    private void ShootHook(Vector2 aimDirection)
    {
        RaycastHit2D hit;
        //if the rope is already attached then detach the rope and exit the function
        if (_ropeAttached)
        {
            ResetRope();
            return;
        }

        //if the raycast hits something enable the rope and add the anchor point to the ropePositions list
        if (hit = Physics2D.Raycast(_playerPosition, aimDirection, ROPEMAXDISTANCE, grappleMask))
        {
            ropeRenderer.enabled = true;
            _ropeAttached = true;
            if (!ropePositions.Contains(hit.point))
            {
                ropePositions.Add(hit.point);
                baseAnchorDistance = ropeJoint.distance = Vector2.Distance(_playerPosition, hit.point);
                _distanceSet = true;
                ropeJoint.enabled = true;
                _ropeHingeAnchorSprite.enabled = true;
            }
        }
        //if the raycast doesn't hit anything disable the rope
        else
        {
            ropeRenderer.enabled = false;
            _ropeAttached = false;
            ropeJoint.enabled = false;
        }

    }
    /// <summary>
    /// Reset all rope settings
    /// </summary>
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeRenderer.enabled = false;
        playerController.isMoving = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePositions.Clear();
        wrapPointsLookup.Clear();
        _ropeHingeAnchorSprite.enabled = false;
        _ropeAttached = false;
    }

    private void UpdateRopePositions()
    {
        //if the rope isn't attached then don't do anything
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
                    _ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!_distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        _distanceSet = true;
                    }

                }
                else if(i - 1 == ropePositions.IndexOf(ropePositions.Last()))   //if it is the last point of the line
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

    private Vector2 GetClosestColliderPoint(RaycastHit2D hit, PolygonCollider2D collider)
    {
        //Store the points of the collider in a dictionary
        Dictionary<float, Vector2> distanceDictionary = collider.points.ToDictionary<Vector2, float, Vector2>(
        position => Vector2.Distance(hit.point, collider.transform.TransformPoint(position)),
        position => collider.transform.TransformPoint(position));
        //Store the keys of the dictionary in a list and then sort the list
        List<float> sortedList = distanceDictionary.Keys.ToList();
        sortedList.Sort();
        //return the position vector of the point that is closest to the player
        return distanceDictionary.Any() ? distanceDictionary[sortedList.First()] : Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.tag == "Ground")
            SceneManager.LoadScene("TitleScene");
    }

}
