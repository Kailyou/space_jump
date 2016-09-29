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

	// Use this for initialization
	void Start () 
	{
		rb2d 		= GetComponent<Rigidbody2D> ();
		animator	= GetComponent<Animator> ();
		health      = GetComponent<EnemyHealth> ();
		player  	= GameObject.FindGameObjectWithTag ("Player");

		rb2d.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate()
	{

	}
}
