using UnityEngine;
using System.Collections;

public class wolf_ai : MonoBehaviour
{
	/* CONFIG */
	private float walkSpeed    			= 0.5f;                     
	private float chaseSpeed	     	= 1.5f;
	private float hitDamage        		= 2f;

	[HideInInspector] 
	private float flipWaitTime	 		= 2f;	
	[HideInInspector] 
	private float attackCooldownTime 	= 2f;	
	[HideInInspector] 
	private float attackDamageDelayTime	= 0.25f;

	/* REFERENCES */
	private Rigidbody2D	rb2d;
	private Animator 	animator;
	private GameObject  player;
	private EnemyHealth health;

	/* RAYCAST */
	public Transform sightStart;
	public Transform sightEnd;
	public Transform attackRangeStart;
	public Transform attackRangeEnd;
	public Transform hearRangeStart;
	public Transform hearRangeEnd;
	private bool playerDetected;
	private bool playerHeard;
	private bool playerInAttackRange;

	/* ATTACK */
	private float currentCooldownTime	 = 0f;
	private bool  attackOnCD        	 = false;
	private float currentDamageDelayTime = 0f;
	private bool  attacking 			 = false;

	/* COLLISIONS */
	private bool  respectBounds    	= true;
	private bool  reachedBound    	= false;
	private float currentWaitTime   = 0f;

	/* HURT */
	private bool hurting                = false;
	private float hurtTime              = 0.25f;
	private float currentHurtTime 		= 0f;




	void Start()
	{
		rb2d 		= GetComponent<Rigidbody2D> ();
		animator	= GetComponent<Animator> ();
		health      = GetComponent<EnemyHealth> ();
		player  	= GameObject.FindGameObjectWithTag ("Player");

		rb2d.isKinematic = true;

		// check start direction of the wolf
		Vector3 scale = transform.localScale;

		// if the x scale is less than 0
		// the object was flipped
		// change the speed to - to get movement in the other direction done.
		if (scale.x < 0)
		{
			walkSpeed  = -walkSpeed;
			chaseSpeed = -chaseSpeed;
		}
	}




	/* UPATE */
	void Update()
	{
		if (health.isDeath())
			return;

		// Check if the wolf detected the player so far by using a raycast
		// and check if the player is in attack range.
		RaycastingHeardPlayer ();
		RaycastingPlayerDetected ();
		RaycastingPlayerInAttackRange ();

		UpdateAttack ();

		// Speed is set to 0 as long as hurt timer is active
		UpdateHurtTimer ();

		// Flip the wolf if player was heard from behind
		// or a bound was reached
		if (reachedBound || playerHeard)
			Flip ();
	}

	private void UpdateAttack()
	{
		// If attack is on cool down,
		// update the cool down and leave function after
		if (attackOnCD) 
		{
			currentCooldownTime += Time.deltaTime;

			if (currentCooldownTime > attackCooldownTime)
			{
				attackOnCD = false;
				animator.SetBool ("AttackOnCD", false);
				currentCooldownTime = 0f;
			}

			return;
		}

		// Leave if Player is not in attack range
		if (!playerInAttackRange)
			return;

		// If attacking apply the damage after a short amount of time (damage delay)
		if (attacking)
		{
			currentDamageDelayTime += Time.deltaTime;

			if (currentDamageDelayTime > attackDamageDelayTime) 
			{
				player.SendMessage ("ApplyDamage", 2, SendMessageOptions.DontRequireReceiver);
				attacking 				= false;
				currentDamageDelayTime	= 0f;
			}

			return;
		}
		
		// Attack if player is in range and the attack is not on cool down yet.
		if (playerInAttackRange && !attackOnCD)
		{
			animator.SetTrigger ("Attacking");
			animator.SetBool ("AttackOnCD", true);
			attacking = true;
			attackOnCD = true;
		} 
	}

	private void UpdateHurtTimer()
	{
		if (!hurting)
		{
			return;
		} 
		else 
		{
			currentHurtTime += Time.deltaTime;

			if (currentHurtTime > hurtTime) 
			{
				hurting = false;
				currentHurtTime = 0f;
			}
		}
	}

	private void Flip()
	{
		// If player was heard, flip immidiatialy 
		if (playerHeard)
		{
			// flip the wolf by inverting x scale and the speed value
			Vector3 scale = transform.localScale;
			scale.x = -scale.x;
			transform.localScale = scale;

			walkSpeed   = -walkSpeed;                     
			chaseSpeed	= -chaseSpeed;
		}
		// flip by bound collision
		// flip after a short amount of time
		else
		{
			// start timer
			currentWaitTime += Time.deltaTime;

			if (currentWaitTime >= flipWaitTime)
			{
				// flip the wolf by inverting x scale and the speed value
				Vector3 scale = transform.localScale;
				scale.x = -scale.x;
				transform.localScale = scale;

				walkSpeed   = -walkSpeed;                     
				chaseSpeed	= -chaseSpeed;

				// reset variables
				currentWaitTime = 0f;
				reachedBound = false;
			} 
		}
	}


	/* FIXED UPDATE */

	void FixedUpdate ()
	{
		// Set speed to 0 if attacking, Flipping, hurting or dieing
		if (health.isDeath () || attackOnCD || hurting || reachedBound)
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
			animator.SetFloat ("Speed", Mathf.Abs(rb2d.velocity.x));
			return;
		} 
		else 
		{
			if (playerDetected) 
			{
				rb2d.velocity = new Vector2 (chaseSpeed, rb2d.velocity.y);
			} else 
			{
				rb2d.velocity = new Vector2 (walkSpeed, rb2d.velocity.y);
			}
		}

		animator.SetFloat ("Speed", Mathf.Abs(rb2d.velocity.x));
	}




	/* RAYCAST */

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


	private void ApplyDamage(int damage)
	{
		hurting = true;
		health.UpdateHP (damage);
	}


	/* COLLISION */

	// Turn arround after reached a bound.
	void OnBoundsTrigger ()
	{
		if (respectBounds)
		{
			reachedBound = true;
		}
	}
}