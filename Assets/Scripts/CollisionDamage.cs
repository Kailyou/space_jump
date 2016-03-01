using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

	public float damage = 1;

	/*
	void OnCollisionEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) 
			other.SendMessage ("ApplyDamage", damage);
	}
	*/

	/*
	void OnCollisionStay2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
			other.SendMessage ("ApplyDamage", damage);
	}
	*/

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player")) 
			other.SendMessage ("ApplyDamage", damage);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
			other.SendMessage ("ApplyDamage", damage);
	}
}
