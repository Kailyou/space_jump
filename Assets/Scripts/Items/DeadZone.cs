using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) 
		{
			other.collider.SendMessage ("DieNow");
		}
	}
}
