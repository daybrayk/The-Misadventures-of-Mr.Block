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
    public LayerMask ropeCutterMask;
    public LineRenderer ropeRenderer;

    /*************** Member Variables ***************/
    private bool _ropeAttached;
    private bool _distanceSet;
    private bool _ropeIsCut;
    private int ropeWrapCount;
    private float baseAnchorDistance;
    private Vector2 _playerPosition;
    private Rigidbody2D _ropeHingeAnchorRb;
    private SpriteRenderer _ropeHingeAnchorSprite;
    private List<Vector2> ropePositions = new List<Vector2>();
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    public PolygonCollider2D attachedCollider;
    public float oldColliderPosition;

    private void Awake()
    {
        ropeJoint.enabled = false;
        _playerPosition = transform.position;
        _ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        _ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        wrapPointsLookup.Clear();
        ropePositions.Clear();
    }

    private void Update()
    {
        //update reference to players position
        _playerPosition = transform.position;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !_ropeIsCut)
        {
            Vector2 touchPosition = main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 aimDirection = touchPosition - new Vector2(transform.position.x, transform.position.y);
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);

            if (aimAngle < 0f)
                aimAngle = Mathf.PI * 2 + aimAngle;
            Vector3 shootDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

            ShootHook(shootDirection);
        }
        if (attachedCollider == null)
            ResetRope();
        if (_ropeAttached)
        {
            CheckRopeWrap();
            //Don't need to unwrap if there is only one anchor point
            if(ropePositions.Count > 1)
                CheckRopeUnwrap();
        }
        UpdateRopePositions();

    }
    #region Rope Wrap
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
            //if the raycast hits a grapple point retrieve the new closest point and add an anchor point there
            if (playerToNextAnchor.collider != null)
            {
                //Get a reference to the collider that was hit
                PolygonCollider2D nextColliderHit = playerToNextAnchor.collider as PolygonCollider2D;
                if (nextColliderHit != null)
                {
                    //Get the point closest to the character on the collider that was hit
                    Vector2 closestColliderPoint = GetClosestColliderPoint(playerToNextAnchor, nextColliderHit);
                    if (wrapPointsLookup.ContainsKey(closestColliderPoint))
                    {
                        ResetRope();
                        return;
                    }
                    //Add the new point to the ropePositions list and the wrapPoints list
                    ropePositions.Add(closestColliderPoint);
                    wrapPointsLookup.Add(closestColliderPoint, 0);
                    ropeWrapCount++;
                    ropeJoint.distance = Vector2.Distance(_playerPosition, closestColliderPoint);
                }
                return;
            }

            //if the raycast hits a Rope Cutter obstacle we reset the rope
            playerToNextAnchor = Physics2D.Raycast(_playerPosition, (lastRopePoint - _playerPosition).normalized,
                                                         Vector2.Distance(_playerPosition, lastRopePoint) - 0.1f, ropeCutterMask);
            if (playerToNextAnchor.collider != null)
            {
                _ropeIsCut = true;
                ResetRope();
            }


        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 difference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, difference) * sign;
    }

    private void CheckRopeUnwrap()
    {
        //Anchor point we want to unwrap to
        int anchorIndex = ropePositions.Count - 2;
        //Current anchor point the player is hanging from
        int hingeIndex = ropePositions.Count - 1;
        Vector2 anchorPosition = ropePositions[anchorIndex];
        Vector2 hingePosition = ropePositions[hingeIndex];
        Vector2 hingeDir = hingePosition - anchorPosition;
        float hingeAngle = Vector2.SignedAngle(anchorPosition, hingeDir);
        Vector2 playerDir = _playerPosition - anchorPosition;
        float playerAngle = Vector2.SignedAngle(anchorPosition, playerDir);
        if (hingeAngle < 0f)
            hingeAngle += 360f;
            
        if (playerAngle < 0f)
            playerAngle += 360f;
            

        if(playerAngle < hingeAngle)
        {
            if (wrapPointsLookup[hingePosition] == 1)
            {
                Debug.Log("Player Angle: " + playerAngle);
                Debug.Log("Hinge Angle: " + hingeAngle);
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }

            wrapPointsLookup[hingePosition] = -1;
        }
        else
        {
            if(wrapPointsLookup[hingePosition] == -1)
            {
                Debug.Log("Player Angle: " + playerAngle);
                Debug.Log("Hinge Angle" + hingeAngle);
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }
            Debug.DrawLine(transform.position, hingePosition, Color.red);
            wrapPointsLookup[hingePosition] = 1;
        }
    }

    private void UnwrapRopePosition(int anchorIndex, int hingeIndex)
    {
        Vector2 newAnchorPosition = ropePositions[anchorIndex];
        _ropeHingeAnchorRb.transform.position = newAnchorPosition;
        ropeJoint.distance = Vector2.Distance(transform.position, newAnchorPosition);
        wrapPointsLookup.Remove(ropePositions[hingeIndex]);
        ropePositions.RemoveAt(hingeIndex);

    }
    #endregion

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
            attachedCollider = (PolygonCollider2D)hit.collider;
            oldColliderPosition = attachedCollider.transform.position.y;
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
        _distanceSet = false;
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
        //Update rope positions and wrap positions if the player is being carried upwards by a block
        if (attachedCollider.transform.position.y - oldColliderPosition > 0)
        {
            for (int i = 0; i < ropePositions.Count; i++)
            {
                if (wrapPointsLookup.ContainsKey(ropePositions[i]))
                {
                    int temp = wrapPointsLookup[ropePositions[i]];
                    wrapPointsLookup.Remove(ropePositions[i]);
                    ropePositions[i] = new Vector2(ropePositions[i].x, ropePositions[i].y + attachedCollider.transform.position.y - oldColliderPosition);
                    wrapPointsLookup.Add(ropePositions[i], temp);
                }
                else
                    ropePositions[i] = new Vector2(ropePositions[i].x, ropePositions[i].y + attachedCollider.transform.position.y - oldColliderPosition);
            }
        }else if(ropePositions.Count > 1)
        {
            for (int i = 1; i < ropePositions.Count; i++)
            {
                if (wrapPointsLookup.ContainsKey(ropePositions[i]))
                {
                    int temp = wrapPointsLookup[ropePositions[i]];
                    wrapPointsLookup.Remove(ropePositions[i]);
                    ropePositions[i] = new Vector2(ropePositions[i].x, ropePositions[i].y + Time.deltaTime);
                    wrapPointsLookup.Add(ropePositions[i], temp);
                }
                else
                    ropePositions[i] = new Vector2(ropePositions[i].x, ropePositions[i].y + Time.deltaTime);
            }
        }
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
            oldColliderPosition = attachedCollider.transform.position.y;
        }
    }

    private Vector2 GetClosestColliderPoint(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
        //Store the points of the collider in a dictionary
        Dictionary<float, Vector2> distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
        position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
        position => polyCollider.transform.TransformPoint(position));
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
