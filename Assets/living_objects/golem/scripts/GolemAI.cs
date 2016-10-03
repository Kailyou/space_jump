using UnityEngine;
using System.Collections;

public class GolemAI : MonoBehaviour {
	// Config
	public float walking_speed = 0;
	public bool respect_bounds = true;
	public float attack_rate = 2f;

	// References
	private Rigidbody2D rb2d;
	private Animator animator;
	private Vector3 scaleBackup;
	private EnemyHealth enemyHealth;

	public Transform sightStart;
	public Transform sightEnd;
	public Transform attackRangeStart;
	public Transform attackRangeEnd;
	public Transform hearRangeStart;
	public Transform hearRangeEnd;
	private bool playerDetected;
	private bool playerHeard;
	private bool playerInAttackRange;

	private float next_attack = 0f;
	private bool in_player_collision = false;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		scaleBackup = transform.localScale;

		animator.SetBool ("walking", false);
	}

	void RaycastingPlayerDetected()
	{
		Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);
		playerDetected = Physics2D.Linecast (sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer ("Player"));
	}

	void RaycastingPlayerInAttackRange()
	{
		Debug.DrawLine (attackRangeStart.position, attackRangeEnd.position, Color.red);
		playerInAttackRange = Physics2D.Linecast (attackRangeStart.position, attackRangeEnd.position, 1 << LayerMask.NameToLayer ("Player"));
	}

	void RaycastingHeardPlayer()
	{
		Debug.DrawLine (hearRangeStart.position, hearRangeEnd.position, Color.blue);
		playerHeard = Physics2D.Linecast (hearRangeStart.position, hearRangeEnd.position, 1 << LayerMask.NameToLayer ("Player"));
	}
	
	// Update is called once per frame
	void Update () {

		if (enemyHealth.isDeath ())
			return;

		RaycastingHeardPlayer ();
		RaycastingPlayerDetected ();
		RaycastingPlayerInAttackRange ();

		if (!enemyHealth.isDeath ()) {
			if (playerInAttackRange && Time.time > next_attack) {
				next_attack = Time.time + attack_rate;
				animator.SetTrigger ("attack");
			}
		}
	}

	void FixedUpdate ()
	{
		if (!enemyHealth.isDeath ()) {
			rb2d.velocity = new Vector2 (walking_speed, rb2d.velocity.y);

			// Set speed to 0 if dead
			if (enemyHealth.health == 0) {
				rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
			}

			Vector3 scale = transform.localScale;

			// Flip the graphic
			if (rb2d.velocity.x >= 0.1f) {
				scale.x = scaleBackup.x;
			} else if (rb2d.velocity.x <= 0.1f) {
				scale.x = -scaleBackup.x;
			}

			transform.localScale = scale;
		} else {
			rb2d.velocity = Vector2.zero;
		}

		animator.SetBool ("walking", (Mathf.Abs(walking_speed)>=0.1f));
	}

	void OnBoundsTrigger (GameObject bounds)
	{
		if (enemyHealth.isDeath ())
			return;
		
		BoundsTrigger boundstrigger = bounds.GetComponent<BoundsTrigger> ();
		if (respect_bounds) 
		{
			bool flip = false;
			if (walking_speed >= 0.1f && boundstrigger.position == BoundsTrigger.Position.Right)
				flip = true;
			if (walking_speed <= 0.1f && boundstrigger.position == BoundsTrigger.Position.Left)
				flip = true;

			if(flip)
				walking_speed = -walking_speed;
		}
	}

	private void ApplyDamage(int damage)
	{
		if (enemyHealth.isDeath ())
			return;
		
		enemyHealth.UpdateHP (damage);
	}

	void OnCollisionEnter2D (Collision2D other) 
	{
		if (enemyHealth.isDeath ())
			return;
		
		if (other.collider.CompareTag ("Player"))
		{
			Vector3 contactPoint = other.contacts[0].point;
			Vector3 center = this.transform.position;

			bool right = contactPoint.x > center.x;
			bool left = contactPoint.x < center.x;

			if(right)
				walking_speed = Mathf.Abs(walking_speed);
			else if(left)
				walking_speed = -Mathf.Abs(walking_speed);
		}
	}

	void OnCollisionStay2D (Collision2D other)
	{
		if (enemyHealth.isDeath ())
			return;
		
		if (other.collider.CompareTag ("Player"))
		{
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("attack")) {
				other.gameObject.SendMessage("ApplyDamage", 5, SendMessageOptions.RequireReceiver);
			}
		}
	}
}
