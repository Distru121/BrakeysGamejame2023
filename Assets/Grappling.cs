using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    public Camera mainCamera;
    public LineRenderer _lineRenderer;
    public DistanceJoint2D _distanceJoint;
    public Rigidbody2D rb;
    public float force;
    private Vector3 mouseDir;
    public Transform linePosition;
    public Rigidbody2D linePositon_rb;
    public SpriteRenderer grapplingHook_renderer; //the renderer of the line position (end of line)
    public bool isGrappling;
    public CharacterController2D charController;

    public float grapplingHookSpeed = 10f;
    public float grapplingLength = 100f;

    [SerializeField] private LayerMask m_WhatIsGround; //the grappling can grapple to ground

    private Vector2 grapplingLaunchDirection = new Vector2(0, 0);
    public bool grappled = false; //this is on when the grappling hook hooks something
    public bool canGrapplejump = false; //character can only grapple jump while grappling and with a rope y position less than the rope itself
    private bool brokenRope = false;


    void Start() {
        isGrappling = true;
        _distanceJoint.autoConfigureDistance = true;
        _distanceJoint.enabled = false;
        _lineRenderer.enabled = false;
        grapplingHook_renderer.enabled = false;
    }

    void Update() {

        if(isGrappling == true)
        {

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 mousepos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
                linePosition.position = transform.position;

                grapplingLaunchDirection = (mousepos - (Vector2)transform.position).normalized; //sets the launch direction for the grappling hook

                _lineRenderer.SetPosition(0, (Vector2)linePosition.position);

                grapplingHook_renderer.enabled = true;
            }
            if(Input.GetKey(KeyCode.Mouse0))
            {
                //updates the line position (only if the rope is not broken)
                if(!brokenRope){

                _lineRenderer.SetPosition(1, transform.position);
                _lineRenderer.SetPosition(0, linePosition.position);

                _lineRenderer.enabled = true;

                if(grappled)
                {
                    linePositon_rb.velocity = new Vector2(0, 0); //reset the velocity of the end of the line once it reached a grappling spot

                    //if the rope is hooked UNDER the player, disable the distancejoint (stop calculating grappling phisics, otherwise it doesn't make any sense)
                    //if the player is grounded, disable the distancejoint to not hinder movements
                    if(transform.position.y > linePosition.position.y || charController.m_Grounded == true)
                        _distanceJoint.enabled = false;
                    else
                        _distanceJoint.enabled = true;
                    if(_distanceJoint.enabled) //and if there is no distancejoint, you cannot grapplejump!
                        canGrapplejump = true;

                    charController.remainingRope = 2; //if I put 2, it means the character is grappled

                    //if the rope exceeds the max length AFTER it has been hooked, it can extend up to 75% more length before breaking.
                    if((transform.position - linePosition.position).magnitude > grapplingLength * 1.75f)
                    {
                        brokenRope = true;
                        _lineRenderer.enabled = false;
                        _distanceJoint.enabled = false;
                    }
                }
                else
                {
                    //if not grappled, make the rope go very fast in the direction aimed by the mouse at the start, until it finds ground to grapple on
                    linePositon_rb.velocity = grapplingLaunchDirection * grapplingHookSpeed;
                    _distanceJoint.enabled = false;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(linePosition.position, .1f, m_WhatIsGround);
		            for (int i = 0; i < colliders.Length; i++)
		            {
			            if (colliders[i].gameObject != gameObject)
			            { //when grappling, it connects the joint to the hook end
				            grappled = true;
                            _distanceJoint.connectedAnchor = linePosition.position;
			            }
		            }

                    charController.remainingRope = (grapplingLength-(transform.position - linePosition.position).magnitude)/grapplingLength;

                    //if the rope exceeds the max length while it's mid air, it gets fucking annihlated
                    if((transform.position - linePosition.position).magnitude > grapplingLength)
                    {
                        brokenRope = true;
                        _lineRenderer.enabled = false;
                        grapplingHook_renderer.enabled = false;
                    }
                }

                Vector2 dir = linePosition.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                linePosition.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                }
                else
                {
                    charController.remainingRope = 0; //if the rope is broken, send information about it
                    canGrapplejump = false;
                }
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                _distanceJoint.enabled = false;
                _lineRenderer.enabled = false;
                grapplingHook_renderer.enabled = false;
                grappled = false;
                brokenRope = false;
                charController.remainingRope = 1;
                canGrapplejump = false;

            }

            if(Input.GetKey(KeyCode.Mouse1) && grappled)
            {
                Vector3 direction = linePosition.position - transform.position;

                rb.AddForce(new Vector2(direction.x * force, direction.y * force).normalized * force * Time.deltaTime);
                _distanceJoint.enabled = false;
            }
            if(Input.GetKeyUp(KeyCode.Mouse2) && grappled)
            {
                _distanceJoint.enabled = true;
            }
        }
    }

    public void destroyRope() {
        _distanceJoint.enabled = false;
        _lineRenderer.enabled = false;
        grapplingHook_renderer.enabled = false;
        grappled = false;
        brokenRope = true;
    }

}