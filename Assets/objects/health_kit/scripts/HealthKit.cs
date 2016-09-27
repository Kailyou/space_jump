using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour
{
	public float healAmount = 1;

	private AudioSource audioSource;
	private SpriteRenderer sp;
	private Collider2D collider2d;
	private bool is_destroyed = false;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		sp = GetComponent<SpriteRenderer> ();
		collider2d = GetComponent<Collider2D> ();
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (is_destroyed)
			return;
		
		if (other.collider.CompareTag ("Player")) {
			is_destroyed = true;
			sp.enabled = false;
			collider2d.enabled = false;

			other.collider.SendMessage ("AddHealth", healAmount);
			audioSource.Play ();
			Invoke ("die", 2);
		}
	}

	private void die() {
		Destroy (gameObject);
	}
}
