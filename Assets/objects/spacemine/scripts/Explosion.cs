using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("die", 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void die ()
	{
		Destroy(gameObject);
	}
}
