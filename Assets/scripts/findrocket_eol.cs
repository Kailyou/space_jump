using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class findrocket_eol : MonoBehaviour
{
	public GameObject rocket;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player")) {
			other.SendMessage ("Lock", SendMessageOptions.DontRequireReceiver);

			rocket.GetComponent<Animator> ().SetTrigger ("start");
			Invoke ("nextLevel", 4);
		}
	}

	void nextLevel ()
	{
		SceneManager.LoadScene ("Scenes/rocketcrash/rocketcrash");
	}
}
