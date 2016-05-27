using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

	private Animator animator;
	private Rigidbody2D rb2d;
	private BoxCollider2D boxCollider2D;

	public float health = 1;

	void Start ()
	{
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		boxCollider2D = GetComponent<BoxCollider2D> ();
	}

	void ApplyDamage (float damage)
	{
		health -= damage;

		if (health <= 0) {
			DieNow ();
		}
	}

	public void DieNow ()
	{
		health = 0;
		rb2d.isKinematic = true;
		boxCollider2D.isTrigger = true;

		animator.SetTrigger ("hurt");
		Invoke ("die", 1);
	}

	IEnumerator SelfDestroy(){
		yield return new WaitForFixedUpdate();
		Destroy (this.gameObject);
	}

	private void die ()
	{
		this.gameObject.transform.Translate(Vector3.left * 9999);
		StartCoroutine("SelfDestroy");
	}
}
