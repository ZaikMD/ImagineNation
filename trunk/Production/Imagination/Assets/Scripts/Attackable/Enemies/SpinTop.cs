/// 
/// Spin top.
/// Created by: Matthew Whitlaw
/// 
/// This script inherits from base enemy and builds upon it
/// to specifically be a Spin Top. It primarily handles the fight
/// state for the spin top which has it's own state machine.
/// States include:
/// 
/// BuildingUpCharge - the state which pauses the spin top and when
/// ready, determines where the spin top will move to.
/// 
/// Charge - determines whether a player was hit or the destination was
/// reached and changes the state to knockedback or wobble accordingly.
/// 
/// Wobble - if the spin top missed the player then it pauses for a certain
/// amount of time allowing the player to inflict damage
/// 
/// Knockedback - occurs when the top hits the player, applies a knockback to the
/// spin top
/// 
/// HitByPlayer - occurs when the Spin Top is successfully hit by player,
/// applies a knockback
/// 
/// This script also handles the OnTriggerEnter and OnTriggerExit with the player.

#region Change Log
#endregion

using UnityEngine;
using System.Collections;

public class SpinTop : BaseEnemy 
{
	public GameObject m_RagdollPrefab;
	FightStates m_FightState;
	float m_NormalSpeed;
	float m_KnockBackMultiplier;

	//Timers
	float m_WobbleTimer;
	public float m_MaxWobbleTime;

	float m_KnockBackTimer;
	public float m_MaxKnockBackTime;

	float m_HitByPlayerTimer;
	public float m_MaxTimeAfterHitByPlayer;

	public float m_ChargeSpeed;

	//Charging Variables
	float m_ChargeTimer;
	float m_ChargeBuildUpTime;
	bool m_IsCharging;
	Vector3 m_ChargeDirection;
	Vector3 m_ChargeToPosition;
	float m_ChargeDistance;
	float m_PercentToChargePastPlayer;

	//Distances
	float m_DistanceToTarget;
	float m_DistanceToPlayer;
	float m_DistanceFromChargeToPosition;

	//PlayerHit Variables
	bool m_PlayerHit;
	Vector3 m_DirectionHitByPlayer;

	//The enum of fightstates for the enemy fight state
	private enum FightStates
	{
		Wobble,
		Charge,
		BuildingUpCharge,
		KnockedBack,
		HitByPlayer
	}

	void Start () 
	{	
		//Call base enemy's start and intialize all variables specific to Spin Top
		base.Start ();

		//General Variable Values
		m_FightState = FightStates.BuildingUpCharge;
		m_NormalSpeed = m_Agent.speed;
		m_KnockBackMultiplier = 5.0f;
		m_AggroRange = 10.0f;
		m_CombatRange = m_AggroRange;

		//Default Timers
		m_MaxWobbleTime = 2.0f;
		m_WobbleTimer = m_MaxWobbleTime;

		m_MaxKnockBackTime = 1.0f;
		m_KnockBackTimer = m_MaxKnockBackTime;

		m_MaxTimeAfterHitByPlayer = 1.0f;
		m_HitByPlayerTimer = m_MaxTimeAfterHitByPlayer;

		//Charge Values
		m_ChargeTimer = 0.0f;
		m_ChargeBuildUpTime = 1.0f;
		m_ChargeSpeed = 20.0f;
		m_IsCharging = false;
		m_PercentToChargePastPlayer = 1.5f;

		m_PlayerHit = false;

	}

	void Update () 
	{
		//Call Base Enemy's update function
		base.Update ();
	}

	protected override void Fight()
	{
		UpdateFightState ();
	}

	void UpdateFightState ()
	{
		//Call the current fight states function
		switch(m_FightState)
		{
		case FightStates.Wobble:
			Wobble ();
			break;
		case FightStates.Charge:
			Charge ();
			break;
		case FightStates.BuildingUpCharge:
			BuildingUpCharge ();
			break;
		case FightStates.KnockedBack:
			KnockedBack ();
			break;
		case FightStates.HitByPlayer:
			HitByPlayer();
			break;
		default:
			break;
		}
	}

	protected override void Die()
	{
		//Instantiate (m_RagdollPrefab, transform.position, transform.rotation);
	}

	void Wobble()
	{
		//Count down the wobble timer
		if(m_WobbleTimer > 0.0f)
		{
			m_WobbleTimer -= Time.deltaTime;
		}
		else
		{
			//If the player is out of combat range change to the default state
			m_DistanceToPlayer = Vector3.Distance(m_Target.position, transform.position);
			if(m_DistanceToPlayer > m_CombatRange)
			{
				m_State = State.Default;
				return;
			}

			//Otherwise build up charge again and reset wobble timer
			m_FightState = FightStates.BuildingUpCharge;
			m_WobbleTimer = m_MaxWobbleTime;
		}
	}

