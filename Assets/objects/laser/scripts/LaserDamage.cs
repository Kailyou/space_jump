using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour {

	public float damage = 1;

	private Vector3 initialPosition;

	void Start() {
		initialPosition = transform.position;
	}

	void FixedUpdate ()
	{
		if (Vector3.Distance (initialPosition, transform.position) >= 5) {
			Destroy (gameObject);
		}
	}

	// if the laser collides with another 2d collider which is not the player's one
	// execute the ApplyDamage function, there will be no error if this function is not implemented.
	// laser will be destroyed itself after collision.
	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag ("Player") && !other.CompareTag ("ground_bounds") && !other.CompareTag("Bound"))
		{
			other.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);

			Destroy (gameObject);
		}
	}
}
