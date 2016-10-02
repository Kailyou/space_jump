using UnityEngine;
using System.Collections;

public class BoundsTrigger : MonoBehaviour {

	public enum Position {Top, Right, Bottom, Left};

	public Position position;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Enemy")
		{
			other.SendMessage ("OnBoundsTrigger", this.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
