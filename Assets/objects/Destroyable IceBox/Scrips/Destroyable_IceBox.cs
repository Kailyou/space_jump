using UnityEngine;
using System.Collections;

public class Destroyable_IceBox : MonoBehaviour 
{
	/* CONFIG */
	public float health = 2;

	void ApplyDamage()
	{
		health--;

		if (health == 0)
			Destroy (gameObject);
	}
}
