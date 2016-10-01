using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	public void onStart() {
		SceneManager.LoadScene ("Scenes/Intro/Intro");
	}

	public void onExit() {
		Application.Quit ();
	}
}
