using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevel : MonoBehaviour
{
	public PlayerController playerController;
	public Text messageText;
	public string nextScene;
	public string hint;

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Player"))
		{
			if (playerController.collectAmount == playerController.maxCollectAmount) 
			{
				SceneManager.LoadScene (nextScene);
			} 
			else 
			{
				messageText.text = hint;
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
