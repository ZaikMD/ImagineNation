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
/* Mathieu Elias 16/10/2014 : Made it so the mage raycasts towards target before shooting 
 * 
 */
#endregion

public class GnomeMage : BaseEnemy 
{
	private enum FightStates
	{
		Regular = 0,
		Cloning,
		Cloned
	}

	public GameObject m_LookPoint;
	public GameObject m_ProjectilePrefab; 

	private FightStates m_CurrentFightState;

	//Shield Variables
	private float m_ShieldRechargeTime = 3.0f;
	private float m_ShieldTimer;
	private const int m_MaxHealth = 3;
	private bool m_Invulnerable = false;

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

	// Cloning Variables
	private bool m_Moving = false;
	private Vector3 m_Destination;
	public float m_JumpBackDistance = 5.0f;
	public float m_JumpBackSpeed = 25.0f;

	private Vector3 m_PrevPosition;


	//Sound Varibles
	SFXManager m_SFX;

	// Use this for initialization
	void Start () 
	{
		//Sound Varibles
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();

		//Base enemy start
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
			// When his shield is down he begins cloning
				//TODO: Deactivate Shield
				m_CurrentFightState = FightStates.Cloning;
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
				//TODO: Activate Shield
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

		case FightStates.Cloning:
			Cloning();
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
	/// Cloning Fight State
	/// </summary>
	private void Cloning()
	{
		m_Invulnerable = true;

		if (!m_Moving)
		MoveBack ();

		float dist = Vector3.Distance (transform.position, m_Destination);
		if ( dist <= 1.0f || transform.position == m_PrevPosition)
			m_Moving = false;

		m_PrevPosition = transform.position;

		if (!m_Moving)
		CreateClones ();

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
		if (m_Target != null)
		{

			if (m_CanShoot)
			{
				RaycastHit hitInfo;
			
				// Look at the target
				transform.LookAt(m_Target.position);
			
			
				// Raycast the way the gnome is looking, if it hits the player then shoot
				if (!Physics.Raycast(m_LookPoint.transform.position, m_LookPoint.transform.forward,out hitInfo))
					return;
			
				if (hitInfo.collider.tag == Constants.PLAYER_STRING)
				{
					// Create the projectile
					Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);
					//Play Lauch sound
					m_SFX.playSound(this.gameObject, Sounds.MageAttack);
					// bring the shot timer back up
					m_ShotTimer = m_TimeBetweenShots;
					// He just shot so set his can shoot to false
					m_CanShoot = false;
				}
			}
		}
	}

	/// <summary>
	/// Move away from the target 
	/// </summary>
	private void MoveAway ()
	{
		if (m_Target != null)
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

	}

	/// <summary>
	/// Move Back
	/// </summary>
	private void MoveBack()
	{
		transform.LookAt (m_Target.position);

		m_Destination = FindJumpBackPoint (0.0f);

		if (m_Destination == Vector3.zero)
						CreateClones ();
	
		m_Agent.SetDestination (m_Destination);

		m_Moving = true;
	}

	/// <summary>
	/// Find a valid point to move to
	/// </summary>
	/// <returns>The jump back point.</returns>
	/// <param name="angle">Angle.</param>
	private Vector3 FindJumpBackPoint(float angle)
	{
		if (angle == 360.0f)
			return Vector3.zero;

		Vector3 point = RotateAboutOrigin (transform.position, m_Target.position, angle);

		point += (transform.position - m_Target.position).normalized * m_JumpBackDistance;

		Vector3 dir = point - transform.position;
		RaycastHit hitInfo;

		//If we collided with something we need to choose a new point
		if (Physics.Raycast (transform.position, dir, out hitInfo, m_JumpBackDistance))		
			FindJumpBackPoint(angle + 20);		

		return point;
	}

	/// <summary>
	/// Creates the clones and chooses random positions in which to put the real gnome and his clones
	/// </summary>
	private void CreateClones()
	{
		// Create As many random positions as there are clones and +1 for the acutal gnome
		Vector3[] positions = new Vector3[m_NumberOfClones + 1];
	
		for (int i = 0; i < positions.Length; i++)
		{			
			float angle = UnityEngine.Random.Range(0.0f, 2.0f * Mathf.PI);
			
			Vector3 loc = new Vector3( Mathf.Cos(angle),0,Mathf.Sin(angle));
			if (m_Target != null)
			loc = (loc.normalized * m_CloneSpawnDistance) + m_Target.position;

			positions[i] = loc;				
		}

		//Creating the clones and setting their destination
		for (int i = 0; i < m_NumberOfClones; i++)
		{		
			GameObject clone = (GameObject) Instantiate(m_ClonePrefab, transform.position, transform.rotation);
			m_Clones.Add(clone);

			clone.GetComponent<GnomeClone>().OnStartUp();
			clone.GetComponent<GnomeClone>().SetPosition(positions[i]);
			clone.GetComponent<GnomeClone>().SetTarget(m_Target);
		}
		// Setting the real gnomes destination
		m_Agent.destination = positions[positions.Length - 1];

		// Setting the time till shield recharges	
		m_ShieldTimer = m_ShieldRechargeTime;
		// Clones have been created
		m_ClonesCreated = true;  
		m_Invulnerable = false;
		m_CurrentFightState = FightStates.Cloned;
	}

	/// <summary>
	/// Destroys all the created clones
	/// </summary>
	private void DestroyClones()
	{
		// Loop through and destroy all created clones
		for (int i = 0; i < m_NumberOfClones; i++)
		{
			if (m_Clones[i] != null)
			{
				Destroy(m_Clones[i]);
			}
		}
		m_Clones.Clear ();
		m_ClonesCreated = false;
	}

	/// <summary>
	/// call this when gnome dies
	/// </summary>
	protected override void Die ()
	{
		// If there are clones this will destroy them
		if (m_ClonesCreated)
		DestroyClones ();
	}

	public override void onHit (PlayerProjectile proj)
	{
		if (!m_Invulnerable)
			if (this.tag != Constants.PLAYER_STRING)
				m_Health -= 1;        

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
