using UnityEngine;
using System.Collections;

public class wolf_ai : MonoBehaviour
{
	private enum State
	{
		Idle,
		Walk,
		Chase,
		Flip,
		Hurt,
		Attack,
	};

	private State state;

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
	private bool  playerCollision	= false;
	private bool  damageReceived    = false;

	/* HURT */
	private bool invincible 			= false;
	private bool hurting                = false;
	private float hurtTime              = 1f;
	private float currentHurtTime 		= 0f;
	private float invincibleTime 		= 1.5f;
	private float currentInvincibleTime	= 0f;


	// Use this for initialization
	void Awake ()
	{
		rb2d 		= GetComponent<Rigidbody2D> ();
		animator	= GetComponent<Animator> ();
		health      = GetComponent<EnemyHealth> ();
		player  	= GameObject.FindGameObjectWithTag ("Player");

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

		// Start walking
		state = State.Walk;
	}




	/* UPATE */
	void Update()
	{
		/* DEBUG */
		//Debug.Log (state);

		// Set speed to 0 if wolf is death
		if (health.isDeath()) 
		{
			// update animation
			animator.SetBool ("walking", false);
			animator.SetBool ("chasing", false);
			return;
		}

		// Check if the wolf detected the player so far by using a raycast
		// and check if the player is in attack range.
		RaycastingPlayerDetected ();
		RaycastingHeardPlayer ();
		RaycastingPlayerInAttackRange ();

		// Update invincible state of the wolf
		if (invincible)
		{
			currentInvincibleTime += Time.deltaTime;

			if (currentInvincibleTime > invincibleTime) 
			{
				invincible 			  = false;
				currentInvincibleTime = 0f;
			}
		}


		// State machine
		switch (state)
		{
			case State.Idle:
			{
				// update animation
				animator.SetBool ("walking", false);
				animator.SetBool ("chasing", false);

				// Change state
				if (playerInAttackRange) 
				{
					state = State.Attack;
					break;
				}

				if (playerHeard)
				{
					state = State.Flip;
					break;
				}

				if (respectBounds && reachedBound)
				{
					state = State.Flip;
					break;
				}

				if (playerDetected)
				{
					state = State.Chase;
					break;
				}
				else
				{
					state = State.Walk;
					break;
				}
			}

			case State.Walk:
			{
				// Change state
				if (playerInAttackRange) 
				{
					state = State.Attack;
					break;
				}

				if (playerHeard)
				{
					state = State.Flip;
					break;
				}

				if (playerDetected)
				{
					state = State.Chase;
					break;
				}

				if (respectBounds && reachedBound)
				{
					state = State.Flip;
				}

				// Update animation
				animator.SetBool ("chasing", false);
				animator.SetBool ("walking", true);


				break;
			}

			case State.Chase:
			{
				// Change state
				if (playerInAttackRange) 
				{
					state = State.Attack;
					break;
				}

				if (!playerDetected)
				{
					state = State.Walk;
					break;
				}

				if (respectBounds && reachedBound)
				{
					state = State.Flip;
				}

				//update animation
				animator.SetBool ("walking", false);
				animator.SetBool ("chasing", true);

				break;
			}

			case State.Flip:
			{
				// Update animation
				animator.SetBool ("walking", false);
				animator.SetBool ("chasing", false);

				// If player was heard, flip immidiatialy 
				if (playerHeard)
				{
					// flip the wolf by inverting x scale and the speed value
					Vector3 scale = transform.localScale;
					scale.x = -scale.x;
					transform.localScale = scale;

					walkSpeed   = -walkSpeed;                     
					chaseSpeed	= -chaseSpeed;

					state = State.Idle;
				}
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

						state = State.Idle;
					} 
				}



				break;
			}

			case State.Attack:
			{
				Debug.Log ("test");


				// update animation
				animator.SetBool ("walking", false);
				animator.SetBool ("chasing", false);

				if (!attackOnCD) 
				{
					animator.SetTrigger ("attacking");
					attacking  = true;
					attackOnCD = true;
				} 
				else
				{
					// If attacking atm
					// Apply the damage after a short amount of time
					if(attacking)
					{
						currentDamageDelayTime += Time.deltaTime;
						if (currentDamageDelayTime > attackDamageDelayTime) 
						{
							attacking 			   = false;
							currentDamageDelayTime = 0f;
							player.SendMessage("ApplyDamage", 2, SendMessageOptions.DontRequireReceiver);
						}
					}

					// Update Cooldown
					currentCooldownTime += Time.deltaTime;

					// Check if cd is done
					if (currentCooldownTime > attackCooldownTime)
					{
						// Reset cd data
						attackOnCD 		    = false;
						currentCooldownTime = 0f;
						state = State.Idle;
					}
				}

				break;
			}


			case State.Hurt:
			{
				// update animation
				animator.SetBool ("walking", false);
				animator.SetBool ("chasing", false);

				currentHurtTime += Time.deltaTime;

				if (currentHurtTime > hurtTime) 
				{
					hurting 		= false;
					currentHurtTime = 0f;
					state			= State.Idle;
				}

				if (!hurting) 
				{
					hurting = true;

					animator.SetTrigger ("hurt");
						
					health.UpdateHP (-hitDamage);
				}

				break;
			}
		}
	}




	/* FIXED UPDATE
     * Used for the physic
	 */

	void FixedUpdate ()
	{
		// Set speed to 0 if wolf is death
		if (health.isDeath()) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
			return;
		}

		// STATE MACHINE
		switch (state)
		{
			case State.Walk:
			{
				rb2d.velocity = new Vector2 (walkSpeed, rb2d.velocity.y);
				break;
			}

			case State.Chase:
			{
				rb2d.velocity = new Vector2 (chaseSpeed, rb2d.velocity.y);
				break;
			}

			case State.Flip:
			{
				rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
				break;
			}

			case State.Hurt:
			{
				rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
				break;
			}

			case State.Attack:
			{
				rb2d.velocity = new Vector2 (0f, rb2d.velocity.y);
				break;
			}
		}
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




	/* COLLISION */

	// Turn arround after reached a bound.
	void OnBoundsTrigger ()
	{
		if (respectBounds)
		{
			reachedBound = true;
		}
	}

	// when collision between player and wolf started
	void OnCollisionEnter2D (Collision2D other)
	{
		// set speed to 0 if there is a collision with the player
		if (other.collider.CompareTag ("Player"))
		{
			playerCollision = true;
		}
	}

	// when collision between player and wolf stopped
	void OnCollisionExit2D(Collision2D other)
	{
		if (other.collider.CompareTag ("Player"))
		{
			playerCollision = false;
		}
	}


	/* OTHERS */
	private void ApplyDamage()
	{
		// Wolf does not receive damage if he is attacking
		// Also the wolf is invincible for a short amount of time
		// after he received the last attack.
		if (state != State.Attack && !invincible) 
		{
			invincible = true;
			state = State.Hurt;
		}
	}
}