using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private float m_ShieldRechargeTime;
	private float m_ShieldTimer;

	//Clones
	public GameObject m_ClonePrefab;
	private List<GameObject> m_Clones;
	private Vector3 m_CloneSpawnDistance;
	private int m_NumberOfClones;

	//MoveAway Variables
	private float m_RotSpeed;
	private float m_BackSpeed;
	private float m_SwitchRotTimer;

	// Attack Variables
	private float m_TimeBetweenShots;
	private float m_ShotTimer;
	private bool m_CanShoot;

	//TEST
	int dist = 7;

	// Use this for initialization
	void Start () 
	{
		base.Start ();

		m_CurrentFightState = FightStates.Regular;


		m_Health = m_MaxHealth;
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

		m_Clones = new List<GameObject>();
		m_NumberOfClones = 2;
		m_CloneSpawnDistance = new Vector3 (7, 0, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		base.Update ();

		if (m_Health ==1)
		{
			CreateClones();
			m_CurrentFightState = FightStates.Cloned;
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
		m_ShieldTimer -= Time.deltaTime;
		if (m_ShieldTimer <= 0)
		{
			m_Health = m_MaxHealth;

			foreach (GameObject clone in m_Clones)
			{
				Destroy(clone);
				m_Clones.Remove(clone);
			}
			m_CurrentFightState = FightStates.Regular;
			return;
		}

		Shoot();
	}

	private void Shoot()
	{
		m_CanShoot = false;
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

		//transform.RotateAround (m_Target.transform.position, Vector3.up,m_RotSpeed * Time.deltaTime);

		Vector3 point = RotateAboutOrigin (transform.position, m_Target.transform.position, 90);
		point += (transform.position - m_Target.transform.position).normalized * 7.0f;
		m_Agent.SetDestination(point);


		//float rotAngle = 90;
		//m_SwitchRotTimer -= Time.deltaTime;
		//if (m_SwitchRotTimer <= 0)
		//{
		//	
		//	m_SwitchRotTimer = Random.Range(0.3f, 3);
		//}
	}

	private void CreateClones()
	{
		Vector3[] positions = new Vector3[3];
		positions [0] = transform.position;
		positions [1] = m_Target.transform.position + m_CloneSpawnDistance;
		positions[2] = m_Target.transform.position - m_CloneSpawnDistance;
		Vector3 pos;
		int range;

		for (int i = 0; i < m_NumberOfClones - 1; i++)
		{
			do 
			{
			 range = Random.Range(0,2);
			 pos = positions[range];
			}
			while (pos != Vector3.zero);
		
			m_Clones.Add((GameObject) Instantiate(m_ClonePrefab, pos, Quaternion.identity));	
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

	
	public Vector3 RotateAboutOrigin(Vector3 point, Vector3 origin, float angle)
	{
		angle = angle * (Mathf.PI / 180);

		float rotatedX = Mathf.Cos (angle) * (point.x - origin.x) - Mathf.Sin (angle) * (point.z - origin.z) + origin.x;
		float rotatedZ = Mathf.Sin (angle) * (point.x - origin.x) - Mathf.Cos (angle) * (point.z - origin.z) + origin.z;

		return new Vector3(rotatedX, point.y, rotatedZ);
	} 

}
