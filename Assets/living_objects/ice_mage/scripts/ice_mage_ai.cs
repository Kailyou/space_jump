using UnityEngine;
using System.Collections;

public class ice_mage_ai : MonoBehaviour
{
	/* CONFIG */
	private float walkSpeed    			= 0.5f;
	private float hitDamage_melee		= 4f;
	private float hitDamage_range  		= 2f;

	/* REFERENCES */
	private Rigidbody2D	rb2d;
	private Animator 	animator;
	private GameObject  player;
	private EnemyHealth health;

	/* RAYCAST */
	public Transform start_attack_range;
	public Transform end_attack_range;
	public bool playerAttackRange_rangeAttack;
	public bool playerDetected;

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
		RaycastingPlayerDetected ();

		animator.SetBool ("PlayerDetected", playerDetected);
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

	/* RAYCAST */

	private void RaycastingPlayerDetected()
	{
		Debug.DrawLine (start_attack_range.position, end_attack_range.position, Color.green);
		playerAttackRange_rangeAttack = Physics2D.Linecast (start_attack_range.position, end_attack_range.position, 1 << LayerMask.NameToLayer ("Player"));
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
