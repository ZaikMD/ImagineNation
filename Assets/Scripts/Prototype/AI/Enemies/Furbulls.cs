using UnityEngine;
using System.Collections;

public class Furbulls : BaseEnemy 
{
	public GameObject m_FurbullsProjectile;
	GameObject m_Player;

	// Use this for initialization
	void Start () 
	{
		m_Health.m_MaxHealth = 3;
		m_CombatRange = 2.0f;
		m_Player = GameObject.FindGameObjectWithTag ("Player");
	}

	protected override void die()
	{
		//TODO:play death animation and instantiate ragdoll
		Destroy (this.gameObject);
	}

	protected override void fightState()
	{
		//TODO:play attack animation and attack sound
		GameObject projectile = (GameObject)Instantiate (m_FurbullsProjectile, this.transform.position, this.transform.rotation);
		Vector3 direction = (this.transform.position - m_Player.transform.position).normalized;
		
		projectile.gameObject.GetComponent<FurbullsProjectile> ().setForwardDirection(direction)();
	}
}
