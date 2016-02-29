using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public float health = 5;

	void ApplyDamage(float damage) {
		health -= damage;

		if (health <= 0) {
			Destroy (gameObject);
		}
	}
}
