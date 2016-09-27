using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {
	private AudioSource audioSource;
	private bool is_visible = false;

	void OnBecameInvisible() {
		is_visible = false;
	}

	void OnBecameVisible() {
		is_visible = true;
	}

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (is_visible && other.collider.CompareTag ("Ground")) {
			audioSource.Play ();
		}
	}
}
