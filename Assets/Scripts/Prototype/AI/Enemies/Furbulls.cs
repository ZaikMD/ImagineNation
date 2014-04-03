using UnityEngine;
using System.Collections;

public class Furbulls : BaseEnemy 
{
	// Use this for initialization
	void Start () 
	{
		m_Health = 3.0f;
		m_CombatRange = 2.0f;
	}

	protected override void die()
	{
		//play death animation destroy and instantiate body
	}

	protected override void fightState()
	{
		//play attack animation 
	}
}
