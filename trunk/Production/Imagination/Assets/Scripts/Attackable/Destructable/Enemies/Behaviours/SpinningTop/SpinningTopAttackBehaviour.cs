/*
 * Created by Joe Burchill November 14/2014
 * Attack Behaviour for the Spin Top, calls
 * Attack Component
 */

#region ChangeLog
/*
 * Added Update functionality to change state - Joe Burchill 2014/11/26
 */
#endregion

using UnityEngine;
using System.Collections;

public class SpinningTopAttackBehaviour : BaseAttackBehaviour, INotifyHit 
{
    new public AnimatorSpinTops EnemyAnimator
    {
        get
        {
            return base.EnemyAnimator as AnimatorSpinTops;
        }
    }

	//Combat States for each of the Spin Top movements
    private enum CombatStates
    {
        Wobble,
        Charge,
        BuildingUpCharge,
        KnockedBack,
        HitByPlayer
    }

	//Variable to store the Combat State
    private CombatStates m_CombatState = CombatStates.BuildingUpCharge;

	//Charge specific movement variable
    public BaseMovement m_ChargeMovement;
	//Building up Charge specific movement variable
    public BaseMovement m_BuildingChargeMovement;
	//KnockedBack specific movement variable
    public BaseMovement m_KnockedBackMovement;
	//Hit by the player specific movement variable
    public BaseMovement m_HitByPlayerMovement;

	//Timer to determine how long the spin top wobbles
    private float m_WobbleTimer;
	//Max amount of time the spin top wobbles
    public float MaxWobbleTime;

	//Timer to determine how long the spin top is knocked back
    private float m_KnockBackTimer;
	//Max amount of time the spin top is knocked back
    public float MaxKnockBackTime;

	//Timer to determine how long the spin top is knocked back when hit by the player
    private float m_HitByPlayerTimer;
	//Max amount of time the spin top is knocked back when hit by the player
    public float MaxTimeAfterHitByPlayer;

	//Timer for when the Spin Top can charge again
    private float m_ChargeTimer;
	//Const for how long it takes for the spin top to build up a charge
    private const float CHARGE_BUILD_UP_TIME = 0.25f;
	//Percentage for how far the spin top charges past the player
    private const float PERCENT_CHARGE_PAST_PLAYER = 1.5f;
	//Flag to determine if the spin top is charging
    private bool m_IsCharging;
	//Flag to determine if the spin top has hit the player
    private bool m_PlayerHit;

	//Const used to determine the distance from the target
	private const float m_ReachedTargetDistance = 2.0f;

    protected override void start()
    {
		//Call all the start functions for each movement component
        m_CombatComponent.start(this);
        m_TargetingComponent.start(this);
        m_MovementComponent.start(this);
        m_ChargeMovement.start(this);
        m_BuildingChargeMovement.start(this);
        m_KnockedBackMovement.start(this);
        m_HitByPlayerMovement.start(this);

		//Initialize variables
        m_CombatState = CombatStates.BuildingUpCharge;
        m_WobbleTimer = MaxWobbleTime;
        m_KnockBackTimer = MaxKnockBackTime;
        m_HitByPlayerTimer = MaxTimeAfterHitByPlayer;
        m_ChargeTimer = 0.0f;
        m_IsCharging = false;

		m_EnemyAI.m_IsInvincible = true;
		m_EnemyAI.addNotifyHit (this);
    }

