using UnityEngine;
using System.Collections;

public class BoundsTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Wolf" || other.tag == "Seeker" || other.tag == "Golem")
		{
			other.SendMessage ("OnBoundsTrigger", SendMessageOptions.DontRequireReceiver);
		}
	}
}
