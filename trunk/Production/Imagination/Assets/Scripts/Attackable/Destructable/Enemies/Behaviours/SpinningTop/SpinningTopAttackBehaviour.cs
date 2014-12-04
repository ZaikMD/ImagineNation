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
    private enum CombatStates
    {
        Wobble,
        Charge,
        BuildingUpCharge,
        KnockedBack,
        HitByPlayer
    }

    private CombatStates m_CombatState = CombatStates.BuildingUpCharge;

    public BaseMovement m_WobbleMovement;
    public BaseMovement m_ChargeMovement;
    public BaseMovement m_BuildingChargeMovement;
    public BaseMovement m_KnockedBackMovement;
    public BaseMovement m_HitByPlayerMovement;

    private float m_WobbleTimer;
    public float MaxWobbleTime;

    private float m_KnockBackTimer;
    public float MaxKnockBackTime;

    private float m_HitByPlayerTimer;
    public float MaxTimeAfterHitByPlayer;

    private float m_ChargeTimer;
    private const float CHARGE_BUILD_UP_TIME = 0.25f;
    private const float PERCENT_CHARGE_PAST_PLAYER = 1.5f;
    private bool m_IsCharging;

    private bool m_PlayerHit;

	private float m_ReachedTargetDistance;

    protected override void start()
    {
        m_CombatComponent.start(this);
        m_TargetingComponent.start(this);
        m_MovementComponent.start(this);
        m_WobbleMovement.start(this);
        m_ChargeMovement.start(this);
        m_BuildingChargeMovement.start(this);
        m_KnockedBackMovement.start(this);
        m_HitByPlayerMovement.start(this);

        m_CombatState = CombatStates.BuildingUpCharge;
        m_WobbleTimer = MaxWobbleTime;
        m_KnockBackTimer = MaxKnockBackTime;
        m_HitByPlayerTimer = MaxTimeAfterHitByPlayer;
        m_ChargeTimer = 0.0f;
        m_IsCharging = false;
		m_ReachedTargetDistance = 2.0f;

		m_EnemyAI.m_IsInvincible = true;
		m_EnemyAI.addNotifyHit (this);
    }

    public override void update()
    {
        m_Target = Target();

		Combat ();

        if (m_Target == null)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Idle);
            return;
        }

        if (GetDistanceToTarget() >= Constants.SPIN_TOP_ATTACK_RANGE)
        {
            m_EnemyAI.SetState(EnemyAI.EnemyState.Chase);
        }

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
		m_EnemyAI.m_IsInvincible = false;

        Movement();

        if (m_WobbleTimer > 0.0f)
        {
            m_WobbleTimer -= Time.deltaTime;
        }
        else
        {
            m_CombatState = CombatStates.BuildingUpCharge;
            m_WobbleTimer = MaxWobbleTime;
        }
    }

    private void Charge()
    {
        Movement();

        if (!m_PlayerHit)
        {
			if(GetDistanceToTarget() < m_ReachedTargetDistance)
			{
            	m_CombatState = CombatStates.Wobble;
			}
        }
        else
        {
            m_CombatState = CombatStates.KnockedBack;
        }
    }

    private void BuildingUpCharge()
    {
        if (m_ChargeTimer < CHARGE_BUILD_UP_TIME)
        {
			m_BuildingChargeMovement.Movement(transform.position);
            m_ChargeTimer += Time.deltaTime;
        }
        else
        {
            Movement();
            m_ChargeTimer = 0.0f;
            m_CombatState = CombatStates.Charge;
        }
    }

    private void KnockedBack()
    {
        if (m_KnockBackTimer > 0.0f)
        {
            m_KnockBackTimer -= Time.deltaTime;
            Movement();
        }
        else
        {
            m_KnockBackTimer = MaxKnockBackTime;
            m_CombatState = CombatStates.BuildingUpCharge;
        }
    }

    private void HitByPlayer()
    {
		m_HitByPlayerMovement.Movement (transform.position);

        if (m_HitByPlayerTimer > 0.0f)
        {
            m_HitByPlayerTimer -= Time.deltaTime;
            Movement();
        }
        else
        {
            m_HitByPlayerTimer = MaxTimeAfterHitByPlayer;
            m_CombatState = CombatStates.BuildingUpCharge;
        }
    }

    protected override void Movement()
    {
        if (m_EnemyAI.m_UMovement)
        {
            switch (m_CombatState)
            {
                case CombatStates.Wobble:
                    if (m_WobbleMovement != null)
                    {
                        m_WobbleMovement.Movement(m_Target);
                    }
                break;

                case CombatStates.Charge:
                    if (m_ChargeMovement != null)
                    {
                        m_ChargeMovement.Movement(m_Target);
                    }
                break;

                case CombatStates.BuildingUpCharge:
                    if (m_BuildingChargeMovement != null)
                    {
                        m_BuildingChargeMovement.Movement(m_Target);
                    }
                break;

                case CombatStates.KnockedBack:
                    if (m_KnockedBackMovement != null)
                    {
                        m_KnockedBackMovement.Movement(m_Target);
                    }
                break;

                case CombatStates.HitByPlayer:
                    if (m_HitByPlayerMovement != null)
                    {
                        m_HitByPlayerMovement.Movement(m_Target);
                    }
                break;
            }
        }
    }

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        //If the player enters the trigger set player hit to true
        if (m_CombatState != CombatStates.Wobble)
        {
            if (other.tag == Constants.PLAYER_STRING)
            {
                m_PlayerHit = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Reset player hit if the player exits the trigger
        if (other.tag == Constants.PLAYER_STRING)
        {
            m_PlayerHit = false;
        }
	}

	public void NotifyHit()
	{
		m_CombatState = CombatStates.HitByPlayer;
	}
}
