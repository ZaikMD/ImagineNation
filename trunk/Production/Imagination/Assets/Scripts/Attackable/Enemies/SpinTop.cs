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
	float m_DistanceToTarget;

	float m_DistanceToPlayer;
	float m_DistanceToPlayer2;

	public float m_DistanceFromChargeToPosition;

	bool m_PlayerHit;

	private enum FightStates
	{
		Wobble,
		Charge,
		BuildingUpCharge,
		KnockedBack
	}

	void Start () 
	{
		base.Start ();

		m_CharacterController = GetComponent<CharacterController> ();

		m_FightState = FightStates.BuildingUpCharge;
		m_AggroRange = 10.0f;
		m_CombatRange = m_AggroRange;

		m_MaxWobbleTime = 2.0f;
		m_WobbleTimer = m_MaxWobbleTime;

		m_ChargeTimer = 0.0f;
		m_ChargeBuildUpTime = 1.0f;
		m_ChargeSpeed = 5.0f;
		m_IsCharging = false;

		m_PlayerHit = false;

	}

	void Update () 
	{
		base.Update ();
	}

	protected override void Fight()
	{
		UpdateFightState ();
	}

	void UpdateFightState ()
	{
		switch(m_FightState)
		{
		case FightStates.Wobble:
			Wobble ();
			Debug.Log("Wobble");
			break;
		case FightStates.Charge:
			Debug.Log("Charge");
			Charge ();
			break;
		case FightStates.BuildingUpCharge:
			Debug.Log("BuildingUpCharge");
			BuildingUpCharge ();
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

	void Wobble()
	{
		if(m_WobbleTimer > 0.0f)
		{
			m_WobbleTimer -= Time.deltaTime;
		}
		else
		{
			m_DistanceToPlayer = Vector3.Distance(m_Target.position, transform.position);
			if(m_DistanceToPlayer > m_CombatRange)
			{
				m_State = State.Default;
				return;
			}

			m_FightState = FightStates.BuildingUpCharge;
			m_WobbleTimer = m_MaxWobbleTime;
		}
	}

	void Charge()
	{
		Debug.DrawRay(m_ChargeToPosition, transform.position - m_ChargeToPosition, Color.magenta);
		m_Agent.speed = m_ChargeSpeed;
		m_DistanceFromChargeToPosition = Vector3.Distance (m_ChargeToPosition, transform.position);

		if(!m_PlayerHit)
		{
			if(m_DistanceFromChargeToPosition < 3.0f)
			{
				m_FightState = FightStates.Wobble;

			}
		}
		else
		{
			m_FightState = FightStates.KnockedBack;
		}
	}

	void BuildingUpCharge()
	{
		if(m_ChargeTimer < m_ChargeBuildUpTime)
		{
			m_ChargeTimer += Time.deltaTime;
		}
		else
		{
			m_ChargeDirection = m_Target.position - transform.position;
			m_DistanceToTarget = m_ChargeDirection.magnitude;
			
			m_ChargeDistance = m_DistanceToTarget * 1.5f;
			m_ChargeToPosition = m_ChargeDirection.normalized * m_ChargeDistance;

			m_Agent.SetDestination(m_ChargeToPosition);

			m_ChargeTimer = 0.0f;
			m_FightState = FightStates.Charge;

		}
	}

	void KnockedBack()
	{
		//TODO: Bounce top back
		m_FightState = FightStates.BuildingUpCharge;
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
