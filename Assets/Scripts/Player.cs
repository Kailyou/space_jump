using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	// configuration
	public float maxSpeed	 = 10;
	public float jumpForce   = 550;
	[HideInInspector]
	public bool lookingRight = true;
	public bool idling       = false;
	public bool crouching    = false;
	public LayerMask whatIsGround;
	public Transform groundCheck;
	private bool grounded    = false;

	// refs
	private Rigidbody2D rb2d;
	private Animator    animator;

	//timer
	private float timer = 3f;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		animator.SetBool ("crouching", false);
	}
		
	// Update is called once per frame
	void Update () {
		
		//idle timer
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

	void FixedUpdate()	{
		
		// walking left or right
		float hor = Input.GetAxis("Horizontal");

		animator.SetFloat ("speed", Mathf.Abs (hor));

		rb2d.velocity = new Vector2 (hor * maxSpeed, rb2d.velocity.y);

		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.15F, whatIsGround);

		animator.SetBool ("grounded", grounded);

		if ((hor > 0 && !lookingRight) || (hor < 0 && lookingRight)) 
			Flip ();
	}

	public void Flip() {
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}
}
