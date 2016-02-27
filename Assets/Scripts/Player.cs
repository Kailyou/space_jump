using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// configuration
	public float maxVelocity = 10;
	public float speed = 30;
	public float jumpPower = 250f;
	public int maxHealth = 100;

	// refs
	private Rigidbody2D rb2d;
	private Animator animator;

	// status vars
	private bool isMirrored = false;
	private bool canDoubleJump = false;
	private bool isCrouching = false;
	private bool died = false;

	// externally set
	public bool grounded = false; // Ridgidbody2D

	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}

	private void do_jump() {
		if(grounded) {
			rb2d.AddForce(Vector2.up*jumpPower);
			canDoubleJump = true;
		}
		else if(canDoubleJump) {
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
			rb2d.AddForce(Vector2.up*jumpPower);
			canDoubleJump = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// publish data to animator
		//animator.SetBool ("grounded", grounded);
		//animator.SetFloat ("speed", Mathf.Abs(rb2d.velocity.x));
		//animator.SetBool ("Crouching", isCrouching);

		if (died) {
			//animator.SetBool("Dead", true);
			//gameoverMenu.onGameOver(!isLocalPlayer);
			return;
		}

		// walk left image transformation
		if (!isMirrored && rb2d.velocity.x < -0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = true;
		}
		// walk right image transformation
		if (isMirrored && rb2d.velocity.x > 0.1) {
			Vector3 tmp = transform.localScale;
			transform.localScale = new Vector3(tmp.x*-1, tmp.y, tmp.z);
			isMirrored = false;
		}

		// jump
		if (!isCrouching) {
			if (Input.GetButtonDown("Jump"))
				do_jump ();
			if (Input.GetMouseButtonDown(0))
				do_jump ();

			foreach (var touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					do_jump ();
					break;
				}
			}
		}
	}

	void FixedUpdate() {
		if (died) {
			return;
		}

		if (!isCrouching) {
			float h = Input.GetAxis ("Horizontal");

			// stop walking immediatly when the user releases the button
			Vector3 tmp = rb2d.velocity;
			tmp.x *= 0.75f;
			rb2d.velocity = tmp;

			// walk left/right
			rb2d.AddForce (Vector2.right * speed * h);
			rb2d.AddForce (Vector2.right * speed * Input.acceleration.x);

			if (rb2d.velocity.x > maxVelocity) {
				rb2d.velocity = new Vector2 (maxVelocity, rb2d.velocity.y);
			} else if (rb2d.velocity.x < -maxVelocity) {
				rb2d.velocity = new Vector2 (-maxVelocity, rb2d.velocity.y);
			}
		}
	}
}
