using UnityEngine;
using System.Collections;

public class BaseCollider : MonoBehaviour 
{

	public float KNOCKBACK;
	
	public float m_Damage;
	
	protected bool m_IsActive = false;

	float m_MoveSpeed;	
	float m_Range;
	Vector3 m_InitialPosition;

	bool m_isProjectile = false;
	
	// Update is called once per frame
	void Update () 
	{
		if (m_isProjectile)
		{
			transform.position += transform.forward * m_MoveSpeed * Time.deltaTime; //Move the projectile
			
			float distance = Vector3.Distance (m_InitialPosition, transform.position); //Get the distance travelled
			
			if(distance > m_Range)
			{
				Destroy(this.gameObject); //Check the range, if it the distance travelled is greater than it's range, destroy it
			}
		}
	}

	public void LaunchProjectile(float MoveSpeed, float Range)
	{
		m_IsActive = true;
		m_isProjectile = true;
		m_MoveSpeed = MoveSpeed;
		m_Range = Range;
		m_InitialPosition = transform.position;
	}

	public void Activate(bool isActive)
	{
		m_IsActive = isActive;
	}




}
