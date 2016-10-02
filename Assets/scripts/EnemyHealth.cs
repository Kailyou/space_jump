using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	/* CONFIG */
	public float health = 5;
	public bool stay_after_death = false;

	/* REFERENCES */
	private Animator animator;
	private Rigidbody2D rb2d;
	private Collider2D collider2d;

	private bool death = false;

	/* INIT */
	void Start ()
	{
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		collider2d = GetComponent<Collider2D> ();
	}


	public void UpdateHP (int damageChange)
	{
		if (!death) 
		{
			health -= damageChange;

			if (health <= 0) 
			{
				DieNow ();
			} else 
			{
				animator.SetTrigger ("Hurt");
			}
		}
	}


	public void DieNow ()
	{
		death = true;

		if (!stay_after_death) {
			rb2d.isKinematic = true;
			collider2d.isTrigger = true;
		}

		animator.SetTrigger("Death");

		Invoke ("die", 1);
	}


	IEnumerator SelfDestroy(){
		yield return new WaitForFixedUpdate();
		Destroy (this.gameObject);
	}

	public bool isDeath()
	{
		return death;
	}

	private void die ()
	{
		if (!stay_after_death) {
			this.gameObject.transform.Translate (Vector3.left * 9999);
			StartCoroutine ("SelfDestroy");
		}
	}
}
