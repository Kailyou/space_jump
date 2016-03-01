using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour {

	public float maxHealthPoints	  	= 5;
	private float currentHealthPoints;
	private int maxLifePoints			= 3;
	private int currentLifePoints;
	private bool isDamageable 			= true;
	private bool isDead 				= false;

	private Animator animator;
	private PlayerController playerController;


	// Use this for initialization
	void Start () {
		animator        	= GetComponent<Animator> ();
		playerController    = GetComponent<PlayerController>();

		//test with the active scene,
		//this probably will be needed to be changed later on
		//the first scene which will need to use the healthController is the active scene
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
	
	void ApplyDamage(float damage)
	{
		Debug.Log ("LIFE: " + currentLifePoints + " HP: " + currentHealthPoints);

		if (isDamageable)
		{
			currentHealthPoints -= damage;

			//take the max of 0 and currentHealth
			currentHealthPoints = Mathf.Max (0, currentHealthPoints);

			if (!isDead)
			{
				if (currentHealthPoints == 0)
				{
					Dying();
				}
				else
				{
					Damaging();
				}

				isDamageable = false;
				Invoke ("ResetIsDamageAble", 1);
			}
		}
	}

	void ResetIsDamageAble() {
		isDamageable = true;
	}


	void Dying() {
		isDead = true;

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

	void Damaging() {
		animator.SetTrigger ("is_hurt");
	}

	//will be called on a scene change
	void OnDestroy() {
		PlayerPrefs.SetInt ("currentLifePoints", currentLifePoints);
		PlayerPrefs.SetFloat ("currentHealthPoints", currentHealthPoints);
	}
}
