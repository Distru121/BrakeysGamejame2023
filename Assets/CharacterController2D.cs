using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;	// Amount of force added when the player jumps.
	[SerializeField] private float m_SpeedLimit = 100f; //speed limit the player can reach at max
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsDeadly;							// A mask determining what can kill the player
	[SerializeField] private LayerMask m_WhatIsCheckpoint;						// A mask determining what is checkpoint
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceiling
	[SerializeField] private Grappling grapplingScript;
	[SerializeField] private Animator animator;									//the animator for the character.
	[SerializeField] private SpriteRenderer m_Renderer;							//the actual sprite of the player
	[SerializeField] private TrailRenderer trailrenderer;						//the trail behind the player
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public Rigidbody2D m_Rigidbody2D;
	private Collider2D m_Collider;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public Vector2 checkPointPosition = new Vector2(0, 0);

	private int jumpCounter = 0;
	public bool dashed = false;

	public float dashStrength = 40;
	public bool dead = false;

	public float momentum = 0f; //momentum of the player. If it's too high, it will cause a death upon landing.
	private float airborneTime = 0f; //a timer that calculates the time the player was airborne at max momentum (freefalling), and sums itself to momentum, to cause death.
	public float deadlyMomentum = 16f; //the min momentum the player can die at.
	public float playerGravityScale = 3.15f; //the original gravity scale of the player

	//UI ELEMENTS
	public TMP_Text momentum_text;
	public Image grapplingStatus; //the grappling UI
	public Image grapplingStatusRoot; //the grey thing that is seen behind the grappling UI
	public float remainingRope = 1;
	public Image canDoubleJump;
	public Sprite grapplingRope;
	public Sprite grapplingRopeHooked;
	public Image canDash;

	//events
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnDeathEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Collider = GetComponent<Collider2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		if (OnDeathEvent == null)
			OnDeathEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
				if(momentum >= deadlyMomentum)
					OnDeathEvent.Invoke();
			}
		}
		//if player is touching a deadly thing, fucking die
		if(m_Collider.IsTouchingLayers(m_WhatIsDeadly))
			OnDeathEvent.Invoke();

		// sets the checkpoint if touching one
		if(m_Collider.IsTouchingLayers(m_WhatIsCheckpoint)) {
			checkPointPosition = transform.position;
		}


		//set velocity back to speed limit if it exceeds said speed
		if(m_Rigidbody2D.velocity.magnitude > m_SpeedLimit)
			m_Rigidbody2D.velocity = m_Rigidbody2D.velocity.normalized * m_SpeedLimit;

		//momentum calculation
		if(!wasGrounded && m_Rigidbody2D.velocity.magnitude > 14f) //if freefalling, add extra airborne time
			airborneTime += Time.fixedDeltaTime;
		else
			airborneTime = 0;
		momentum = m_Rigidbody2D.velocity.magnitude + (airborneTime * 2);
		if(momentum < 0)
			momentum = 0;
		if(momentum > m_SpeedLimit*2)
			momentum = m_SpeedLimit*2;
		
		if(momentum >= deadlyMomentum) //trail
			trailrenderer.time = Mathf.Max(0.5f, trailrenderer.time+0.025f);
		else if(momentum >= deadlyMomentum-1.2f)
			trailrenderer.time = 0.2f;
		else
			trailrenderer.time = Mathf.Max(trailrenderer.time-0.025f, 0);

		//HERE IT TAKES CARE OF UPDATING THE UI
		//here it writes the current momentum
		momentum_text.text = (Mathf.Round((momentum / m_SpeedLimit)*100)).ToString() + "%";
		if(momentum >= deadlyMomentum)
			momentum_text.color = Color.red;
		else if(momentum >= deadlyMomentum-1.2f)
			momentum_text.color = new Color(1f, 0.5f, 0.31f);
		else if(momentum >= deadlyMomentum-3.6f)
			momentum_text.color = Color.yellow;
		else
			momentum_text.color = Color.white;

		grapplingStatus.fillAmount = remainingRope; //fill the ui element with the remaining rope with...the remaining rope
		if(remainingRope == 2){
			grapplingStatus.sprite = grapplingRopeHooked;
			grapplingStatusRoot.color = new Color(0, 0, 0, 0);}
		else{
			grapplingStatus.sprite = grapplingRope;
			grapplingStatusRoot.color = new Color(1, 1, 1, 1);}

		if(jumpCounter < 1 && momentum < deadlyMomentum - 1.2 && !grapplingScript.grappled) //doublejump ui
			canDoubleJump.color = new Color(1, 1, 1, 1);
		else
			canDoubleJump.color = new Color(0, 0, 0, 0); //sets transparency to 0 if you cannot double jump

		if(!dashed) //dash ui
			canDash.color = new Color(1, 1, 1, 1);
		else
			canDash.color = new Color(0, 0, 0, 0);
		
	}

	public void Move(float move, bool jump, bool dash)
	{
	// if he ded, he not move
		if(!dead) {
			//MOVE THE CHARACTER
			if(m_Grounded){ //MOVE THE CHARACTER IN A STANDARD WAY ONLY IF IT IS GROUNDED
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			Vector3 appliedVelocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			m_Rigidbody2D.velocity = appliedVelocity;
			}
			else //ELSE, MOVE IT USING PHISICS (IT WILL SWING WITH THE ROPE)
			{
				m_Rigidbody2D.AddForce(new Vector2(move, 0));
			}

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}

			// If the player should jump...
			if (jump)
			{
				// if grounded jumps normally
			if(m_Grounded) {
				// Add a vertical force to the player.
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			}
			// if grappled AND higher than the rope position jumps and breaks the rope
				else if(grapplingScript.canGrapplejump) {
					// adds the force and destroys the rope
					grapplingScript.destroyRope();
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					animator.SetBool("GrappleJumping", true); //sets the grapplejump variable on the animator to true
					jumpCounter++;
				}
				// if has jumped only one time and is not grappled performs a super duper mega ultra cosmic hyper cool double-jump
				else if(jumpCounter < 1 && momentum < deadlyMomentum - 1.2 && !grapplingScript.grappled) {
					// resets the player's momentum and adds the force
					m_Rigidbody2D.velocity = new Vector2(move / 1.5f, 0);
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					animator.SetBool("IsDoubleJumping", true); //sets the doublejumping to true on the animator
					jumpCounter++;
				}
			}

			if(m_Grounded) {
				jumpCounter = 0;
				dashed = false;
				animator.SetBool("GrappleJumping", false);
				animator.SetBool("IsDoubleJumping", false); //sets the doublejumping to true on the animator
			}
			if(grapplingScript.grappled)
			{
				animator.SetBool("GrappleJumping", false); //reset the grapplejumping on the animator if you grapple again, so it can animate another jump.
			}

			// if the player should dash and the momentum isn't deadly the horizontal velocity is boosted and the vertical velocity is set to 0 (so he stops in mid air)
			if(dash) {
				if(grapplingScript.canGrapplejump)
				{
					grapplingScript.destroyRope(); //if you are grappled (so you can grapple jump), the dash destroys the rope
				}
				if(m_FacingRight)
					m_Rigidbody2D.velocity = new Vector2(dashStrength, 0);
				else
					m_Rigidbody2D.velocity = new Vector2(-dashStrength, 0);
				dashed = true;
			}
		}
		// press enter to respawn
		else if(Input.GetKey(KeyCode.Return))  {
			
			respawn();
		}
		else
		{
			//when dead, there is no collider, and no gravity, and no sprite lol
			m_Collider.enabled = false;
			m_Rigidbody2D.gravityScale = 0;
			m_Rigidbody2D.drag = 2f;
			m_Renderer.enabled = false;
			trailrenderer.enabled = false;
		}


	}

	void respawn() { //respawns the player
		m_Rigidbody2D.velocity = Vector2.zero;
		m_Rigidbody2D.position = checkPointPosition;
		dead = false;
		grapplingScript.dead = false;

		//resets the collider, gravity, and sprite
		m_Collider.enabled = true;
		m_Rigidbody2D.gravityScale = playerGravityScale;
		m_Rigidbody2D.drag = 0f;
		m_Renderer.enabled = true;
		trailrenderer.enabled = true;
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
