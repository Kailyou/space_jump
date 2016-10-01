using UnityEngine;
using System.Collections;

public class BoundsTrigger : MonoBehaviour {

	public enum Position {Top, Right, Bottom, Left};

	public Position position;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Wolf" || other.tag == "Seeker" || other.tag == "Golem")
		{
			other.SendMessage ("OnBoundsTrigger", this.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
