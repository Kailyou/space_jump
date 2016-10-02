using UnityEngine;
using System.Collections;

public class MeleeAttackDamage : MonoBehaviour
{
	public PlayerController player;
	private Animator animator;

	public void Start()
	{
		animator = player.GetComponent<Animator>();
	}

	public void OnTriggerStay2D(Collider2D other)
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName ("Attack_Melee") && !player.isDamageAlreadyApplied() && other.CompareTag ("Enemy"))
		{
			other.SendMessage ("ApplyDamage", player.meleeDamage);	
			player.setDamageAlreadyApplied(true);
		}
	}
}
