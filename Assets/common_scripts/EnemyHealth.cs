using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

	private Animator animator;

	public float health = 1;

	void Start ()
	{
		animator = GetComponent<Animator> ();
	}

	void ApplyDamage (float damage)
	{
		health -= damage;

		if (health <= 0) {
			animator.SetTrigger ("hurt");
			Invoke ("die", 1);
		}
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
