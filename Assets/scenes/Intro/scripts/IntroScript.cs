using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour {

	private Text text;
	private enum State{
		STATE_1,
		STATE_2,
		STATE_3,
		STATE_4,
		STATE_FINAL,
	};
	private State state = State.STATE_1;
	private float stateTime = 0;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		reset ();
	}

	private void reset() {
		text.material.color = Color.white;
	}

	private void setText(string s) {
		text.text = s;
	}

	private void handleState(string s, float displayTime, bool fadeout=true) {
		// next
		if (stateTime > displayTime+1) {
			state++;
		}

		// fade out
		else if (stateTime > displayTime) {
			if (fadeout) {
				text.material.color = Color.Lerp (text.material.color, Color.clear, 2f * Time.deltaTime);
			} else {
				state++;
			}
		}

		// fade in
		else if (stateTime <= displayTime) {
			setText (s);
			text.material.color = Color.Lerp (text.material.color, Color.white, 2f * Time.deltaTime);
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
			handleState ("There was a Explorer on a small planet...", 2);
			break;

		case State.STATE_2:
			handleState ("But he needs some more energy to power is hyperdrive.", 4);
			break;

		case State.STATE_3:
			handleState ("He can find these on a nearby planet, so...", 3);
			break;

		case State.STATE_4:
			handleState ("LET'S GO", 3, false);
			break;

		case State.STATE_FINAL:
			SceneManager.LoadScene ("Scenes/Game-findrocket");
			break;
		}

		if (state != oldState) {
			stateTime = 0;
		}
	}
}
