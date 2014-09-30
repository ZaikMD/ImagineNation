using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class SpinTop : BaseEnemy 
{
	CharacterController m_CharacterController;
	public GameObject m_RagdollPrefab;
	FightStates m_FightState;

	float m_WobbleTimer;
	float m_MaxWobbleTime;

	float m_ChargeTimer;
	float m_ChargeBuildUpTime;
	float m_ChargeSpeed;
	bool m_IsCharging;
	Vector3 m_ChargeDirection;
	Vector3 m_ChargeToPosition;
	float m_ChargeDistance;

	bool m_PlayerHit;

	private enum FightStates
	{
		Default,
		Wobble,
		Charge,
		KnockedBack
	}

	void Start () 
	{
		base.Start ();

		m_CharacterController = GetComponent<CharacterController> ();

		m_FightState = FightStates.Charge;
		m_AggroRange = 10.0f;
		m_CombatRange = m_AggroRange;

		m_WobbleTimer = 0.0f;
		m_MaxWobbleTime = 1.0f;

		m_ChargeTimer = 0.0f;
		m_ChargeBuildUpTime = 2.0f;
		m_ChargeSpeed = 10.0f;
		m_IsCharging = false;
		m_ChargeDistance = 3.0f;

		m_PlayerHit = false;

	}

	void Update () 
	{
		base.Update ();
	}

	protected override void Fight()
	{
		switch(m_FightState)
		{
		case FightStates.Default:
			Default();
			Debug.Log("Default");
			break;
		case FightStates.Wobble:
			Wobble ();
			Debug.Log("Wobble");
			break;
		case FightStates.Charge:
			Debug.Log("Charge");
			Charge ();
			break;
		case FightStates.KnockedBack:
			Debug.Log("KnockedBack");
			KnockedBack ();
			break;
		default:
			break;
		}
	}

	protected override void Die()
	{
		Instantiate (m_RagdollPrefab, transform.position, transform.rotation);
	}

	void Default()
	{

	}

	void Wobble()
	{
		if(m_WobbleTimer < m_MaxWobbleTime)
		{
			m_WobbleTimer += Time.deltaTime;
		}
		else
		{
			m_FightState = FightStates.Charge;
		}
	}

	void Charge()
	{

		if(!m_IsCharging)
		{
			if(m_ChargeTimer < m_ChargeBuildUpTime)
			{
				m_ChargeTimer += Time.deltaTime;
				m_ChargeDirection = m_Target.position - transform.position;
			}
			else
			{
				m_ChargeToPosition = m_ChargeDirection.normalized * m_ChargeDistance;

				m_IsCharging = true;
				m_ChargeTimer = 0.0f;
			}
		}
		else
		{
			Vector3 tempDir = m_ChargeToPosition - transform.position;
			float DistanceFromChargeToPosition = tempDir.magnitude;

			if(DistanceFromChargeToPosition > 1.0f)
			{
				//transform.position += m_ChargeDirection.normalized * m_ChargeSpeed * Time.deltaTime;
				//m_CharacterController.Move(m_ChargeDirection.normalized * m_ChargeSpeed * Time.deltaTime);
				//m_Target = m_ChargeToPosition;
				m_Agent.SetDestination(m_ChargeToPosition);

				if(m_PlayerHit)
				{
					m_FightState = FightStates.KnockedBack;
				}
			}
			else
			{
				m_FightState = FightStates.Wobble;
				m_IsCharging = false;

				for (int i = 0; i < m_Players.Length; i++)
				{
					if (m_Players[i] != m_Target)
					{
						float distance = Vector3.Distance(gameObject.transform.position, m_Players[i].transform.position);
						
						if (GetDistanceToTarget() >= distance)
						{
							m_Target = m_Players[i].gameObject.transform;
						}
					}
				}

				m_Agent.SetDestination(m_Target.position);
			}

		}
	}

	void KnockedBack()
	{
		//TODO: Bounce top back
		m_FightState = FightStates.Charge;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			Destructable destructableObj = (Destructable)other.GetComponentInChildren<Destructable> ();
			if(destructableObj != null)
			{
				destructableObj.onHit(new EnemyProjectile());
				m_PlayerHit = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			m_PlayerHit = false;
		}
	}

}
