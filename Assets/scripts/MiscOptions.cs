using UnityEngine;
using System.Collections;

public class MiscOptions : MonoBehaviour {

	public bool removeWhenInvisible = false;
	public float rotationSpeed = 0f;
	public Vector2 velocity;
	public GameObject startOtherWhenVisible = null;
	public bool waitUntilVisible = true;
	public bool restartOnDestroy = false;

	private Rigidbody2D rb2d = null;
	private bool started = false;
	private Vector2 startPosition;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		if (!waitUntilVisible)
			start ();

		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (rotationSpeed>0f) {
			transform.Rotate (Vector3.forward, rotationSpeed * Time.deltaTime);
		}
	}

	public void restart() {
		if (restartOnDestroy) {
			GameObject clone = (GameObject)Instantiate (gameObject, startPosition, Quaternion.identity);
		}
	}

	void OnBecameInvisible() {
		if (removeWhenInvisible) {
			restart ();
			Destroy (gameObject);
		}
	}

	void OnBecameVisible() {
		start ();
	}

	void start() {
		if (started)
			return;

		if (rb2d) {
			rb2d.velocity = velocity;
		}

		if(startOtherWhenVisible)
			startOtherWhenVisible.SendMessage("start", 0, SendMessageOptions.RequireReceiver);

		started = true;
	}
}
