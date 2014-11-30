/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles Combat the functionality for the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections.Generic;

public class GnomeCombat : BaseAttackBehaviour 
{
	enum CombatStates
	{
		Regular = 0,
		Cloning,
		Cloned
	}
	CombatStates m_CurrentCombatState = CombatStates.Regular;

	GnomeShield m_Shield;

	const float m_TimeBetweenShots = 1.5f;
	float m_ShotTimer;
	
	Vector3 m_PrevPos;

	// Cloning state variables
	public BaseMovement m_CloningMovement;
	bool m_JumpedBack = false;

	const int m_NumberOfClones = 2;
	List<GnomeClone> m_Clones;
	const float m_ClonePosDist = 3.0f;

	Vector3 m_Destination;

	//Cloned state variables
	public BaseCombat m_ClonedCombat;
	public BaseMovement m_ClonedMovement;

	public const float m_ClonedTime = 3.0f;
	float m_ClonedTimer = 0.0f;


	//prefabs
	public GameObject m_GnomeClonePrefab;


	// Use this for initialization
	protected override void start ()
	{
		//Initialise all of the components
		m_TargetingComponent.start (this);
		m_CombatComponent.start (this);
		m_MovementComponent.start (this);
		m_CloningMovement.start (this);
		m_ClonedCombat.start (this);
		m_ClonedMovement.start (this);

		m_Shield = GetComponentInParent<GnomeShield> ();
		m_Clones = new List<GnomeClone> ();
	}
	
	// Update is called once per frame
	public override void update ()
	{
		// Grab the current target
		m_Target = Target ();

		// If we no longuer have a target the players are either dead, gone or something went wrong
		//return to idle state which doesnt require a target
		if (m_Target == null)
		{
			m_EnemyAI.SetState (EnemyAI.EnemyState.Idle);
			return;
		}

		// If we are no longuer in attack range go back to chase
		float dist = Vector3.Distance (transform.position, m_Target.transform.position); 
		if (dist >= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);

		//Calls the update for whichever combat state we are in
		switch (m_CurrentCombatState)
		{
		case CombatStates.Regular:
			Regular();
			break;

		case CombatStates.Cloning:
			Cloning();
			break;

		case CombatStates.Cloned:
			Cloned();
			break;
		}

		m_ShotTimer -= Time.deltaTime;
	}

	// Regular combat Update
	void Regular()
	{
		// Call movement and combat
		Movement ();
		Combat ();

		// If health is equal to one, deactivate the shield and switch to cloning combat state
		if (m_EnemyAI.m_Health == 1)
		{
			DeactivateShield();
			m_CurrentCombatState = CombatStates.Cloning;
		}
	}

	// Cloning combat update
	void Cloning()
	{
		// If we havent jumpedBack yet call movement
		if (!m_JumpedBack)
		Movement ();

		Vector3 pos1 = new Vector3 (transform.position.x, 0, transform.position.z);
		Vector3 pos2 = new Vector3 (m_Destination.x, 0, m_Destination.z);

		// If we have reached our position or something went wrong and we cant get any closer then 
		// its time to create the clones, set the cloned timer and switch the combat state to cloned
		float dist = Vector3.Distance (pos1, pos2);
		if (dist <= 2.0f || m_PrevPos == transform.position)
		{
			CreateClones ();
			m_ClonedTimer = m_ClonedTime;
			m_CurrentCombatState = CombatStates.Cloned;
		}
		m_PrevPos = transform.position;
	}

	// Cloned combat update
	void Cloned()
	{
		// If the cloned timer has run out then change the combat state to regular and clear the list of clones
		if (m_ClonedTimer <= 0)
		{
			m_CurrentCombatState = CombatStates.Regular;	
			m_Clones.Clear();
		}

		// Move, attack and decrement the cloned timer
		Movement ();
		Combat ();
		m_ClonedTimer -= Time.deltaTime;
	}

	// create the gnome clones
	void CreateClones()
	{
		// Find the positions for each gnome to travel to and attack from
		Vector3[] positions = new Vector3[m_NumberOfClones + 1];		
		for (int i = 0; i < positions.Length; i++)
		{			
			float angle = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);
			
			Vector3 loc = new Vector3( Mathf.Cos(angle),0,Mathf.Sin(angle));
			if (m_Target != null)
				loc = (loc.normalized * m_ClonePosDist) + m_Target.transform.position;
			
			positions[i] = loc;				
		}

		// Create the clones
		for (int i = 0; i < m_NumberOfClones; i++)
		{
			GameObject clone =(GameObject) Instantiate((Object) m_GnomeClonePrefab, transform.position, transform.rotation);
			GnomeClone gnomeClone = clone.GetComponent<GnomeClone>();

			gnomeClone.Create(m_ClonedMovement, m_ClonedCombat, positions[i], m_ClonedTime, m_Target, getProjectilePrefab());
			m_Clones.Add(gnomeClone);
		}

		// Move to the last chosen location to attack from
		m_MovementComponent.Movement (positions [positions.Length - 1]);
	}

	// Deactivate the gnomes shield
	void DeactivateShield()
	{
		// Deactivate the gnome shield and pass how long to stay deactivated for
		m_Shield.DeactivateShield (m_ClonedTime);
	}

	// Override of the Movement function since this behaviour needed extra movement components.
	protected override void Movement ()
	{
		if (m_EnemyAI.m_UMovement)
		{
			switch (m_CurrentCombatState)
			{
			case CombatStates.Regular:
				if (m_MovementComponent != null)			
					m_MovementComponent.Movement(m_Target);
				break;
				
			case CombatStates.Cloning:
				if (m_CloningMovement != null)	
					m_Destination = m_CloningMovement.Movement(m_Target);
					m_JumpedBack = true;
				break;
				
			case CombatStates.Cloned:
				if (m_ClonedMovement != null)	
					m_ClonedMovement.Movement(m_Target);				
				break;
			}
		}
	}

	// Override of the Combat function since this behaviour needed extra combat components.
	protected override void Combat ()
	{
		if (m_ShotTimer <= 0)
		{
			if(m_Target == null)
				return;

			transform.LookAt (m_Target.transform.position);			
			if (m_EnemyAI.m_UCombat)
			{
				switch (m_CurrentCombatState)
				{
				case CombatStates.Regular:
					if (m_CombatComponent != null)
						m_CombatComponent.Combat();				
					break;
					
				case CombatStates.Cloned:
					if (m_ClonedCombat != null)
						m_ClonedCombat.Combat();		
					break;
				}
			}

			m_ShotTimer = m_TimeBetweenShots;
		}
	}
}
