using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBeginSignEvent : MonoBehaviour 
{
	public Text messageText; 
		
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			messageText.text = "Level 1 - Ice Planet";
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			messageText.text = "";
		}
	}
}
