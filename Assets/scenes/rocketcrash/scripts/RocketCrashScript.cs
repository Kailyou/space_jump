using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RocketCrashScript : MonoBehaviour {

	public Text messageBox;
	private enum State{
		STATE_1,
		STATE_2,
		STATE_3,
		STATE_FINAL,
	};
	private State state = State.STATE_1;
	private float stateTime = 0;

	private void handleState(string s, float displayTime, bool fadeout=true) {
		// next
		if (stateTime > displayTime+1) {
			state++;
		}

		// fade out
		else if (stateTime > displayTime) {
			if (fadeout) {
				messageBox.material.color = Color.Lerp (messageBox.material.color, Color.clear, 2f * Time.deltaTime);
			} else {
				state++;
			}
		}

		// fade in
		else if (stateTime <= displayTime) {
			messageBox.text = s;
			messageBox.material.color = Color.Lerp (messageBox.material.color, Color.white, 2f * Time.deltaTime);
		}
	}
	
	// Update is called once per frame
	void Update () {
		State oldState = state;
		stateTime += Time.deltaTime;

		if (Input.GetKey (KeyCode.Escape)) {
			state = State.STATE_FINAL;
		}

		switch (state) {
		case State.STATE_1:
			break;

		case State.STATE_2:
			handleState ("OH NO!\n\nThe Engine still seems to be working but the Hyperdrive broke.", 4);
			break;

		case State.STATE_3:
			handleState ("Let's search the nearby space debris for replacement parts.", 5, false);
			break;

		case State.STATE_FINAL:
			SceneManager.LoadScene ("Scenes/Game-space");
			break;
		}

		if (state != oldState) {
			stateTime = 0;
		}
	}

	void OnCollisionEnter2D (Collision2D other) 
	{
		if (other.collider.CompareTag ("Ground") && state==State.STATE_1)
		{
			state++;
			stateTime = 0;
		}
	}
}
