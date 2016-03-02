using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour {

	public float  maxHealthPoints	  	= 5;
	private float currentHealthPoints;
	private int   maxLifePoints			= 3;
	private int   currentLifePoints;
	private bool  isDamageable 			= true;
	private bool  isDead 				= false;

	private bool  isDotActive  			= false;
	private float dotDamage				= 0f;
	private float dotTimerDelta 		= 0f;
	private float dotTimerTillDamage	= 0f;

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
	}


	void receiveDotDamage(float damage)
	{
		currentHealthPoints -= damage;
		currentHealthPoints = Mathf.Max (0, currentHealthPoints);

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
		}
	}

	void ApplyDamage(float damage)
	{
		if (isDamageable)
		{
			//Debug.Log ("lives :" + currentLifePoints + " , hp : " + currentHealthPoints);

			currentHealthPoints -= damage;

			//take the max of 0 and currentHealth
			currentHealthPoints = Mathf.Max (0, currentHealthPoints);

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

				isDamageable = false;
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
		dotDamage 			= 0f;
	}

	//plays dieing animation and deactivates the player's controll
	//restarts the level if the player is not game over
	//restarts the whole game if the player is game over
	void Dying() {

		animator.SetBool ("is_dead", true);

		playerController.enabled = false;

		currentLifePoints--;

		if (currentLifePoints <= 0) {
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

	//will be called on a scene change
	void OnDestroy() {
		PlayerPrefs.SetInt ("currentLifePoints", currentLifePoints);
		PlayerPrefs.SetFloat ("currentHealthPoints", currentHealthPoints);
	}

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
}
