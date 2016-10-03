﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Intro_Help_One : MonoBehaviour
{
	public GameObject eventBound;
	public PlayerController playerController;

	private bool active     = false;
	private bool infoLocked = false;

	private float waitTime;

	public Text messageText;
		
	// Update is called once per frame
	void Update ()
	{
		if (active && Input.anyKey && Time.time > waitTime)
		{
			playerController.do_lock = false;
			messageText.text = " ";

			Destroy(eventBound);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("test1");

		if (other.CompareTag ("Player") && !infoLocked) 
		{
			playerController.do_lock = true;
			messageText.text = "Press arrow keys to move and jump.";
			infoLocked = true;
			active     = true;
			waitTime   = Time.time + 2f;
		}
	}
}
