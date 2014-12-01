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

public class SpinningTopAttackBehaviour : BaseAttackBehaviour 
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

    private bool m_PlayerHit;

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
    }

    public override void update()
    {
        m_Target = Target();

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

    }

    private void Charge()
    {

    }

    private void BuildingUpCharge()
    {

    }

    private void KnockedBack()
    {

    }

    private void HitByPlayer()
    {

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
}
