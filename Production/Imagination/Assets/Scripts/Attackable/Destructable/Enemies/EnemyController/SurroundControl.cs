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
				Vector3 surroundLocation = RotateAboutOrigin( m_EnemyGroup[i].transform.position , m_Target.transform.position, angle);

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
	protected Vector3 RotateAboutOrigin(Vector3 point, Vector3 origin, float angle)
	{
		// Convert the angle to radians
		angle = angle * (Mathf.PI / 180);
		
		// Find out the new x and z locations 
		float rotatedX = Mathf.Cos (angle) * (point.x - origin.x) - Mathf.Sin (angle) * (point.z - origin.z) + origin.x;
		float rotatedZ = Mathf.Sin (angle) * (point.x - origin.x) - Mathf.Cos (angle) * (point.z - origin.z) + origin.z;
		
		return new Vector3(rotatedX, point.y, rotatedZ);
	} 
}
