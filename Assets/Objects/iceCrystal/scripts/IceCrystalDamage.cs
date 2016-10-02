using UnityEngine;
using System.Collections;

public class IceCrystalDamage : MonoBehaviour
{
	/* CONFIG */
	private int damage = 2;

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
			other.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			Destroy(crystal);
		}
	}
}
