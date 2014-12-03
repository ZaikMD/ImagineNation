// Created by Jason Hein Dec 3, 2014
//
// In progress
//
//
//

#region Changelog

#endregion


using UnityEngine;
using System.Collections;

public class JackInTheBossState : MonoBehaviour
{
	//Where along the animation this attack is
	public enum JackInTheBossAttackState
	{
		Entering = 0,
		Attacking,
		Exiting
	}
	protected JackInTheBossAttackState m_AttackState = JackInTheBossAttackState.Entering;

	/// <summary>
	/// Updates the state. It is called by JackInTheBoss.
	/// </summary>
	public void UpdateState ()
	{
		switch (m_AttackState)
		{
		case JackInTheBossAttackState.Entering:
		{
			Entering();
			break;
		}
		case JackInTheBossAttackState.Attacking:
		{
			Attacking();
			break;
		}
		case JackInTheBossAttackState.Exiting:
		{
			Exiting();
			break;
		}
		}
	}

	/// <summary>
	/// Enters the attack.
	/// </summary>
	public void EnterAttack()
	{
		m_AttackState = JackInTheBossAttackState.Entering;
	}

	/// <summary>
	/// Entering this attack.
	/// </summary>
	protected virtual void Entering()
	{

	}

	/// <summary>
	/// During the attack
	/// </summary>
	protected virtual void Attacking()
	{
		
	}

	/// <summary>
	/// Exiting the attack
	/// </summary>
	protected virtual void Exiting()
	{
		
	}
}
