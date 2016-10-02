using UnityEngine;
using System.Collections;

public class ice_mage_ai : MonoBehaviour
{
	/* CONFIG */
	private float walkSpeed    			= 0.5f;
	private float hitDamage_melee		= 4f;
	private float hitDamage_range  		= 2f;

	/* STATUS */
	private bool lookingRight = false;
	public bool playerDetected;

	/* REFERENCES */
	public GameObject mage;
	private Rigidbody2D	rb2d;
	private Animator 	animator;
	private GameObject  player;
	private EnemyHealth health;

	/* ATTACK */
	private float attackCooldownTime_range = 2f;
	private float next_attack_range = 0f;
	public GameObject iceProjectilePrefab;
	public Transform iceProjectileSpawnPoint;
	public float iceProjectileSpeed = 250;

	/* HURT */
	private bool hurting                = false;
	private float hurtTime              = 0.25f;
	private float currentHurtTime 		= 0f;

	// Use this for initialization
	private void Start () 
	{
		rb2d 		= GetComponent<Rigidbody2D> ();
		animator	= GetComponent<Animator> ();
		health      = GetComponent<EnemyHealth> ();
		player  	= GameObject.FindGameObjectWithTag ("Player");

		rb2d.isKinematic = true;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		if (!playerDetected)
			return;

		animator.SetBool ("PlayerDetected", playerDetected);

		if (player.transform.position.x > transform.position.x
		   && !lookingRight) 
		{
			Flip();
		}

		if (player.transform.position.x < transform.position.x
			&& lookingRight) 
		{
			Flip();
		}

		// Speed is set to 0 as long as hurt timer is active
		UpdateHurtTimer ();

		UpdateAttack ();
	}

	private void FixedUpdate()
	{
		// Set speed to 0 if wolf is death
		if (health.isDeath()) 
		{
			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
			return;
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

	private void UpdateAttack()
	{
		// Range Attack
		if(playerDetected && Time.time > next_attack_range)
		{
			next_attack_range = Time.time + attackCooldownTime_range;

			animator.SetTrigger ("Attack_IceProjectile");

			//audioSource.Play ();

			// Create laser object and adds a force to the looking side of the player
			GameObject iceProjectile = (GameObject)Instantiate (iceProjectilePrefab, iceProjectileSpawnPoint.position, Quaternion.identity);
			iceProjectile.tag = "IceProjectile";

			// Turn iceProjectile around if needed
			if (lookingRight)
			{
				Vector3 myScale = iceProjectile.transform.localScale;
				myScale.x *= -1;
				iceProjectile.transform.localScale = myScale;
			}

			Vector3 direction = player.GetComponent<Rigidbody2D>().transform.position - rb2d.transform.position;
			iceProjectile.GetComponent<Rigidbody2D> ().AddForce (direction.normalized * iceProjectileSpeed);
		}
	}


	private void ApplyDamage(int damage)
	{
		hurting = true;
		health.UpdateHP (damage);
	}




	/* OTHERS */

	public void Flip ()
	{
		lookingRight = !lookingRight;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
	}

	public void PlayerDetected()
	{
		playerDetected = true;
	}

	public void PlayerUndetected()
	{
		playerDetected = false;
	}

}
