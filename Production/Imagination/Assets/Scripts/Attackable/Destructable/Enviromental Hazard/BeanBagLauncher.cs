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

	//Change parameters
	public float m_AimTime;
	public float m_ChargeTime;
	public float m_ReloadTime;
	public float m_CrosshairsFlashRate;
	public float m_AirTimeMultiplier;
	public float m_ProjectileSpeed;
	public float m_ProjectileSpread;
	public float BeanBagAmount;
	
	public Transform m_BulletLaunchLocation;
	public GameObject m_BulletPrefab;

	private float m_CurrentAimTime;
	private float m_CurrentChargeTime;
	private float m_CurrentReloadTime;
	private float m_CurrentFlashTime;
	private short m_CurrentTarget;

	private bool m_HasTraget;

	private Vector3 m_LaunchLocation;
	public GameObject m_CrossHair;

	// Use this for initialization
	void Start () 
	{
		ResetTimers();

		m_HasTraget = false;
		m_CurrentState = TimingStates.Idle;
		m_LaunchLocation = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch(m_CurrentState)
		{
			case TimingStates.Idle:
			UpdateIdle();
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

	void UpdateIdle()
	{
		ResetTimers();
		TurnOffCrosshairs();
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
		m_HasTraget = true;
		TurnOnCrosshairs();
		AimAtPlayer();
		PaintTarget();
		if(m_CurrentAimTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.Charge;
			return;
		}
		m_CurrentAimTime -= Time.deltaTime;
	}
	
	void UpdateCharge()
	{
		FlashCrosshair();
		PaintTarget();
		if(m_CurrentChargeTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.Launch;
			return;
		}
		m_CurrentChargeTime -= Time.deltaTime;
	}

	void UpdateLaunch()
	{
		//TODO: fire projectile at Saved loction.
		LaunchProjectile();
		TurnOffCrosshairs();
		GetNextTarget();
		m_CurrentState = TimingStates.Reload;	
	}

	void AimAtPlayer()
	{
		m_LaunchLocation = GetAimLocation();
	}

	void LaunchProjectile()
	{
		//Spawn new launch projectile.

		for(int i = 0; i < BeanBagAmount; i++)
		{
			GameObject beanBag = (GameObject)Instantiate(m_BulletPrefab);

			Vector2 Offset = new Vector2(Random.Range(0.0f, 1.0f) * m_ProjectileSpread, Random.Range(0.0f, 1.0f) * m_ProjectileSpread);

			Vector3 StartPosition = new Vector3(m_BulletLaunchLocation.position.x + Offset.x, 
			                                    m_BulletLaunchLocation.position.y, m_BulletLaunchLocation.position.z + Offset.y);
			Vector3 FinalPosition = new Vector3(m_LaunchLocation.x + Offset.x, m_LaunchLocation.y, m_LaunchLocation.z + Offset.y);

			beanBag.transform.position = StartPosition;
			beanBag.GetComponent<BeanBag>().SetVelocity(StartPosition, FinalPosition, m_ProjectileSpeed);
		}
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
		if (m_CurrentTarget == 1)
		{
			return PlayerInfo.getPlayer(Players.PlayerOne).transform;
		}
		else
		{
			return PlayerInfo.getPlayer(Players.PlayerTwo).transform;
		}
	}

	void GetNextTarget()
	{
		if(m_CurrentTarget == 1)
		{
			if(WithinRange(2))
			{
				m_CurrentTarget = 2;
				return;
			}
			m_CurrentTarget = 1;
			return;
		}
		else
		{
			if(WithinRange(1))
			{
				m_CurrentTarget = 1;
				return;
			}
			m_CurrentTarget = 2;
			return;
		}
	}

	void PaintTarget()
	{
		Vector3 LightHeight = new Vector3 (0, 2.0f, 0);
		m_CrossHair.transform.position = m_LaunchLocation + LightHeight;
		m_CrossHair.transform.LookAt(m_LaunchLocation);
	}


	bool WithinRange(short currentTarget)
	{
		float Range = this.GetComponent<CapsuleCollider>().radius;
		float Distance;

		if(currentTarget == 1)
		{
			Distance = Vector3.Distance(this.transform.position, PlayerInfo.getPlayer(Players.PlayerOne).transform.position);
		}
		else
		{
			Distance = Vector3.Distance(this.transform.position, PlayerInfo.getPlayer(Players.PlayerTwo).transform.position);
		}

		return Range > Distance;
	}

	void FlashCrosshair()
	{
		if(m_CurrentFlashTime < 0)
		{
			m_CurrentFlashTime = m_CrosshairsFlashRate;

			if(m_CrossHair.GetComponent<Light>().enabled)
			{
				TurnOffCrosshairs();
			}
			else
			{
				TurnOnCrosshairs();
			}
		}
		m_CurrentFlashTime -= Time.deltaTime;	
	}

	void TurnOffCrosshairs()
	{
		m_CrossHair.GetComponent<Light>().enabled = false;
	}

	void TurnOnCrosshairs()
	{
		m_CrossHair.GetComponent<Light>().enabled = true;	
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

		switch(other.name)
		{
			case Constants.ALEX_WITH_MOVEMENT_STRING:
			OtherCharacter = Characters.Alex;
			break;

			case Constants.DEREK_WITH_MOVEMENT_STRING:
			OtherCharacter = Characters.Derek;
			break;

			case Constants.ZOE_WITH_MOVEMENT_STRING:
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
		Characters OtherCharacter = Characters.Alex;
		short tempCharcater;

		switch(other.name)
		{
		case Constants.ALEX_WITH_MOVEMENT_STRING:
			OtherCharacter = Characters.Alex;
			break;
			
		case Constants.DEREK_WITH_MOVEMENT_STRING:
			OtherCharacter = Characters.Derek;
			break;
			
		case Constants.ZOE_WITH_MOVEMENT_STRING:
			OtherCharacter = Characters.Zoe;
			break;
		}
		
		if(OtherCharacter == GameData.Instance.PlayerOneCharacter)
		{
			tempCharcater = 1;
		}
		else
		{
			tempCharcater = 2;
		}

		//check if the player leaving is the current target, if not, we don't need to do anything.
		if(tempCharcater != m_CurrentTarget)
		{
			return;
		}



		if(m_CurrentTarget == 1)
		{
			if(WithinRange(2))
			{
				m_CurrentTarget = 2;
			}
			else
			{
				m_CurrentState = TimingStates.Idle;
				m_HasTraget = false;
			}
		}
		else
		{
			if(WithinRange(1))
			{
				m_CurrentTarget = 1;
			}
			else
			{
				m_CurrentState = TimingStates.Idle;
				m_HasTraget = false;
			}
		}

	}
}
