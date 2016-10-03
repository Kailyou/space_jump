using UnityEngine;
using System.Collections;

public class IceSpike : MonoBehaviour
{
	/* CONFIG */
	private float dotCooldownTime_melee	= 0.5f;
	private float next_dot_time 		= 0f;
	private int damage					= 1;

	void OnCollisionStay2D (Collision2D other)
	{
		Debug.Log ("test");

		if (other.collider.CompareTag ("Player")  && Time.time > next_dot_time)
		{
			next_dot_time = Time.time + dotCooldownTime_melee;

			other.collider.SendMessage ("ApplyDamage", damage);
		}
	}
}
