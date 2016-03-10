﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {

	//PLAYER STATS
	public float  maxHealthPoints	  	= 17;
	private float currentHealthPoints;
	private int   maxLifePoints			= 5;
	private int   currentLifePoints;
	private bool  isDamageable 			= true;
	private bool  isDead 				= false;

	//GUI
	public Image healthGui;
	public Text lifePointsText;
	public Text messageText;

	//DAMAGE DOT
	private bool  isDotActive  			= false;
	private float dotDamage				= 0f;
	private float dotTimerDelta 		= 0f;
	private float dotTimerTillDamage	= 0f;

	//REFERENCES
	private Animator animator;
	private PlayerController playerController;


	// Use this for initialization
	void Start () {
		animator        	= GetComponent<Animator> ();
		playerController    = GetComponent<PlayerController>();

		//start with max values at first scene
		if (SceneManager.GetActiveScene().buildIndex == 0) {
			currentLifePoints = maxLifePoints;
			currentHealthPoints = maxHealthPoints;
		} 
		else
		{
			currentHealthPoints	= PlayerPrefs.GetFloat ("currentHealthPoints");
			currentLifePoints   = PlayerPrefs.GetInt ("currentLifePoints");
		}

		messageText.text = "";
		updateGUI ();
	}

	void ApplyDamage(float damage)
	{
		if (isDamageable)
		{
			isDamageable = false;
			currentHealthPoints -= damage;
			//take the max of 0 and currentHealth
			currentHealthPoints = Mathf.Max (0, currentHealthPoints);
			updateGUI ();

			if (!isDead)
			{
				if (currentHealthPoints == 0)
				{
					isDead = true;
					Dying();
				}
				else
				{
					playHurtAnimation();
				}
					
				Invoke ("ResetIsDamageAble", 1);
			}
		}
	}

	void ResetIsDamageAble() {
		isDamageable = true;
	}

	//activates the dot damage
	void ActivateDot(string type)
	{
		isDotActive   	= true;

		if (type.Equals ("iceSpikes"))
		{
			dotDamage = 1f;
			dotTimerTillDamage	= 1f;	
		} 

		//secure that at least 1 damage will be taken each second
		if (dotTimerTillDamage <= 0)
		{
			dotDamage = 1f;
			dotTimerTillDamage = 1f;
		}
	}

	//deactivates the dot damage
	void DeactiveDot()
	{
		isDotActive = false;
		dotDamage 	= 0f;
	}

	public void AddHealth(float extraHealth)
	{
		currentHealthPoints += extraHealth;
		currentHealthPoints = Mathf.Min (currentHealthPoints, maxHealthPoints);
		updateGUI ();
	}

	//plays dieing animation and deactivates the player's controll
	//restarts the level if the player is not game over
	//restarts the whole game if the player is game over
	void Dying() {

		animator.SetBool ("is_dead", true);
		playerController.enabled = false;
		currentLifePoints--;

		if (currentLifePoints <= 0) {
			messageText.text = "Game Over";
			Invoke ("StartGame", 3); //start game with a three second delay
		} else {
			Invoke ("RestartLevel", 1);
		}
	}

	void StartGame() {
		SceneManager.LoadScene (0);
	}

	//revieves the player and activates the player's control again
	void RestartLevel() {
		currentHealthPoints = maxHealthPoints;
		isDead = false;

		animator.SetBool ("is_dead", false);

		playerController.enabled = true;

		//turn player if he is not looking right
		if(!playerController.lookingRight) {
			playerController.Flip ();
		}

		//TODO
		//generate new level and reset player
	}

	void playHurtAnimation() {
		animator.SetTrigger ("is_hurt");
	}


	void updateGUI()
	{
		//Set lifepoints
		lifePointsText.text = currentLifePoints.ToString ();

		//Set healthpoints
		healthGui.fillAmount = currentHealthPoints / maxHealthPoints;
	}


	/* update function */
	void Update ()
	{
		if (isDotActive) 
		{
			dotTimerDelta += Time.deltaTime;

			if (dotTimerDelta >= dotTimerTillDamage)
			{
				//Debug.Log ("test");
				ApplyDamage (dotDamage);
				dotTimerDelta = 0f;
			}
		}
	}


	/* will be called on a scene change */
	void OnDestroy() {
		PlayerPrefs.SetInt ("currentLifePoints", currentLifePoints);
		PlayerPrefs.SetFloat ("currentHealthPoints", currentHealthPoints);
	}
}
