/*
 * Created by Jason Hein October 31/2014
 * Designed to replaced the current BaseEnemy script
 */ 
#region ChangeLog
/* 
 * Changed to BaseIdleBehaviour, BaseChaseBehaviour,
 * BaseAttackBehaviour, BaseDeadBehaviour
 */
#endregion

using UnityEngine;
using System.Collections.Generic;


public class EnemyAI : Destructable
{
	//Enemy States
	public enum EnemyState
	{
		Idle = 1,
		Chase = 2,
		Attack = 4,
		Dead = 8,
		Count,
		Invalid = 0
	}
	EnemyState m_State = EnemyState.Idle;
	int m_NoUpdateStates = 0;

	//The behavoirs of this enemy
	public BaseIdleBehaviour m_IdleBehavoir;
	public BaseChaseBehaviour m_ChaseBehavoir;
	public BaseAttackBehaviour m_AttackBehavoir;
	public BaseDeadBehaviour m_DeadBehavoir;

	//UpdateComponent
	//TODO: switch to gets
	public bool m_UMovement = true;
	public bool m_UCombat = true;
	public bool m_UTargeting = true;
	public bool m_UEnterCombat = true;
	public bool m_ULeaveCombat = true;

	public GameObject m_ProjectilePrefab;

	//Choose a Behavoir to update
	void FixedUpdate ()
	{
		if (PauseScreen.IsGamePaused)
						return;

		if(((int)m_State & m_NoUpdateStates) != 0)
		{
			return;
		}

		//Call a behavoir based on our state
		if (m_State == EnemyState.Idle)
		{
			m_IdleBehavoir.update();
		}
		else if (m_State == EnemyState.Chase)
		{
			m_ChaseBehavoir.update();
		}
		else if (m_State == EnemyState.Attack)
		{
			m_AttackBehavoir.update();
		}
		else if (m_State == EnemyState.Dead)
		{
			m_DeadBehavoir.update();
		}
	}

	//Sets the current state
	public virtual void SetState (EnemyState state)
	{
		if (state != null && m_State != state)
		{
			//Set state
			m_State = state;

			//Tell group our state
		}
	}

	//Returns the current state
	public EnemyState getState()
	{
		return m_State;
	}

	//Forces a state to not update unless provided null
	public virtual void setNoUpdateStates(int state)
	{
		if (state != 0 && m_NoUpdateStates != state)
		{
			m_NoUpdateStates = state;
		}
	}

	//Forces a state unless provided null
	public virtual void addNoUpdateState(int state)
	{
		m_NoUpdateStates = m_NoUpdateStates | state;
	}

	public virtual void removeNoUpdateState(int state)
	{
		m_NoUpdateStates = m_NoUpdateStates ^ state;
	}

	//Forces a state unless provided null
	public virtual void setNoUpdateStates(EnemyState state)
	{
		setNoUpdateStates((int) state);
	}
	
	//Forces a state unless provided null
	public virtual void addNoUpdateState(EnemyState state)
	{
		addNoUpdateState ((int)state);
	}
	
	public virtual void removeNoUpdateState(EnemyState state)
	{
		removeNoUpdateState ((int)state);
	}

	public GameObject GetProjectilePrefab()
	{
		return m_ProjectilePrefab;
	}
}
