using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{

	void OnCollisionEnter2D (Collision2D other)
	{
		other.collider.SendMessage ("DieNow", SendMessageOptions.DontRequireReceiver);
	}
}
