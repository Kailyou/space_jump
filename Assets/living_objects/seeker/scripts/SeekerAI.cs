using UnityEngine;
using System.Collections;

public class SeekerAI : MonoBehaviour 
{
	// Config
	public float walking_speed 	= 0;
	public bool respect_bounds	= true;

	// References
	private Rigidbody2D rb2d;
	private Animator animator;
	private Vector3 scaleBackup;
	private EnemyHealth enemyHealth;

	// DOT
	private int dotDamage 			= 1;
	private float dotCooldownTime	= 0.5f;
	private float next_dot_time		= 0f;

	// Stats
	private bool started = false;

	// Use this for initialization
	void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		scaleBackup = transform.localScale;

		animator.SetBool ("walking", false);
	}

	void OnBecameVisible()
	{
		started = true;
	}

	void FixedUpdate ()
	{
		if (!started)
			return;

		if (enemyHealth.isDeath ())
			return;

		rb2d.velocity = new Vector2 (walking_speed, rb2d.velocity.y);

		// Set speed to 0 if death
		if (enemyHealth.health == 0)
		{
			rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
		}

		Vector3 scale = transform.localScale;

		// Flip the graphic
		if (rb2d.velocity.x >= 0.1f) 
		{
			scale.x = -scaleBackup.x;
		} 
		else if (rb2d.velocity.x <= 0.1f) 
		{
			scale.x = scaleBackup.x;
		}

		transform.localScale = scale;

		animator.SetBool ("walking", (walking_speed!=0));
	}


	// Is used for the collision between Seeker and the bounds.
	// The Seeker will turn around after a collision with a bound.
	void OnBoundsTrigger ()
	{
		if (respect_bounds) 
		{
			walking_speed = -walking_speed;
		}
	}

	// When there is a collision between Seeker and another object,
	// the Seeker will turn around. 
	void OnCollisionEnter2D (Collision2D other) 
	{
		if (respect_bounds)
		{
			if (other.collider.CompareTag ("Enemy"))
			{
				walking_speed = -walking_speed;
			}
		}
	}

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")  && Time.time > next_dot_time)
		{
			next_dot_time = Time.time + dotCooldownTime;

			other.collider.SendMessage ("ApplyDamage", dotDamage);
		}
	}

	private void ApplyDamage(int damage)
	{
		enemyHealth.UpdateHP (damage);
	}
}