	void Charge()
	{
		//Debug.DrawRay(m_ChargeToPosition, transform.position - m_ChargeToPosition, Color.magenta);

		//Set a custom charge speed and check how far from the ChargeToPosition
		m_Agent.speed = m_ChargeSpeed;
		m_DistanceFromChargeToPosition = Vector3.Distance (m_ChargeToPosition, transform.position);

		//If the player wasn't hit
		if(!m_PlayerHit)
		{
			if(m_DistanceFromChargeToPosition < 3.0f)
			{
				//Enter wobble state and reset the agent speed
				m_FightState = FightStates.Wobble;
				m_Agent.speed = m_NormalSpeed;

			}
		}
		else
		{
			//If the player was hit then enter knocked back and reset the agent speed
			m_FightState = FightStates.KnockedBack;
			m_Agent.speed = m_NormalSpeed;
		}
	}

	void BuildingUpCharge()
	{
		//If still building up
		if(m_ChargeTimer < m_ChargeBuildUpTime)
		{
			//Increase timer and ensure that the destination is itself
			m_ChargeTimer += Time.deltaTime;
			m_Agent.SetDestination(transform.position);
		}
		else
		{
			//Get the player position and your position
			Vector3 currentPosition = transform.position;
			Vector3 destinationPosition = m_Target.transform.position;

			//Get the direction vector between then and zero out the y axis
			m_ChargeDirection = destinationPosition - currentPosition;
			m_ChargeDirection.y = 0.0f;

			//Get the distance between the two
			m_DistanceToTarget = m_ChargeDirection.magnitude;

			//Get a distance just passed the distance to the player
			m_ChargeDistance = m_DistanceToTarget * m_PercentToChargePastPlayer;

			//Determine a specific position just passed the player
			m_ChargeToPosition = currentPosition + m_ChargeDirection.normalized * m_ChargeDistance;

			//Set new destination
			m_Agent.SetDestination(m_ChargeToPosition);

			//reset timer and go into Charge state
			m_ChargeTimer = 0.0f;
			m_FightState = FightStates.Charge;

		}
	}

	void KnockedBack()
	{
		//Set the destination to the spin top itself
		m_Agent.SetDestination (transform.position);

		//Check the knockback timer
		if(m_KnockBackTimer > 0.0f)
		{
			//If Move in the opposite direction that the player was collided with
			m_KnockBackTimer -= Time.deltaTime;
			transform.position -= m_ChargeDirection.normalized * Time.deltaTime * m_KnockBackMultiplier;
		}
		else
		{
			//If knockback is over then reset the timer and go into building up charge
			m_KnockBackTimer = m_MaxKnockBackTime;
			m_FightState = FightStates.BuildingUpCharge;
		}
	}

	void HitByPlayer()
	{
		//Set the destination to the spin top itself
		m_Agent.SetDestination (transform.position);
		
		//Check the timer
		if(m_HitByPlayerTimer > 0.0f)
		{
			//If Move in the opposite direction that the player's projectile came from.
			m_HitByPlayerTimer -= Time.deltaTime;
			transform.position -= m_DirectionHitByPlayer.normalized * Time.deltaTime * m_KnockBackMultiplier;
		}
		else
		{
			//If the timer is over then reset the timer and go into building up charge
			m_HitByPlayerTimer = m_MaxTimeAfterHitByPlayer;
			m_FightState = FightStates.BuildingUpCharge;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//If the player enters the trigger set player hit to true and get their destructible component 
		//to be able to call their onhit function and apply damage.
		if(other.tag == Constants.PLAYER_STRING)
		{
			m_PlayerHit = true;
			Destructable destructableObj = (Destructable)other.GetComponentInChildren<Destructable> ();
			if(destructableObj != null)
			{
				destructableObj.onHit(new EnemyProjectile());
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		//Reset player hit if the player exits the trigger
		if(other.tag == Constants.PLAYER_STRING)
		{
			m_PlayerHit = false;
		}
	}

	public override void onHit(PlayerProjectile proj)
	{
		if(m_FightState == FightStates.Wobble)
		{
			//If the spin top is in the wobble state call on hit and set the state to HitByPlayer
			base.onHit (proj);
			m_DirectionHitByPlayer = m_Target.position - transform.position;
			m_FightState = FightStates.HitByPlayer;
		}
	}

}
