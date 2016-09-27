using UnityEngine;
using System.Collections;

public class Spacemine : MonoBehaviour {
	public GameObject explosion;

	private MiscOptions misc = null;
	private bool is_visible = false;

	void OnBecameInvisible() {
		is_visible = false;
	}

	void OnBecameVisible() {
		is_visible = true;
	}

	// Use this for initialization
	void Start () {
		misc = GetComponent<MiscOptions> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Ground"))
			return;
		
		other.gameObject.SendMessage ("ApplyDamage", 5, SendMessageOptions.DontRequireReceiver);
		ApplyDamage (1);
	}

	private void ApplyDamage(float damage)
	{
		if (is_visible) {
			Instantiate (explosion, transform.position, Quaternion.identity);
		}
		Invoke ("die", 0.3f);
	}

	private void die ()
	{
		if (misc) {
			misc.restart ();
		}
		Destroy(gameObject);
	}
}
