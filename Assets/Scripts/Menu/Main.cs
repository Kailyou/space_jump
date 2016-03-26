using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public void onStart() {
		SceneManager.LoadScene ("Scenes/Game-findrocket");
	}

	public void onExit() {
		Application.Quit ();
	}
}
