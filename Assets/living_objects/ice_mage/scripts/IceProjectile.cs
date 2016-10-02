using UnityEngine;
using System.Collections;

public class IceProjectile : MonoBehaviour
{
	public int damage = 4;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Vector3.Distance (initialPosition, transform.position) >= 20)
		{
			Destroy (gameObject);
		}
	}

	// if the laser collides with another 2d collider which is not the player's one
	// execute the ApplyDamage function, there will be no error if this function is not implemented.
	// laser will be destroyed itself after collision.
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Laser") || other.CompareTag("Ground"))
		{
			other.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);

			Destroy(gameObject);
		}
	}
}
