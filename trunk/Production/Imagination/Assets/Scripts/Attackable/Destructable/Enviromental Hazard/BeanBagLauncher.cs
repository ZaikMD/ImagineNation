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

/*Change Log
 * 
 *
 * 
 * 
 */


public enum TimingStates
{
	Idle,
	ReloadToAim,
	Aim,
	Charge,
	Launch,
	Reload,
	Dead
}


public class BeanBagLauncher : Destructable
{
	//for testing.
	public TimingStates m_CurrentState;

	//Change parameters
	public float m_AimTime;
	public float m_ChargeTime;
	public float m_ReloadTime;
	public float m_DeathTime;
	public float m_Range;
	public float m_CrosshairsFlashRate;
	public float m_ProjectileSpeed;
	public float m_ProjectileGravity;
	public float m_ProjectileSpread;
	public float m_BeanBagAmount;

	public float m_LightHeight;
	public Transform m_BulletLaunchLocation;
	public GameObject m_BulletPrefab;
	public Animation m_Anim;

	public AnimationClip m_ChargeAndShoot;
	public AnimationClip m_Death;

	private float m_CurrentAimTime;
	private float m_CurrentChargeTime;
	private float m_CurrentReloadTime;
	private float m_CurrentFlashTime;
	private short m_CurrentTarget;
	private float m_CurrentDeathTime;

	private bool m_HasTraget;
	private bool m_HasAlreadyDead = false;

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
		base.Update();

		DistanceCheck();

