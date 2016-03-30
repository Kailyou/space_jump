using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour
{

	public float healAmount = 1;

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) {
			other.collider.SendMessage ("AddHealth", healAmount);
			Destroy (gameObject);
		}
	}
}
