using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	// References
	private Rigidbody2D rb2d;
	private Animator animator;

	// Config
	public float maxSpeed  = 10;
	public float jumpForce = 550;
	public GameObject laserPrefab;
	public Transform laserSpawnPoint;
	public float laserSpeed = 500;

	// Status
	[HideInInspector]
	public bool lookingRight = true;
	[HideInInspector]
	public bool idling = false;
	[HideInInspector]
	private bool crouching = false;
	private bool grounded  = false;
	private bool jumping   = false;
	private float timer = 3f;
	private bool do_lock = false;

	// Attack
	private float attackCooldownTime        = 1.5f;
	private float currentAttackCooldownTime = 0f;
	private bool attackOnCD = false;
	private bool attacking = false;


	// Collision detection
	public LayerMask whatIsGround;
	public Transform groundCheck;

	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();

		animator.SetBool ("crouching", false);
	}

	void Update ()
	{		
		// Update attack cooldown time
		if (attackOnCD)
		{
			currentAttackCooldownTime += Time.deltaTime;

			if (currentAttackCooldownTime > attackCooldownTime)
			{
				attackOnCD = false;
				currentAttackCooldownTime = 0f;
			}
		}

		if (!do_lock) 
		{
			// Jump
			if (Input.GetButtonDown ("Jump") && grounded) 
			{
				jumping = true;
			}

			// Laser attack
			if (Input.GetButtonDown ("Fire1") && !attacking && !attackOnCD)
			{
				attacking = true;
			}
		}

		// Idle 
		if (Mathf.Abs(rb2d.velocity.x) <= 0.1f) 
		{
			idling = true;
		} else 
		{
			idling = false;
			timer = 3f;
		}

		if (idling) 
		{
			timer -= Time.deltaTime;
		}
			
		animator.SetBool ("idling", timer <= 0f);
	}

	void FixedUpdate ()
	{
		if (!do_lock) {
			// Walking left or right
			float hor = Input.GetAxis ("Horizontal");
			rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);

			// Change direction of the player by inverting the scale position
			if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight))
				Flip ();
		}

		// Checks if the player is grounded by checking if the ground check position of the player
		// colides with any other circle
		// the both hit box circles of the player are excluded.
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);

		// Jump
		if (jumping) 
		{
			rb2d.AddForce (new Vector2 (0, jumpForce));
			jumping = false;
		}

		// Attack
		if (attacking)
		{
			attackOnCD = true;
			animator.SetTrigger ("attacking");
			GameObject laser = (GameObject)Instantiate (laserPrefab, laserSpawnPoint.position, Quaternion.identity);
			laser.tag = "Laser";

			if (lookingRight) 
			{
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.right * laserSpeed);
			} else 
			{
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.left * laserSpeed);
			}

			attacking = false;
		}

		animator.SetBool ("grounded", grounded);
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
