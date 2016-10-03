using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
	public PlayerController playerController;
	public Text messageText;

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			if (playerController.collectAmount == playerController.maxCollectAmount) 
			{
				SceneManager.LoadScene ("Scenes/rocketcrash/rocketcrash");
			} 
			else 
			{
				messageText.text = "Collect all energy before continue travelling.";
			}
		}
	}

	private void OnTriggerExit2D (Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			messageText.text = "";
		}
	}
}
