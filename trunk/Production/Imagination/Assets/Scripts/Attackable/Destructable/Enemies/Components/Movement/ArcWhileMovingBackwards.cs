/*
 * Created by Mathieu Elias
 * Date: Nov 23, 2014
 *  
 * This script makes the enemy move backwards while arcing left anf right around his target
 * 
 */
#region ChangeLog
/*
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class ArcWhileMovingBackwards : BaseMovement 
{
	public float m_RotSpeed = 15f;
	public float m_BackUpSpeed = 3f;
	private float m_SwitchRotTimer;
	public float m_MoveDist = 7.0f;
	private int m_MoveAngle = 90;

	public float m_MinSwitchRotTime = 0.3f;
	public float m_MaxSwitchRotTime = 3.0f;


	// override of the abstract movement function
	public override Vector3 Movement (GameObject target)
	{
		if (target != null)
		{
			m_Agent.speed = m_BackUpSpeed;
			// Update the switch rotation timer
			m_SwitchRotTimer -= Time.deltaTime;
			
			// If the timer is smaller or equal to zero  then simply switch the direction of his rotation
			if (m_SwitchRotTimer <= 0)
			{
				m_MoveAngle *= -1;
				// Reset the timer to a random value between .3 and 3 seconds
				m_SwitchRotTimer = Random.Range(m_MinSwitchRotTime, m_MaxSwitchRotTime);
			}
			
			// Apply the rotation while trying to stay a certain dist from the player
			Vector3 point = RotateAboutOrigin (transform.position, target.transform.position, m_MoveAngle);
			point += (transform.position - target.transform.position).normalized * m_MoveDist;

			m_Agent.SetDestination(point);

			return point;
		}
		return Vector3.zero;
	}
}
