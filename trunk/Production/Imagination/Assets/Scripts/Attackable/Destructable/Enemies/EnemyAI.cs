/*
 * Created by Jason Hein October 31/2014
 * Designed to replaced the current BaseEnemy script
 */ 
#region ChangeLog
/* 
 * Changed to BaseIdleBehaviour, BaseChaseBehaviour,
 * BaseAttackBehaviour, BaseDeadBehaviour
 * 
 * Added the controller variable along with the get and set methods - Mathieu Elias, Dec 1
 * 
 * Changed FixedUpdate to virtual - Joe Burchill Dec 3, 2014
 * 
 * Added the target variable as well as property - Mathieu Elias, Jan 8 2015
 * 
 */
#endregion

using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(CharacterController))]
public class EnemyAI : Destructable
{
	//Enemy States
	public enum EnemyState
	{
		Idle = 1,
		Chase = 2,
		Attack = 4,
		Dead = 8,
		KnockedBack = 16,
		Count = 5,
		Invalid = 0
	}
	EnemyState m_State = EnemyState.Idle;
	int m_NoUpdateStates = 0;

	//List of NotifyHits
	List<INotifyHit> m_NotifyHit = new List<INotifyHit>();

	//The behavoirs of this enemy
	public BaseIdleBehaviour m_IdleBehavoir;
	public BaseChaseBehaviour m_ChaseBehavoir;
	public BaseAttackBehaviour m_AttackBehavoir;
	public BaseDeadBehaviour m_DeadBehavoir;
	public BaseKnockedBackBehavouir m_KnockBackBehaviour;

	//UpdateComponent
	//TODO: switch to gets
	public bool m_UMovement = true;
	public bool m_UCombat = true;
	public bool m_UTargeting = true;
	public bool m_UEnterCombat = true;
	public bool m_ULeaveCombat = true;

	//public projectile prefab
	public GameObject m_ProjectilePrefab;
	
	EnemyController m_EnemyController;

	public bool m_IsInvincible { get; set; }

	protected GameObject m_Target;

    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

    public AnimatorEnemyBase i_Animator;

    protected virtual void Start()
    {
        if(i_Animator == null)
        {
            i_Animator = (AnimatorEnemyBase)gameObject.GetComponentInChildren(typeof(AnimatorEnemyBase));
#if DEBUG || UNITY_EDITOR
            if ( i_Animator == null)
            {
                Debug.LogError("fix this or ill break your legs");
            }
#endif
        }
    }

	//Choose a Behavoir to update
	public virtual void FixedUpdate ()
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

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
		else if (m_State == EnemyState.KnockedBack)
		{
			m_KnockBackBehaviour.update();
		}
		else if (m_State == EnemyState.Dead)
		{
			m_DeadBehavoir.update();
		}
	}

	//When the ai collides wtih something while falling through the air
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (m_State == EnemyState.KnockedBack)
		{
			m_KnockBackBehaviour.OnCollide(hit);
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

	//Removes the NoUpdate State by passing in an integer
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

	//Removes the NoUpdate State by passing in an EnemyState
	public virtual void removeNoUpdateState(EnemyState state)
	{
		removeNoUpdateState ((int)state);
	}

	//Returns the Projectile Prefab
	public GameObject GetProjectilePrefab()
	{
		return m_ProjectilePrefab;
	}

	//Sets the Enemy Controller
	public void SetController(EnemyController controller)
	{
		m_EnemyController = controller;
	}

	//Returns the Enemy Controller
	public EnemyController GetController()
	{
		return m_EnemyController;
	}

	//Overridden OnHit function for the player's light projectile attack
	public override void onHit(LightCollider proj, float damage)
	{
		//Call the notifyHit function to let the enemy's know when it gets hit
		NotifyHit();
		//Check if the enemy is invincible
		if(!m_IsInvincible)
		{
			m_KnockBackBehaviour.SetKnockBack(proj.KNOCKBACK, proj.transform.forward);
			base.onHit(proj, damage);
		}
	}

	//Overridden OnHit function for the player's heavy projectile attack
	public override void onHit(HeavyCollider proj, float damage)
	{
		//Call the notifyHit function to let the enemy's know when it gets hit
		NotifyHit();

		//Check if the enemy is invincible
		if(!m_IsInvincible)
		{
			m_KnockBackBehaviour.SetKnockBack(proj.KNOCKBACK, proj.transform.forward);
			base.onHit(proj, damage);
		}
	}

 	private void NotifyHit()
	{
		//Loop through the list of NotifyHits and call the function for all of them
		for(int i = 0; i < m_NotifyHit.Count; i++)
		{
			m_NotifyHit[i].NotifyHit();
		}
	}

	public void addNotifyHit(INotifyHit notifyHit)
	{
		//public function to add a NotifyHit to the list
		m_NotifyHit.Add (notifyHit);
	}

	public GameObject Target
	{
		get { return m_Target;}
		set {m_Target = value;}
	}
}