		//Checks our current state, and goes to its resperctive update
		switch(m_CurrentState)
		{
			case TimingStates.Idle:
			UpdateIdle();
			break;

			case TimingStates.ReloadToAim:
			ReloadToAim();
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

			case TimingStates.Dead:
			UpdateDead();
			break;
		}
	}

	//Resets all the timer's to there starting time
	void ResetTimers()
	{
		m_CurrentAimTime = m_AimTime;
		m_CurrentChargeTime = m_ChargeTime;
		m_CurrentReloadTime = m_ReloadTime;	
	}

	void UpdateDead()
	{
		TurnOffCrosshairs();

		if(m_CurrentDeathTime < 0)
		{
			Destroy(this.gameObject);
			return;
		}	

		m_CurrentDeathTime -= Time.deltaTime;
	}


	void UpdateIdle()
	{
		//make sure the timers are correct, make sure light is off
		ResetTimers();
		TurnOffCrosshairs();
	}
	
   	void UpdateReload()
	{
		//Checks if we finished our reload phase
		if(m_CurrentReloadTime < 0)
		{
			ResetTimers();
			m_CurrentState = TimingStates.ReloadToAim;
			return;
		}
		//updates timer
		m_CurrentReloadTime -= Time.deltaTime;
	}

	void ReloadToAim()
	{
		//AimAtPlayer();
		UpdateAim();
		TurnOnCrosshairs(); //turn on our crosshairs
		m_CurrentState = TimingStates.Aim;
	}

	void UpdateAim()
	{
		//if we are aiming we have a target
		m_HasTraget = true;
		AimAtPlayer(); //gets our player location and sets it to our launch location
		PaintTarget(); // places the crosshairs on our launch location
		if(m_CurrentAimTime < 0) //Have we finished our timer
		{
			//Reset our timers, we have reloaded, and change states
			ResetTimers();
			m_CurrentState = TimingStates.Charge;
			return;
		}
		m_CurrentAimTime -= Time.deltaTime; // update timer
	}
	
	void UpdateCharge()
	{
		m_Anim.Play("ChargeAndShoot");
		FlashCrosshair(); //Turns the crosshair on and off, based on a timer.
		PaintTarget(); // move the cross hairs to the launch location.
		if(m_CurrentChargeTime < 0) // checks if we are done charging.
		{
			ResetTimers();
			m_CurrentState = TimingStates.Launch;
			return;
		}
		m_CurrentChargeTime -= Time.deltaTime;
	}

	void UpdateLaunch()
	{
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
		for(int i = 0; i < m_BeanBagAmount; i++) 
		{
			GameObject beanBag = (GameObject)Instantiate(m_BulletPrefab); // instatiates a bean bag projectile.

			Vector2 Offset = new Vector2(Random.Range(0.0f, 1.0f) * m_ProjectileSpread, Random.Range(0.0f, 1.0f) * m_ProjectileSpread); // Creates and offset for the projectile

			//calculates the start and final position
			Vector3 StartPosition = new Vector3(m_BulletLaunchLocation.position.x + Offset.x, 
			                                    m_BulletLaunchLocation.position.y, m_BulletLaunchLocation.position.z + Offset.y);
			Vector3 FinalPosition = new Vector3(m_LaunchLocation.x + Offset.x, m_LaunchLocation.y, m_LaunchLocation.z + Offset.y);

			//moves the projectile to its fire point
			beanBag.transform.position = StartPosition;
			beanBag.GetComponent<BeanBag>().SetVelocity(StartPosition, FinalPosition, m_ProjectileSpeed, m_ProjectileGravity); //pass the values for velocity calculation.
		}
	}

	Vector3 GetAimLocation()
	{
		Transform PlayerPosition = GetPlayerLocation(); //Gets the currently targeted plyers location
		RaycastHit hitInfo;
		//Raycast to the ground below the player.
		Physics.Raycast(PlayerPosition.position, -PlayerPosition.up, out hitInfo);
		return hitInfo.point;
	}

	Transform GetPlayerLocation()
	{
		//checks which player we are and returns there transform.
		if (m_CurrentTarget == 1)
		{
			return PlayerInfo.getPlayer(Players.PlayerOne).transform;
		}
		else
		{
			return PlayerInfo.getPlayer(Players.PlayerTwo).transform;
		}
	}

	//a set of logic to determine if we can the next player or target current again.
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

	//places our crosshairs in place.
	void PaintTarget()
	{
		Vector3 LightHeight = new Vector3 (0, m_LightHeight, 0);
		m_CrossHair.transform.position = m_LaunchLocation + LightHeight;
	}

	//preforms a distance check and compares against the size of our collider.
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

	//toggles the crosshairs based off a timer.
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

	//disables the light
	void TurnOffCrosshairs()
	{
		m_CrossHair.GetComponent<Light>().enabled = false;
	}

	//Enables the crosshairs.
	void TurnOnCrosshairs()
	{
		m_CrossHair.GetComponent<Light>().enabled = true;	
		m_CrossHair.transform.LookAt(m_LaunchLocation);
	}

	void DistanceCheck()
	{
		if(m_HasTraget)
		{
			float Distance;

			//Check if our current target is in range,
			if(m_CurrentTarget == 1)
			{
				Distance = Vector3.Distance(transform.position, PlayerInfo.getPlayer(Players.PlayerOne).transform.position);
			}
			else
			{
				Distance = Vector3.Distance(transform.position, PlayerInfo.getPlayer(Players.PlayerTwo).transform.position);
			}

			if(Distance < m_Range)
			{
				return;
			}
			else
			{
				short otherTarget;

				if(m_CurrentTarget == 1)
				{
					Distance = Vector3.Distance(transform.position, PlayerInfo.getPlayer(Players.PlayerTwo).transform.position);
					otherTarget = 2;
				}
				else
				{
					Distance = Vector3.Distance(transform.position, PlayerInfo.getPlayer(Players.PlayerOne).transform.position);
					otherTarget = 1;
				}
				
				if(Distance < m_Range)
				{
					m_CurrentTarget = otherTarget; 
					return;
				}
				else
				{
					m_CurrentState = TimingStates.Idle;
					m_HasTraget = false;
					return;
				}
			}
		}

		float PlayerDistance = Vector3.Distance(PlayerInfo.getPlayer(Players.PlayerOne).transform.position, transform.position);
		if(PlayerDistance < m_Range)
		{
			m_CurrentTarget = 1;
			m_CurrentState = TimingStates.ReloadToAim;
			return;
		}

		PlayerDistance = Vector3.Distance(PlayerInfo.getPlayer(Players.PlayerTwo).transform.position, transform.position);

		if(PlayerDistance < m_Range)
		{
			m_CurrentTarget = 1;
			m_CurrentState = TimingStates.Aim;
			return;
		}

	}
	
	//if no current target, sets the player entering to current target.
	void OnTriggerEnter(Collider other)
	{
		//check if other is player.
		if(other.tag != Constants.PLAYER_STRING)
			return;

		//checks if we already have a target
		if(m_HasTraget)
		{
			return;
		}

		//sets our state to aim
		m_CurrentState = TimingStates.Aim;	

		//initialize a local varible
		Characters OtherCharacter = Characters.Alex;

		//gets which player this is.
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

		//checks if this player one or two
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
		//local varibale.
		Characters OtherCharacter = Characters.Alex;
		short tempCharcater;

		//checks which character we are
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

		//checks which player we are
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

		//if our current target leaves we need to check if the other player is in range. if not we set to idle.
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


	protected override void onDeath()
	{
		if(!m_HasAlreadyDead)
		{
			m_Anim.Play(m_Death.name);
			m_CurrentState = TimingStates.Dead;
			m_HasAlreadyDead = true;
			m_CurrentDeathTime = m_DeathTime;
		}
	}
}

