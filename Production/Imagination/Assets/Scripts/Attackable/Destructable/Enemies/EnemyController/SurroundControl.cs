/*
 * Created by Mathieu Elias December 1, 2014
 * Control type which makes the group surround their target
 */ 
#region ChangeLog
/* 
 * 
 * 
 */
#endregion
using UnityEngine;
using System.Collections;

public class SurroundControl : BaseControlType 
{
	float m_StoppingDist = 2.0f;

	float m_AngleZero;


	// Gets the group of enemies ready to be controlled
	public override void start (EnemyAI[] enemies, GameObject target)
	{
		m_EnemyGroup = enemies;

		m_Target = target;

		for (int i = 0; i < m_EnemyGroup.Length; i++)
		{
			// Stop all the enemies in the group from updating their movement ad targeting
			m_EnemyGroup[i].m_UMovement = false;
			//m_EnemyGroup[i].m_UTargeting = false;

			// Make sure all their states are set to attack
			m_EnemyGroup[i].SetState(EnemyAI.EnemyState.Attack);
		}
	}

	public override void update ()
	{
		// Get the angles at which the enemies will come at their target
		float angle = 360 / m_EnemyGroup.Length;

		float currentAngle = 0;

		for (int i = 0; i < m_EnemyGroup.Length; i++)
		{ 
			if (m_EnemyGroup[i] != null)
			{
				// Choose the enemies surrond location
				Vector3 surroundLocation = SurroundPoint(m_Target.transform.position, currentAngle);

				Instantiate(Resources.Load("Cube"),surroundLocation,Quaternion.identity);
				
				EnemyWithMovement temp = m_EnemyGroup[i] as EnemyWithMovement;		
				if(temp != null)
				{
					NavMeshAgent agent =  temp.GetAgent;
					agent.stoppingDistance = 2.0f;
					agent.SetDestination(surroundLocation);
				}
				currentAngle += angle;
			}
		}
	}

	public override void end ()
	{
		for (int i = 0; i < m_EnemyGroup.Length; i++)
		{
			if (m_EnemyGroup[i] != null)
			{
				// Stop all the enemies in the group form updating their movement
				m_EnemyGroup[i].m_UMovement = true;
				m_EnemyGroup[i].m_UTargeting = true;
			}
		}
		m_EnemyGroup = null;
	}

	// Returns a location along the rotation of an object
	protected Vector3 SurroundPoint(Vector3 point, float angle)
	{
		float pointX = Mathf.Cos (angle);
		float pointZ = Mathf.Sin (angle);

		pointX += point.x;
		pointZ += point.z;

		Vector3 surroundPoint = new Vector3 (pointX, point.y, pointZ);
		
		return surroundPoint;
	} 
}
