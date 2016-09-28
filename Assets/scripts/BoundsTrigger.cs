using UnityEngine;
using System.Collections;

public class BoundsTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Wolf" || other.tag == "Seeker")
		{
			Debug.Log ("test");
			other.SendMessage ("OnBoundsTrigger", SendMessageOptions.DontRequireReceiver);
		}
	}
}
