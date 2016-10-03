using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {

	public GameObject menuObject;

	private float nextEscTime = 0;

	// Use this for initialization
	void Start () {
		menuObject.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape) && Time.time > nextEscTime) {
			nextEscTime = Time.time + 0.2f;
			menuObject.active = !menuObject.active;
		}
	}

	public void onButtonRestart() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void onButtonMenu() {
		SceneManager.LoadScene ("Scenes/menu_main/scene");
	}

	public void onButtonExit() {
		Application.Quit ();
	}
}
