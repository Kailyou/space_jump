using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	// References
	private Rigidbody2D rb2d;
	private Animator animator;
	private AudioSource audioSource;

	// Config
	public float maxSpeed  = 10;
	public float jumpForce = 550;
	public GameObject laserPrefab;
	public Transform laserSpawnPoint;
	public float laserSpeed = 500;

	// Status
	[HideInInspector]
	public bool lookingRight = true;
	private bool grounded  = false;
	private bool do_lock = false;

	// Jump
	private bool prepareJump = false;
	private bool jumping     = false;
	private bool jumpLock    = false;


	/* Attack */
	// Melee Attack
	private float attackCooldownTime_melee        	= 0.25f;
	private float currentAttackCooldownTime_melee	= 0f;
	private bool attackOnCD_melee 				  	= false;
	private bool attacking_melee 					= false;

	// Range Attack
	private float attackCooldownTime_range        	= 1f;
	private float currentAttackCooldownTime_range	= 0f;
	private bool attackOnCD_range 					= false;
	private bool attacking_range 					= false;

	// Collision detection
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Idling
	private float timer = 5f;
	private bool idling = false;



	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update ()
	{		
		handleUserInput();

		updateAttack ();

		updateJump ();
					
		// Change direction of the player by inverting the scale position
		float hor = Input.GetAxis ("Horizontal");

		if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight))
		{
			Flip ();
		}

		// Idle 
		if (Mathf.Abs(rb2d.velocity.x) <= 0.1f) 
		{
			idling = true;
		} else 
		{
			idling = false;
			timer = 5;
		}

		updateTimer ();
	}

	private void handleUserInput()
	{
		/* CONTROLLING */
		if (!do_lock) 
		{
			// Jump
			if (Input.GetButtonDown ("Jump") && grounded && !prepareJump && !jumping) 
			{
				prepareJump = true;
				idling = false;
				timer = 5f;
			}

			// Melee Attack
			if (Input.GetButtonDown ("Fire1") && !attacking_melee && !attackOnCD_melee && !attacking_range)
			{
				attacking_melee = true;
				idling = false;
				timer = 5f;
			}

			// Range Attack
			if (Input.GetButtonDown ("Fire2") && !attacking_range && !attackOnCD_range && !attacking_melee)
			{
				attacking_range = true;
				idling = false;
				timer = 5f;
			}
		}
	}


	private void updateAttack()
	{
		// Melee
		if (attacking_melee) 
		{
			attackOnCD_melee = true;

			Debug.Log ("test");

			animator.SetTrigger ("attacking_melee");

			attacking_melee = false;
		}

		// Range
		if (attacking_range)
		{
			attackOnCD_range = true;

			animator.SetTrigger ("attacking_range");

			audioSource.Play ();

			// Create laser object and adds a force to the looking side of the player
			GameObject laser = (GameObject)Instantiate (laserPrefab, laserSpawnPoint.position, Quaternion.identity);
			laser.tag = "Laser";

			if (lookingRight) 
			{
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.right * laserSpeed);
			} else 
			{
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.left * laserSpeed);
			}

			attacking_range = false;
		}
	}


	// Handles the jump of the trooper
	// When the tropper is jumping and reaches the ground
	// the jump will stop.
	private void updateJump()
	{
		// Check if jump is done
		if (prepareJump) 
		{
			if (!grounded)
			{
				animator.SetBool ("jumping", true);
				jumping     = true;
				prepareJump = false;
				jumpLock    = true;
			}
		} 

		else if (jumping) 
		{
			if (grounded) 
			{
				animator.SetBool ("jumping", false);
				jumping = false;
				jumpLock = false;
			}
		}
	}


	private void updateTimer()
	{
		// Melee Attack Cooldown
		if (attackOnCD_melee)
		{
			currentAttackCooldownTime_melee += Time.deltaTime;

			if (currentAttackCooldownTime_melee > attackCooldownTime_melee)
			{
				attackOnCD_melee = false;
				currentAttackCooldownTime_melee = 0f;
			}
		}

		// Range Attack Cooldown
		if (attackOnCD_range)
		{
			currentAttackCooldownTime_range += Time.deltaTime;

			if (currentAttackCooldownTime_range > attackCooldownTime_range)
			{
				attackOnCD_range = false;
				currentAttackCooldownTime_range = 0f;
			}
		}

		// Idle timer
		if (idling) 
		{
			timer -= Time.deltaTime;
		}

		animator.SetBool ("idling", timer <= 0f);
	}




	void FixedUpdate ()
	{
		if (!do_lock)
		{
			// Walking left or right
			float hor = Input.GetAxis ("Horizontal");

			rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);
		}

		// Checks if the player is grounded by checking if the ground check position of the player
		// colides with any other circle
		// the both hit box circles of the player are excluded.
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.5F, whatIsGround);
		animator.SetBool ("grounded", grounded);

		// Jump
		if (prepareJump && !jumpLock) 
		{
			rb2d.AddForce (new Vector2 (0, jumpForce));
			jumping  = false;
			jumpLock = true; 
		}

		animator.SetFloat ("speed", Mathf.Abs (rb2d.velocity.x));
	}




	public void Flip ()
	{
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	public void Lock ()
	{
		do_lock = true;
		Destroy (gameObject);
	}
}
