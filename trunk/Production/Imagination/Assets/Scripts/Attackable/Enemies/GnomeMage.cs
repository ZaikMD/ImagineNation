using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Created by Mathieu Elias
 * Date: Sept 29, 2014
 *  
 * This script handles the functionality of the Gnome Mage enemy
 * 
 */
#region ChangeLog
/* 
 * 
 */
#endregion

public class GnomeMage : BaseEnemy 
{
	private enum FightStates
	{
		Regular = 0,
		Cloned
	}

	public GameObject m_ProjectilePrefab; 

	private const int m_MaxHealth = 3;

	private FightStates m_CurrentFightState;

	//Shield Variables
	private float m_ShieldRechargeTime = 2.0f;
	private float m_ShieldTimer;

	//Clones
	public GameObject m_ClonePrefab;
	private List<GameObject> m_Clones;
	public float m_CloneSpawnDistance = 5.0f;
	public int m_NumberOfClones = 2;
	private bool m_ClonesCreated;

	//MoveAway Variables
	public float m_RotSpeed = 15f;
	public float m_BackUpSpeed = 5f;
	private float m_SwitchRotTimer;
	public float m_MoveDist = 7.0f;
	private int m_MoveAngle;

	public float m_MinSwitchRotTime = 0.3f;
	public float m_MaxSwitchRotTime = 3.0f;

	// Attack Variables
	public float m_TimeBetweenShots = 1.5f;
	private float m_ShotTimer;
	private bool m_CanShoot;

	// Use this for initialization
	void Start () 
	{
		base.Start ();

		m_CurrentFightState = FightStates.Regular;

		// Set the health/shield variables
		m_Health = m_MaxHealth;
	    m_ShieldTimer = 0.0f;

		// Set the movement variables
		m_SwitchRotTimer = Random.Range(0.3f, 3);
		m_MoveAngle = 90;

		// Shooting variables
		m_ShotTimer = 0.0f;
		m_CanShoot = true;

		//Combat variables
		m_AggroRange = 20.0f;
		m_CombatRange = 10.0f;

		// Clone variables
		m_Clones = new List<GameObject>();

		m_ClonesCreated = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Call the Base Enemies Update
		base.Update ();

		// Do to the way base enemie was built I am just using when his health goes down to one to remove his shield
		if (m_Health == 1)
		{
			if (!m_ClonesCreated)
			{
			// When his shield is down create his two clones
			CreateClones();
			m_CurrentFightState = FightStates.Cloned;
			}
		}

		// If his shot timer is smaller or equal to zero he can shoot
		m_ShotTimer -= Time.deltaTime;
		if (m_ShotTimer <= 0)
		{
			m_CanShoot = true;
		}

		// If his clones have been created, start decreasing the timer for when the clones disappear and his health(Shield) comes back
		if (m_ClonesCreated)
		{
			m_ShieldTimer -= Time.deltaTime;
			if (m_ShieldTimer <= 0)
			{
				m_Health = m_MaxHealth;
				DestroyClones();
			
				m_CurrentFightState = FightStates.Regular;
				m_ClonesCreated = false;
				return;
			}
		}
	}

	/// <summary>
	/// Override of the base ennemies chase function
	/// </summary>
	protected override void Chase ()
	{
		//This enemy does not Chase He will just start shooting the player
		Shoot ();
		m_State = State.Default;

	}

	/// <summary>
	/// override of the base enemies fight state
	/// </summary>
	protected override void Fight ()
	{
		// If we are in the regular fight state or the clone one
		switch (m_CurrentFightState)
		{
		case FightStates.Regular:
			Regular();
			break;


		case (FightStates.Cloned):
			Cloned();
				break;			
		}

		m_State = State.Default;
	}

	/// <summary>
	/// Regular Fight State
	/// </summary>
	private void Regular()
	{
		MoveAway ();
		Shoot ();
	}

	/// <summary>
	/// Cloned fight state for when his shields are down and he spawns his clones
	/// </summary>
	private void Cloned()
	{
		Shoot();
	}

