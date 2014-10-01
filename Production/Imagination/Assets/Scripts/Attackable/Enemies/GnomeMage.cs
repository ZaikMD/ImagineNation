using UnityEngine;
using System.Collections;

public class GnomeMage : BaseEnemy 
{
	private enum FightStates
	{
		Regular = 0,
		Cloned
	}

	public GameObject m_ProjectilePrefab; 
	public GameObject m_ClonePrefab;

	private FightStates m_CurrentFightState;

	//Shield Variables
	private int m_ShieldHealth;
	private float m_ShieldRechargeTime;
	private float m_ShieldTimer;

	private GameObject[] m_Clones;
	private Vector3 m_CloneSpawnDistance;

	//MoveAway Variables
	private float m_RotSpeed;
	private float m_BackSpeed;
	private float m_SwitchRotTimer;

	// Attack Variables
	private float m_TimeBetweenShots;
	private float m_ShotTimer;
	private bool m_CanShoot;

	// Use this for initialization
	void Start () 
	{
		base.Start ();

		m_CurrentFightState = FightStates.Regular;
		
		m_ShieldHealth = 2;
		m_ShieldRechargeTime = 2.0f;
	    m_ShieldTimer = 0.0f;

		m_RotSpeed = 15f;
		m_BackSpeed = 5f;
		m_SwitchRotTimer = Random.Range(0.3f, 3);

		m_TimeBetweenShots = 1.0f;
		m_ShotTimer = 0.0f;
		m_CanShoot = true;


		m_AggroRange = 20.0f;
		m_CombatRange = 10.0f;

		m_Clones = new GameObject[2];
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();

		if (m_ShieldHealth <= 0)
		{
			CreateClones();
		}

		m_ShotTimer -= Time.deltaTime;
		if (m_ShotTimer <= 0)
		{
			m_CanShoot = true;
		}
	}


	protected override void Chase ()
	{
		//This enemy does not Chase He will just start shooting the player
		Shoot ();
		m_State = State.Default;

	}

	protected override void Fight ()
	{

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

	private void Regular()
	{
		MoveAway ();
		Shoot ();

	}

	private void Cloned()
	{

	}

	private void Shoot()
	{
		if (m_CanShoot)
		{
			transform.LookAt(m_Target);
			m_CanShoot = false;
			m_ShotTimer = m_TimeBetweenShots;
			Instantiate (m_ProjectilePrefab, transform.position, transform.rotation);
		}
	}

	private void MoveAway ()
	{
		if (m_Agent.enabled)
			m_Agent.enabled = false;

		transform.RotateAround (m_Target.transform.position, Vector3.up,m_RotSpeed * Time.deltaTime);
		Vector3 dir = m_Target.transform.position - transform.position;
		dir.y = 0;
		
		transform.position -= dir.normalized * m_BackSpeed * Time.deltaTime;
		transform.LookAt (m_Target.transform.position);
		
		m_SwitchRotTimer -= Time.deltaTime;
		if (m_SwitchRotTimer <= 0)
		{
			m_RotSpeed *= -1;
			m_SwitchRotTimer = Random.Range(0.3f, 3);
		}

	}

	private void CreateClones()
	{
		Vector3[] positions = new Vector3[3];
		positions [0] = transform.position;
		positions [1] = m_Target.transform.position + m_CloneSpawnDistance;
		positions[2] = m_Target.transform.position - m_CloneSpawnDistance;
		Vector3 pos;
		int range;

		for (int i = 0; i < m_Clones.Length; i++)
		{
			do 
			{
			 range = Random.Range(0,2);
			 pos = positions[range];
			}
			while (pos != Vector3.zero);
		
			m_Clones[i] =(GameObject) Instantiate(m_ClonePrefab, pos, Quaternion.identity);	
			positions[range] = Vector3.zero;


		}

		for (int i = 0; i < positions.Length; i++)
		{
			if (positions[i] != Vector3.zero)
			{
				transform.position = positions[i];
			}
		}

	}

	protected override void Die ()
	{
		throw new System.NotImplementedException ();
	}
}
