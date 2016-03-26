﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// references
	private Rigidbody2D rb2d;
	private Animator    animator;

	// configuration, visible inspector
	public float maxSpeed	 = 10;
	public float jumpForce   = 550;

	// configuration, not visible at inspector
	[HideInInspector]
	public bool lookingRight = true;
	[HideInInspector]
	public bool idling       = false;
	[HideInInspector]
	public bool crouching    = false;
	[HideInInspector]
	private bool grounded    = false;
	[HideInInspector]
	private bool jumping     = false;
	[HideInInspector]
	private bool attacking   = false;

	// collision detection
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// q attack (laser)
	public GameObject laserPrefab;
	public Transform  laserSpawnPoint;
	public float      laserSpeed = 500;

	//timer for idle animation
	private float timer = 3f;

	private bool do_lock = false;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		animator.SetBool ("crouching", false);
	}
		
	/* Update is called once per frame */
	void Update () {
			
		if (!do_lock) {
			/* input */

			//jump
			if (Input.GetButtonDown ("Jump") && grounded) {
				jumping = true;
			}

			//laser attack
			if (Input.GetButtonDown ("Fire1") && !attacking) {
				attacking = true;
			}
		}

		//idle 
		if (rb2d.velocity.x <= 0.1f) {
			idling = true;
		} else {
			idling = false;
			timer = 3f;
		}

		if(idling) {
			timer -= Time.deltaTime;
		}
			
		animator.SetBool ("idling", timer <= 0f);
	}

	/* FixedUpdate is called depending on a fixed amount of time */
	void FixedUpdate()	{

		if (!do_lock) {
			// walking left or right
			float hor = Input.GetAxis ("Horizontal");

			animator.SetFloat ("speed", Mathf.Abs (hor));

			rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);

			//change direction of the player by inverting the scale position
			if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight)) 
				Flip ();
		}

		//checks if the player is grounded by checking if the ground check position of the player
		//colides with any other circle
		//the both hit box circles of the player are excluded.
		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);
		animator.SetBool ("grounded", grounded);

		//jump
		if(jumping)	{
			rb2d.AddForce(new Vector2(0, jumpForce));
				jumping = false;
		}

		//attack
		if (attacking)	{
			animator.SetTrigger ("attacking");
			GameObject laser = (GameObject) Instantiate (laserPrefab, laserSpawnPoint.position, Quaternion.identity);

			if (lookingRight)
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.right * laserSpeed);
			else
				laser.GetComponent<Rigidbody2D> ().AddForce (Vector3.left * laserSpeed);

			attacking = false;
		}
	}

	public void Flip() {
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	public void Lock() {
		do_lock = true;
		Destroy (gameObject);
	}
}
