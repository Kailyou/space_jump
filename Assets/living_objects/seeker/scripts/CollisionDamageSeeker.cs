using UnityEngine;
using System.Collections;

public class CollisionDamageSeeker : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) {
			other.collider.SendMessage ("ActivateDot", "seeker", SendMessageOptions.DontRequireReceiver);
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) {
			//Debug.Log ("exit");
			other.collider.SendMessage ("DeactiveDot", SendMessageOptions.DontRequireReceiver);
		}
	}
}
