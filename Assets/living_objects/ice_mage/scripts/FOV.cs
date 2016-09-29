using UnityEngine;
using System.Collections;

public class FOV : MonoBehaviour
{
	public GameObject iceMage;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			iceMage.SendMessage ("PlayerDetected");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			iceMage.SendMessage ("PlayerUndetected");
		}
	}
}
