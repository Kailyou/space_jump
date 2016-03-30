using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) {
			other.collider.SendMessage ("ActivateDot", "iceSpikes", SendMessageOptions.DontRequireReceiver);
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