    public override void update()
    {
		//Set our target each update
        setTarget(Target());
		//Call our Combat functionality
		Combat ();

		//If our target is null then set back to idle
		if (getTarget() == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

		//If the target is within attack range of the spin top then switch to chase
        if (GetDistanceToTarget() >= Constants.SPIN_TOP_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
        }

		//Switch statement to loop through our Combat states
        switch (m_CombatState)
        {
            case CombatStates.Wobble:
                Wobble();
                break;
            case CombatStates.Charge:
                Charge();
                break;
            case CombatStates.BuildingUpCharge:
                BuildingUpCharge();
                break;
            case CombatStates.KnockedBack:
                KnockedBack();
                break;
            case CombatStates.HitByPlayer:
                HitByPlayer();
                break;
            default:
                break;
        }
    }

    private void Wobble()
	{
        EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.Wobble);

		//Set our spin top to not be invincible when wobbling
		m_EnemyAI.m_IsInvincible = false;

		//Call the wobble movement
        Movement();

        if (m_WobbleTimer > 0.0f)
        {
			//aslong as the timer is greater than 0, decrement the timer
            m_WobbleTimer -= Time.deltaTime;
        }
        else
		{
			//If the timer is up then switch to BuildingUpCharge
            m_CombatState = CombatStates.BuildingUpCharge;
            EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.ExitWobble);
			//Reset the timer
            m_WobbleTimer = MaxWobbleTime;
        }
    }

    private void Charge()
    {
		m_EnemyAI.m_IsInvincible = true;
        //EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.Attack);
		//Call charge movement
        Movement();

        if (!m_PlayerHit)
        {
			//If the player hasn't been hit and we have reached our target behind the player
			if(GetDistanceToTarget() < m_ReachedTargetDistance)
			{
				//Set to wobble
            	m_CombatState = CombatStates.Wobble;
			}
        }
        else
        {
			//If we have hit the player then get knockedback
            m_CombatState = CombatStates.KnockedBack;
        }
    }

    private void BuildingUpCharge()
    {
		m_EnemyAI.m_IsInvincible = true;
		//Check charge timer
        if (m_ChargeTimer < CHARGE_BUILD_UP_TIME)
        {
			//Increment charge Timer as long as it is less than our build up time
            m_ChargeTimer += Time.deltaTime;
        }
        else
        {
			//Call build up movement
            Movement();
			//Reset timer
            m_ChargeTimer = 0.0f;
			//Switch to the charge state
            m_CombatState = CombatStates.Charge;
        }
    }

    private void KnockedBack()
    {
		m_EnemyAI.m_IsInvincible = false;

		//If the knockback timer is greater than 0
        if (m_KnockBackTimer > 0.0f)
        {
			//Decrement timer and call the funcion for knockback
            m_KnockBackTimer -= Time.deltaTime;
            Movement();
        }
        else
        {
			//Reset knockback timer
            m_KnockBackTimer = MaxKnockBackTime;
			//Set to building charge
            m_CombatState = CombatStates.BuildingUpCharge;
        }
    }

    private void HitByPlayer()
    {
		//Check timer
        if (m_HitByPlayerTimer > 0.0f)
        {
			//Decrement and call movement function
            m_HitByPlayerTimer -= Time.deltaTime;
            Movement();
        }
        else
        {
			//Reset Timer
            m_HitByPlayerTimer = MaxTimeAfterHitByPlayer;
			//Set to building up charge
            m_CombatState = CombatStates.BuildingUpCharge;
            EnemyAnimator.playAnimation(AnimatorSpinTops.Animations.ExitWobble);
        }
    }

    protected override void Movement()
    {
		//Check if we are updating movement
        if (m_EnemyAI.m_UMovement)
        {
			//Switch through combat state calling movement functionality for each
            switch (m_CombatState)
            {
                case CombatStates.Wobble:
                    if (m_MovementComponent != null)
                    {
					m_MovementComponent.Movement(getTarget());
                    }
                break;

                case CombatStates.Charge:
                    if (m_ChargeMovement != null)
                    {
					m_ChargeMovement.Movement(getTarget());
                    }
                break;

                case CombatStates.BuildingUpCharge:
                    if (m_BuildingChargeMovement != null)
                    {
					m_BuildingChargeMovement.Movement(getTarget());
                    }
                break;

                case CombatStates.KnockedBack:
                    if (m_KnockedBackMovement != null)
                    {
					m_KnockedBackMovement.Movement(getTarget());
                    }
                break;

                case CombatStates.HitByPlayer:
                    if (m_HitByPlayerMovement != null)
                    {
					m_HitByPlayerMovement.Movement(getTarget());
                    }
                break;
            }
        }
    }

	//Returns distance between target and enemy
    private float GetDistanceToTarget()
    {
		return Vector3.Distance(transform.position, getTarget().transform.position);
    }

	//Check if it has run into the player in any state other than wobble
    void OnTriggerEnter(Collider other)
    {
        //If the player enters the trigger set player hit to true
        if (m_CombatState != CombatStates.Wobble)
        {
            if (other.tag == Constants.PLAYER_STRING)
            {
				//Set flag to true
                m_PlayerHit = true;
            }
        }
    }

	//Check if the player has run away from the spin top and exited the trigger
    void OnTriggerExit(Collider other)
    {
        if (other.tag == Constants.PLAYER_STRING)
        {
			//Set flag to flase
            m_PlayerHit = false;
        }
	}

	public void NotifyHit()
	{
		//Call Hit ByPlayer movement
		m_HitByPlayerMovement.Movement(getTarget());
		//Switch to the Hit player state
		m_CombatState = CombatStates.HitByPlayer;
	}

	public override void ComponentInfo (out string[] names, out BaseComponent[] components)
	{
		names = new string[7];
		components = new BaseComponent[7];
		
		names [0] = Constants.COMBAT_STRING;
		components [0] = m_CombatComponent;
		
		names [1] = Constants.TARGETING_STRING;
		components [1] = m_TargetingComponent;
		
		names [2] = Constants.MOVEMENT_STRING;
		components [2] = m_MovementComponent;
		names [3] = Constants.CHARGE_MOVEMENT_STRING;
		components [3] = m_ChargeMovement;
		names [4] = Constants.BUILDING_CHARGE_STRING;
		components [4] = m_BuildingChargeMovement;
		names [5] = Constants.KNOCKED_BACK_MOVEMENT_STRING;
		components [5] = m_KnockedBackMovement;
		names [6] = Constants.HIT_BY_PLAYER_MOVEMENT_STRING;
		components [6] = m_HitByPlayerMovement;
	}

	public override int numbComponents ()
	{
		return 7;
	}
	
	public override void SetComponents (string[] components)
	{
		m_ComponentsObject = transform.FindChild (Constants.COMPONENTS_STRING).gameObject;
		
		m_CombatComponent = m_ComponentsObject.GetComponent (components [0]) as BaseCombat;
		m_TargetingComponent = m_ComponentsObject.GetComponent (components [1]) as BaseTargeting;

		m_MovementComponent = m_ComponentsObject.GetComponent (components [2]) as BaseMovement;
		m_ChargeMovement = m_ComponentsObject.GetComponent (components [3]) as BaseMovement;
		m_BuildingChargeMovement = m_ComponentsObject.GetComponent (components [4]) as BaseMovement;
		m_KnockedBackMovement = m_ComponentsObject.GetComponent (components [5]) as BaseMovement;
		m_HitByPlayerMovement = m_ComponentsObject.GetComponent (components [6]) as BaseMovement;
	}
}
