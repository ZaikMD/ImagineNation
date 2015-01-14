using UnityEngine;
using System.Collections;


/* Created by: Kole Tackney
 * Date: 12, 01, 2014	 
 * 
 * this class will hold the logic for the bean bag launcher,
 * the bean bag launcher logic will mostly be timer based, switching between 
 * each character and taking a single shot at them.  
 * 
 * the bean bag launcher will paint a target below the targeted player,
 * the launcher will then "Charge" its attack, the aimer will stay 
 * at the players loction when the charge began. the bean bag will then launch
 * at the charge loction. then the launcher will reload.
 */


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

	public GameObject m_CrossHairsPrefab;

	private float m_CurrentAimTime;
	private float m_CurrentChargeTime;
	private float m_CurrentReloadTime;
	private short m_CurrentTarget;

	private bool m_HasTraget;

	private Vector3 m_LaunchLocation;
	private GameObject m_CrossHair;


	// Use this for initialization
	void Start () 
	{
		m_HasTraget = false;
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
		AimAtPlayer ();

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
		//TODO: fire projectile at Saved loction.

		DeleteCurrentCrosshairs();
		GetNextTarget ();
		m_CurrentState = TimingStates.Reload;	
	}

	void AimAtPlayer()
	{
		if(m_CrossHair == null)
		{
			m_CrossHair = (GameObject)Instantiate(m_CrossHairsPrefab);
		}

		m_CrossHair.transform.position = GetAimLocation();	

		m_LaunchLocation = m_CrossHair.transform.position;
	}

	void AimAtSavedLocation()
	{

	
	}

	Vector3 GetAimLocation()
	{
		Transform PlayerPosition = GetPlayerLocation();
		RaycastHit hitInfo;

		Physics.Raycast(PlayerPosition.position, -PlayerPosition.up, out hitInfo);
		return hitInfo.point;
	}

	Transform GetPlayerLocation()
	{
		GameObject currentPlayer = new GameObject();

		if(m_CurrentTarget == 1)
		{
			switch(GameData.Instance.PlayerOneCharacter)
			{
				case Characters.Alex:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
				break;

				case Characters.Derek:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
				break;

				case Characters.Zoe:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
				break;
			}
		}
		else
		{
			switch(GameData.Instance.PlayerTwoCharacter)
			{
				case Characters.Alex:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
				break;
				
				case Characters.Derek:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
				break;
				
				case Characters.Zoe:
				currentPlayer = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
				break;
			}
		}

		return currentPlayer.transform;
	}

	void GetNextTarget()
	{
		GameObject OtherPlayer = new GameObject();

		if(m_CurrentTarget == 1)
		{
			switch(GameData.Instance.PlayerTwoCharacter)
			{
				case Characters.Alex:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
				break;

				case Characters.Derek:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
				break;

				case Characters.Zoe:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
				break;
			}
		}
		else
		{
			switch(GameData.Instance.PlayerOneCharacter)
			{
				case Characters.Alex:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.ALEX_STRING);
				break;
				
				case Characters.Derek:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.DEREK_STRING);
				break;
				
				case Characters.Zoe:
				OtherPlayer = GameObject.FindGameObjectWithTag(Constants.ZOE_STRING);
				break;
			}
		}


		float Distance = Vector3.Distance(this.transform.position, OtherPlayer.transform.position);
		float Range = this.GetComponent<CapsuleCollider>().radius;

		//They are out of range.
		if(Range < Distance)
		{
			return;
		}

		if (m_CurrentTarget == 1) 
		{
			m_CurrentTarget = 2; 		
		}
		else
		{
			m_CurrentTarget = 1;
		}
	}

	void DeleteCurrentCrosshairs()
	{
		if(m_CrossHair == null)
		{
			return;
		}

		Destroy(m_CrossHair);
		m_CrossHair = null;
	}

	void OnTriggerEnter(Collider other)
	{
		//check if other is player.
		if(other.tag != Constants.PLAYER_STRING)
			return;


		if(m_HasTraget)
		{
			return;
		}

		m_CurrentState = TimingStates.Aim;	

		Characters OtherCharacter = Characters.Alex;

		switch(other.tag)
		{
			case Constants.ALEX_STRING:
			OtherCharacter = Characters.Alex;
			break;

			case Constants.DEREK_STRING:
			OtherCharacter = Characters.Derek;
			break;

			case Constants.ZOE_STRING:
			OtherCharacter = Characters.Zoe;
			break;
		}

		if(OtherCharacter == GameData.Instance.PlayerOneCharacter)
		{
			m_CurrentTarget = 1;
		}
		else
		{
			m_CurrentTarget = 2;
		}
	}

	void OnTriggerExit(Collider other)
	{
		m_CurrentState = TimingStates.Idle;
	}
}
