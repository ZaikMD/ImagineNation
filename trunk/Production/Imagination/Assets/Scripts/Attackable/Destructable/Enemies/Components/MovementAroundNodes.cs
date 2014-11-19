/*
 * Created by Joe Burchill Novemeber 15/2014
 * 
 * Created to use for Enemy AI movement around
 * patrol nodes when out of combat
 */
#region ChangeLog
/* 
 *
 */
#endregion

using UnityEngine;
using System.Collections;

public class MovementAroundNodes : BaseMovement 
{
    //Patrol Node Variables
    public Transform[] PatrolNodes;
    private int m_NodeCount;
    private int m_CurrentPatrolNode = 0;

    //Target for the enemy to move to
    private Transform m_Target;

    private NavMeshAgent m_Agent;

    //Min and max patrol speed for randomization
    private const float MIN_PATROL_SPEED = 3.0f;
    private const float MAX_PATROL_SPEED = 5.0f;

    //Distance value for when the enemy reaches a patrol node
    private const float REACHED_NODE_DISTANCE = 2.0f;

    public override void start(BaseBehaviour baseBehaviour)
    {
 	    base.start(baseBehaviour);
    }


    public override void Movement()
    {
        //Set stopping Distance to reach node without stopping short
        m_Agent.stoppingDistance = 0;

        //Set our target to the transform of the path node array
        m_Target = PatrolNodes[m_CurrentPatrolNode].transform;

        //Check if we have reached our current target node
        if (GetDistanceToTarget() <= REACHED_NODE_DISTANCE)
        {
            //If so we increment our current path node
            m_CurrentPatrolNode++;

            //Randomize enemy speed between the 2 constants after reaching each node, organic feel
            float randomSpeed = Random.Range(MIN_PATROL_SPEED, MAX_PATROL_SPEED);
            m_Agent.speed = randomSpeed;


            //Check if we have reached our node count, if so we reset the current
            //path node and set our target again
            if (m_CurrentPatrolNode >= m_NodeCount)
            {
                m_CurrentPatrolNode = 0;
                m_Target = PatrolNodes[m_CurrentPatrolNode].transform;
            }

        }
        else
        {
            //If we haven't reached our target node then we check we have a target
            //and set the agent destination
            if (m_Target != null)
            {
                m_Agent.SetDestination(m_Target.position);
            }
        }
    }

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, m_Target.transform.position);
    }
}
