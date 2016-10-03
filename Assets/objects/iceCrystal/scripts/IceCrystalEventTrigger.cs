using UnityEngine;
using System.Collections;

public class IceCrystalEventTrigger : MonoBehaviour 
{
	/* REFERENCES */
	public GameObject crystal;

	// Use this for initialization
	void Start ()
	{
		
	}

	// if the player collides with the event trigger hit box of the IceCrystal,
	// the crystal will fall down and apply damage if hit the player
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			crystal.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}
}
