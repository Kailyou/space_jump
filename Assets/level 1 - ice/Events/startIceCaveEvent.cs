using UnityEngine;
using System.Collections;

/*
 * Small event, which will turn off the snow effect
 * and lowers the light level
 */

public class startIceCaveEvent : MonoBehaviour 
{
	/* REFERENCES */
	public ParticleSystem snowEffect;
	private GameObject player;
	public MeshRenderer meshRendererBackground;

	void OnTriggerEnter2D(Collider2D other)
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		if (other.CompareTag ("Player"))
		{
			// Check if player is moving out or inside the cave
			// is he is looking right, he is moving inside
			// else he is moving outside
			if (player.GetComponent<PlayerController> ().isLookingRight ()) 
			{
				meshRendererBackground.enabled = false;

				// Stop snow effect
				snowEffect.Stop ();
				snowEffect.Clear ();
			} 
			else 
			{
				meshRendererBackground.enabled = true;

				snowEffect.Play ();
			}
		}
	}
}
