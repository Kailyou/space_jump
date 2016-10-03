using UnityEngine;
using System.Collections;

public class IceSpike : MonoBehaviour
{
	/* DOT */
	private int dotDamage				= 1;
	private float dotCooldownTime		= 0.5f;
	private float next_dot_time 		= 0f;

	void OnCollisionStay2D (Collision2D other)
	{
		if (other.collider.CompareTag ("Player")  && Time.time > next_dot_time)
		{
			next_dot_time = Time.time + dotCooldownTime;

			other.collider.SendMessage ("ApplyDamage", dotDamage);
		}
	}
}
