using UnityEngine;
using System.Collections;

public class EnemyHealthSeeker : MonoBehaviour {

	private Animator    animator;

	void Start () {
		animator = GetComponent<Animator>();
	}

	void ApplyDamage(float damage) {
		animator.SetTrigger ("hurt");
		Invoke("die", 1);
	}

	private void die() {
		Destroy (gameObject);
	}
}
