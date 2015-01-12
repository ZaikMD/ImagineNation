using UnityEngine;
using System.Collections;

public enum TimingStates
{
	Idle,
	Aim,
	Charge,
	Launch,
	Reload
}


public class BeanBagLauncher : MonoBehaviour 
{
	//for testing.
	public TimingStates m_CurrentState;
	public float m_CurrentTimerRemaining;

	//Change parameters
	public float m_AimTime;
	public float m_ChargeTime;
	public float m_ReloadTime;
	public float m_AirTimeMultiplier;

	private float m_CurrentAimTime;
	private float m_CurrentChargeTime;
	private float m_CurrentReloadTime;

	// Use this for initialization
	void Start () 
	{
		m_CurrentState = TimingStates.Idle;
	}
	
	// Update is called once per frame
	void Update ()
	{

		switch(m_CurrentState)
		{
			case TimingStates.Idle:
			//Do Nothing
			break;

			case TimingStates.Aim:
			UpdateAim();
			break;

			case TimingStates.Charge:
			UpdateCharge();
			break;

			case TimingStates.Launch:
			UpdateLaunch();
			break;

			case TimingStates.Reload:
			UpdateReload();
			break;
		}
	}

	void ResetTimers()
	{
		m_CurrentAimTime = m_AimTime;
		m_CurrentChargeTime = m_ChargeTime;
		m_CurrentReloadTime = m_ReloadTime;	
	}

	/// <summary>
	/// this Class will update a timer and change states upon completion. 
	/// </summary>
   	void UpdateReload()
	{
		if(m_CurrentReloadTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.Aim;
			return;
		}
		m_CurrentReloadTime -= Time.deltaTime;
	}

	void UpdateAim()
	{
		//TODO: draw target at player position.

		if(m_CurrentReloadTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.Charge;
			return;
		}
		m_CurrentAimTime -= Time.deltaTime;
	}
	
	void UpdateCharge()
	{
		//TODO: draw target at saved location

		if(m_CurrentReloadTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.Aim;
			return;
		}
		m_CurrentChargeTime -= Time.deltaTime;
	}

	void UpdateLaunch()
	{
		//TODO: fire projectile at loction.
		m_CurrentState = TimingStates.Reload;	
	}

	void OnTriggerEnter(Collider other)
	{
		m_CurrentState = TimingStates.Aim;	
	}

	void OnTriggerExit(Collider other)
	{
		m_CurrentState = TimingStates.Idle;
	}
}
