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

	/* REFERENCES */
	private Rigidbody2D	rb2d;
	private Animator 	animator;
	private GameObject  player;
	private EnemyHealth health;

	/* RAYCAST */
	public Transform startRangeIceProjectile;
	public Transform endRangeIceProjectile;
	public bool playerDetected;
	public bool playerAttackRange_iceProjectile;

	/* ATTACK */
	private bool attacking_iceProjectile  = false;
	private bool attackOnCD_iceProjectile = false;
	private float currentCDIceProjectile = 0f;
	private float CDIceProjectile = 3f;
	public GameObject iceProjectilePrefab;
	public Transform iceProjectileSpawnPoint;
	public float iceProjectileSpeed = 200;

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

		RaycastingPlayerInAttackRange ();

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
		// Ice Projectile
		// If attack is on cool down,
		// update the cool down and leave function after
		if (attackOnCD_iceProjectile) 
		{
			currentCDIceProjectile += Time.deltaTime;

			if (currentCDIceProjectile > CDIceProjectile)
			{
				attackOnCD_iceProjectile = false;
				currentCDIceProjectile = 0f;
			}

			return;
		}

		// Leave if Player is not in attack range
		if (!playerAttackRange_iceProjectile)
			return;

		// Attack if player is in range and the attack is not on cool down yet.
		if (playerAttackRange_iceProjectile && !attackOnCD_iceProjectile)
		{
			animator.SetTrigger ("Attack_IceProjectile");
			attackOnCD_iceProjectile = true;
			//audioSource.Play ();

			// Create laser object and adds a force to the looking side of the player
			GameObject iceProjectile = (GameObject)Instantiate (iceProjectilePrefab, iceProjectileSpawnPoint.position, Quaternion.identity);
			iceProjectile.tag = "iceProjectile";

			Vector3 direction = player.GetComponent<Rigidbody2D>().transform.position - rb2d.transform.position;
			iceProjectile.GetComponent<Rigidbody2D> ().AddForce (direction.normalized * iceProjectileSpeed);
		} 
	}


	private void ApplyDamage(int damage)
	{
		hurting = true;
		health.UpdateHP (damage);
	}



	/* RAYCAST */

	private void RaycastingPlayerInAttackRange()
	{
		Debug.DrawLine (startRangeIceProjectile.position, endRangeIceProjectile.position, Color.green);
		playerAttackRange_iceProjectile = Physics2D.Linecast (startRangeIceProjectile.position, endRangeIceProjectile.position, 1 << LayerMask.NameToLayer ("Player"));
	}


	/* OTHERS */

	public void PlayerDetected()
	{
		playerDetected = true;
	}

	public void PlayerUndetected()
	{
		playerDetected = false;
	}

}
