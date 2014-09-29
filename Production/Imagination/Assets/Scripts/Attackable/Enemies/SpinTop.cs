using UnityEngine;
using System.Collections;

public class SpinTop : BaseEnemy 
{
	public GameObject m_RagdollPrefab;
	FightStates m_FightState;

	float m_ChargeTimer;
	float m_ChargeBuildUpTime;
	float m_ChargeSpeed;
	bool m_IsCharging;
	Vector3 m_ChargeDirection;
	Vector3 m_ChargeToPosition;

	private enum FightStates
	{
		Default,
		Wobble,
		Charge
	}

	void Start () 
	{
		base.Start ();

		m_FightState = FightStates.Default;

		m_ChargeTimer = 0.0f;
		m_ChargeBuildUpTime = 1.0f;
		m_ChargeSpeed = 20.0f;
		m_IsCharging = false;

	}

	void Update () 
	{
		base.Update ();
	}

	protected void Fight()
	{
		switch(m_FightState)
		{
		case FightStates.Default:
			Default();
			break;
		case FightStates.Wobble:
			Wobble ();
			break;
		case FightStates.Charge:
			Charge ();
			break;
		}
	}

	protected void Die()
	{
		Instantiate (m_RagdollPrefab, transform.position, transform.rotation);
	}

	void Default()
	{

	}

	void Wobble()
	{

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
				//m_ChargeToPosition = m_ChargeDirection.normalized
				m_IsCharging = true;
				m_ChargeTimer = 0.0f;
			}
		}
		else
		{

			transform.position += m_ChargeDirection.normalized * m_ChargeSpeed * Time.deltaTime;
		}
	}
}
