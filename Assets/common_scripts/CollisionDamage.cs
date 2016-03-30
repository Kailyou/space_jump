using UnityEngine;
using System.Collections;

public class CollisionDamage : MonoBehaviour
{
	public string dotName = "";

	private EnemyHealth enemyHealth;
	private Collider2D currentCollider = null;

	void Start() {
		enemyHealth = GetComponent<EnemyHealth> ();
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (enemyHealth && enemyHealth.health <= 0)
			return;

		if (other.collider.CompareTag ("Player")) {
			other.collider.SendMessage ("ActivateDot", dotName, SendMessageOptions.DontRequireReceiver);
			currentCollider = other.collider;
		}
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")) {
			other.collider.SendMessage ("DeactiveDot", SendMessageOptions.DontRequireReceiver);
		}

		currentCollider = null;
	}

	void OnDestroy ()
	{
		if (currentCollider) {
			currentCollider.SendMessage ("DeactiveDot", SendMessageOptions.DontRequireReceiver);
		}
	}
}
