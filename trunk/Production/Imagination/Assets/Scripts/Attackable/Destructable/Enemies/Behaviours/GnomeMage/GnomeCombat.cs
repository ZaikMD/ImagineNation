using UnityEngine;
using System.Collections.Generic;
/*
 * Created by Mathieu Elias
 * Date: Nov 14, 2014
 *  
 * This script handles the functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion

public class GnomeCombat : BaseAttackBehaviour 
{
	enum CombatStates
	{
		Regular = 0,
		Cloning,
		Cloned
	}
	CombatStates m_CurrentCombatState = CombatStates.Regular;

	public BaseMovement m_CloningMovement;
	bool m_JumpedBack = false;

	public BaseCombat m_ClonedCombat;
	public BaseMovement m_ClonedMovement;

	public const float m_ClonedTime = 3.0f;
	float m_ClonedTimer = 0.0f;

	Vector3 m_Destination;

	GnomeShield m_Shield;

	const int m_NumberOfClones = 2;
	List<GnomeClone> m_Clones;
	const float m_ClonePosDist = 3.0f;

	const float m_TimeBetweenShots = 100.5f;
	float m_ShotTimer;

	Vector3 m_PrevPos;

	//prefabs
	public GameObject m_GnomeClonePrefab;


	// Use this for initialization
	protected override void start ()
	{
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
		m_Target = Target ();

		if (m_Target == null)
		{
			m_EnemyAI.SetState (EnemyAI.EnemyState.Idle);
			return;
		}

		float dist = Vector3.Distance (transform.position, m_Target.transform.position); 
		if (dist >= Constants.MAGE_ATTACK_RANGE)
			m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);

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

	void Regular()
	{
		Movement ();
		Combat ();

		if (m_EnemyAI.m_Health == 1)
		{
			DeactivateShield();
			m_CurrentCombatState = CombatStates.Cloning;
		}
	}

	void Cloning()
	{
		if (!m_JumpedBack)
		Movement ();

		Vector3 pos1 = new Vector3 (transform.position.x, 0, transform.position.z);
		Vector3 pos2 = new Vector3 (m_Destination.x, 0, m_Destination.z);

		float dist = Vector3.Distance (pos1, pos2);

		if (dist <= 2.0f || m_PrevPos == transform.position)
		{
			CreateClones ();
			m_ClonedTimer = m_ClonedTime;
			m_CurrentCombatState = CombatStates.Cloned;
		}
		m_PrevPos = transform.position;
	}

	void Cloned()
	{
		if (m_ClonedTimer <= 0)
		{
			m_CurrentCombatState = CombatStates.Regular;	
			m_Clones.Clear();
		}

		Movement ();
		Combat ();
		m_ClonedTimer -= Time.deltaTime;
	}

	void CreateClones()
	{
		Vector3[] positions = new Vector3[m_NumberOfClones + 1];
		
		for (int i = 0; i < positions.Length; i++)
		{			
			float angle = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);
			
			Vector3 loc = new Vector3( Mathf.Cos(angle),0,Mathf.Sin(angle));
			if (m_Target != null)
				loc = (loc.normalized * m_ClonePosDist) + m_Target.transform.position;
			
			positions[i] = loc;				
		}

		for (int i = 0; i < m_NumberOfClones; i++)
		{
			GameObject clone =(GameObject) Instantiate((Object) m_GnomeClonePrefab, transform.position, transform.rotation);
			GnomeClone gnomeClone = clone.GetComponent<GnomeClone>();

			gnomeClone.Create(m_ClonedMovement, m_ClonedCombat, positions[i], m_ClonedTime, m_Target, getProjectilePrefab());
			m_Clones.Add(gnomeClone);
		}

		m_MovementComponent.Movement (positions [positions.Length - 1]);
	}

	void DeactivateShield()
	{
		m_Shield.DeactivateShield (m_ClonedTime);
	}

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