	/// <summary>
	/// Shoot your current target. The target is chosen in base enemy
	/// </summary>
	private void Shoot()
	{
		m_CanShoot = false;
		if (m_CanShoot)
		{
			// Look at the target
			transform.LookAt(m_Target.position);
			// Create the projectile
			Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);
			// bring the shot timer back up
			m_ShotTimer = m_TimeBetweenShots;
			// He just shot so set his can shoot to false
			m_CanShoot = false;
		}
	}

	/// <summary>
	/// Move away from the target 
	/// </summary>
	private void MoveAway ()
	{
		// Update the switch rotation timer
		m_SwitchRotTimer -= Time.deltaTime;

		// If the timer is smaller or equal to zero  then simply switch the direction of his rotation
		if (m_SwitchRotTimer <= 0)
		{
			m_MoveAngle *= -1;
			// Reset the timer to a random value between .3 and 3 seconds
			m_SwitchRotTimer = Random.Range(m_MinSwitchRotTime, m_MaxSwitchRotTime);
		}

		// Apply the rotation while trying to stay a certain dist from the player
		Vector3 point = RotateAboutOrigin (transform.position, m_Target.position, m_MoveAngle);
		point += (transform.position - m_Target.position).normalized * m_MoveDist;
		m_Agent.SetDestination(point);

	}

	/// <summary>
	/// Creates the clones and chooses random positions in which to put the real gnome and his clones
	/// </summary>
	private void CreateClones()
	{
		Vector3[] positions = new Vector3[m_NumberOfClones + 1];
	
		for (int i = 0; i < positions.Length; i++)
		{
			
			float angle = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);
			
			Vector3 loc = new Vector3( Mathf.Cos(angle),0,Mathf.Sin(angle));			
			loc = (loc.normalized * m_CloneSpawnDistance) + m_Target.position;

			positions[i] = loc;				
		}

		Vector3 pos;
		int range;

		for (int i = 0; i < m_NumberOfClones; i++)
		{
			do 
			{
			 range = Random.Range(0,2);
			 pos = positions[range];
			}while (pos == Vector3.zero);
		
			m_Clones.Add((GameObject) Instantiate(m_ClonePrefab, pos, Quaternion.identity));	
			positions[range] = Vector3.zero;
		}

		for (int i = 0; i < m_NumberOfClones; i++)
		{
			m_Clones[i].GetComponent<GnomeClone>().SetTarget(m_Target);
		}

		for (int i = 0; i < positions.Length; i++)
		{
			if (positions[i] != Vector3.zero)
			{
				transform.position = positions[i];
				m_Agent.destination = positions[i];
			}
		}
		
		m_ShieldTimer = m_ShieldRechargeTime;
		m_ClonesCreated = true;
	}

	/// <summary>
	/// Destroys all the created clones
	/// </summary>
	private void DestroyClones()
	{
		// Loop through and destroy all created clones
		for (int i = 0; i < m_NumberOfClones; i++)
		{
			Destroy(m_Clones[i]);
		}
		m_Clones.Clear ();
	}

	/// <summary>
	/// call this when you die
	/// </summary>
	protected override void Die ()
	{
		// If there are clones this will destroy them
		if (m_ClonesCreated)
		DestroyClones ();
	}

	/// <summary>
	/// Grab a point along a rotation around an object
	/// <returns>The about origin.</returns>
	/// <param name="point">Point.</param>
	/// <param name="origin">Origin.</param>
	/// <param name="angle">Angle.</param>
	public Vector3 RotateAboutOrigin(Vector3 point, Vector3 origin, float angle)
	{
		// Convert the angle to radians
		angle = angle * (Mathf.PI / 180);

		// Find out the new x and z locations 
		float rotatedX = Mathf.Cos (angle) * (point.x - origin.x) - Mathf.Sin (angle) * (point.z - origin.z) + origin.x;
		float rotatedZ = Mathf.Sin (angle) * (point.x - origin.x) - Mathf.Cos (angle) * (point.z - origin.z) + origin.z;

		return new Vector3(rotatedX, point.y, rotatedZ);
	} 



}
