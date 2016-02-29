using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour {

	public float damage = 1;

	//if the laser collides with another 2d collider which is not the player's one
	//execute the ApplyDamage function, there will be no error if this function is not implemented.
	//laser will be destroyed itself after collision.
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("test");
		if (!other.CompareTag ("Player")) {
			other.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
		}
	}
}
