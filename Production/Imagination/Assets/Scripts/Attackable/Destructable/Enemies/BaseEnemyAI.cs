/*
 * Created by Jason Hein October 31/2014
 * Designed to replaced the current BaseEnemy script
 */ 
#region ChangeLog
/* 
 * 
 */
#endregion

using UnityEngine;
using System.Collections.Generic;


public class BaseEnemyAI : Destructable
{
	//Enemy States
	public enum EnemyState
	{
		Invalid,
		Idle,
		Chase,
		Attack,
		Dead,
		Count
	}
	EnemyState m_State = EnemyState.Idle;
	EnemyState m_ForcedState = EnemyState.Invalid;

	//The behavoirs of this enemy
	public BaseBehavoir m_IdleBehavoir;
	public BaseBehavoir m_ChaseBehavoir;
	public BaseBehavoir m_AttackBehavoir;
	public BaseBehavoir m_DeadBehavoir;

#if UNITY_EDITOR
	void Start ()
	{
		if (m_IdleBehavoir == null || m_ChaseBehavoir == null || m_AttackBehavoir == null || m_DeadBehavoir == null)
		{
			Debug.LogError(gameObject.name + " is missing a base enemy behavoir.");
		}
	}
#endif

	//Choose a Behavoir to update
	void Update ()
	{
		//Call a behavoir based on our state
		if (m_State == EnemyState.Idle || m_ForcedState == EnemyState.Idle)
		{
			m_IdleBehavoir.Update();
		}
		else if (m_State == EnemyState.Chase || m_ForcedState == EnemyState.Chase)
		{
			m_ChaseBehavoir.Update();
		}
		else if (m_State == EnemyState.Attack || m_ForcedState == EnemyState.Attack)
		{
			m_AttackBehavoir.Update();
		}
		else if (m_State == EnemyState.Dead || m_ForcedState == EnemyState.Dead)
		{
			m_DeadBehavoir.Update();
		}
	}

	//Sets the current state
	public virtual void SetState (EnemyState state)
	{
		if (state != null && m_State != state)
		{
			m_State = state;
			//Tell group our state
		}
	}

	//Returns the current state
	public EnemyState getState()
	{
		return m_State;
	}

	//Forces a state unless provided null
	public virtual void ForceState(EnemyState state)
	{
		if (state != null && m_ForcedState != state)
		{
			m_ForcedState = state;
		}
	}
}
