using UnityEngine;
using System.Collections;

public class BoundsTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		other.SendMessage ("onBoundsTrigger", SendMessageOptions.DontRequireReceiver);
	}
}
