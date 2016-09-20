using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
	/* CONFIG */
	public float health = 5;

	/* REFERENCES */
	private Animator animator;
	private Rigidbody2D rb2d;
	private BoxCollider2D boxCollider2D;

	private bool death = false;




	/* INIT */
	void Start ()
	{
		animator = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		boxCollider2D = GetComponent<BoxCollider2D> ();
	}


	public void UpdateHP (float damageChange)
	{
		if (!death) 
		{
			health += damageChange;

			if (health <= 0) 
			{
				DieNow ();
			}		
		}
	}


	public void DieNow ()
	{
		death = true;

		rb2d.isKinematic = true;
		boxCollider2D.isTrigger = true;

		animator.SetTrigger("death");

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
		this.gameObject.transform.Translate(Vector3.left * 9999);
		StartCoroutine("SelfDestroy");
	}
}