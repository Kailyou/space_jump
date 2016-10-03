using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	// References
	private Rigidbody2D rb2d;
	private Animator animator;
	private AudioSource audioSource;
	public Text collectableText;

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
	public bool do_lock = false;
	public int collectAmount;
	public int maxCollectAmount ;

	/* Attack */
	public bool alreadyDamageApplied = false;

	// Melee Attack
	public int meleeDamage 					= 1;
	private float attackCooldownTime_melee	= 0.25f;
	private float next_attack_melee 		= 0f;

	// Range Attack
	private float attackCooldownTime_range	= 1f;
	private float next_attack_range 		= 0f;

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

		// Level 1
		collectAmount    = 0;
		maxCollectAmount = 6;
		if(collectableText)
			collectableText.text = collectAmount.ToString() + " / " + maxCollectAmount.ToString();
	}

	void Update ()
	{		
		handleUserInput();

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
		}
		else {
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
			if (Input.GetButtonDown ("Jump") && grounded) 
			{
				rb2d.AddForce (new Vector2 (0, jumpForce));
				animator.SetTrigger ("jump");

				idling = false;
				timer = 5f;
			}

			// Melee Attack
			if (Input.GetButtonDown ("Fire1") && Time.time > next_attack_melee)
			{
				animator.SetTrigger ("attacking_melee");
				next_attack_melee = Time.time + attackCooldownTime_melee;

				alreadyDamageApplied = false;
				idling = false;
				timer = 5f;
			}

			// Range Attack
			if (Input.GetButtonDown ("Fire2") && Time.time > next_attack_range)
			{
				animator.SetTrigger ("attacking_range");
				next_attack_range = Time.time + attackCooldownTime_range;
				audioSource.Play ();

				// Create laser object and adds a force to the looking side of the player
				GameObject laser = (GameObject)Instantiate (laserPrefab, laserSpawnPoint.position, Quaternion.identity);
				laser.tag = "Laser";
				if (lookingRight) 
					laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.right * laserSpeed);
				else
					laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.left * laserSpeed);

				idling = false;
				timer = 5f;
			}
		}
	}

	private void updateTimer()
	{
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
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.5F, whatIsGround, -Mathf.Infinity, 999);
		animator.SetBool ("grounded", grounded);

		animator.SetFloat ("speed", Mathf.Abs (rb2d.velocity.x));
	}




	/* OTHERS */

	// Locks the controll input of the player
	public void Lock ()
	{
		do_lock = true;
		Destroy (gameObject);
	}

	// Flips the player
	public void Flip ()
	{
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	// Adds the given collectamount to the player
	public void AddCollectable(int amount)
	{
	    collectAmount += amount;

		if(collectableText)
			collectableText.text = collectAmount.ToString() + " / " + maxCollectAmount.ToString();
	}
}
