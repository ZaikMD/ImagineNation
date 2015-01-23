using UnityEngine;
using System.Collections;

/* Created by: Kole Takney.
 * Date: jan 22, 2015
 * 
 * this class preforms a calculation to get the projectiles
 * initial velocity, then travels at a set horizontal velocity
 * with gravity applied. 
 * 
 */

#region 
/*
 * 
 * 
 * 
 */
#endregion


public class BeanBag : MonoBehaviour
{
	public float m_HorizontalSpeed; // Speed towards final position;
	public float m_VerticalAcceleration; //Gravity
	
	private Vector3 m_InitialPosition;
	private Vector3 m_FinalPosition;

	Vector3 m_CurrentVelocity;
	const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;


	void Start ()
	{
		Vector2 XZStart = new Vector2(m_InitialPosition.x, m_InitialPosition.z);
		Vector2 XZFinal = new Vector2(m_FinalPosition.x, m_FinalPosition.z);
		
		float VerticalDifference = m_InitialPosition.y - m_FinalPosition.y;
				
		Vector2 HorizontalDifference = XZFinal - XZStart;
		Vector2 HorizontalVelocity = HorizontalDifference;
		HorizontalVelocity.Normalize();

		float DistanceToTravel = Mathf.Sqrt(Mathf.Pow(HorizontalDifference.x, 2) + Mathf.Pow(HorizontalDifference.y, 2));  
		float speed = Mathf.Sqrt(Mathf.Pow(HorizontalVelocity.x, 2) + Mathf.Pow(HorizontalVelocity.y, 2));

		float TimeTillImpact = DistanceToTravel / (speed * m_HorizontalSpeed);
		float TimeTillImpactx = HorizontalDifference.y / HorizontalVelocity.y;

		//Safety check so we don't divide by zero.
		if(VerticalDifference != 0 && TimeTillImpact != 0)
		{
			float InitialYVelocity = (VerticalDifference / TimeTillImpact) - (m_VerticalAcceleration * TimeTillImpact * 0.5f);
			m_CurrentVelocity = new Vector3(HorizontalVelocity.x * m_HorizontalSpeed, -InitialYVelocity, HorizontalVelocity.y * m_HorizontalSpeed);
		}
		else
		{
			float InitialYVelocity = 0 - (m_VerticalAcceleration * TimeTillImpact * 0.5f);
			m_CurrentVelocity = new Vector3(HorizontalVelocity.x * m_HorizontalSpeed, -InitialYVelocity, HorizontalVelocity.y * m_HorizontalSpeed);
		}
	}
	
	void Update () 
	{
		//Check if paused
		if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }
		
		//Update Postion
		transform.Translate(m_CurrentVelocity * Time.deltaTime);
		
		//Update Velocity
		m_CurrentVelocity.y -= m_VerticalAcceleration * Time.deltaTime;
	}

	/// <summary>
	/// Sets the varibals to calculate the initial velocity of projectile.
	/// </summary>1
	/// <param name="LaunchLocation">The location where the projectile is being launched from.</param>
	/// <param name="DestinationLocation">The location the projectile is supposed to land.</param>
	/// <param name="NewSpeed">The speed at which the projectile will travel.</param>
	public void SetVelocity(Vector3 LaunchLocation, Vector3 DestinationLocation, float NewSpeed, float NewGravity)
	{
		m_HorizontalSpeed = NewSpeed;
		m_VerticalAcceleration = NewGravity;
		m_InitialPosition = LaunchLocation;
		m_FinalPosition = DestinationLocation;
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "BeanBag" || obj.tag == "BeanBagLauncher" || obj.tag == "CollideWithMovingPlatforms")
		{
			return;
		}

		if(obj.gameObject.GetComponent(typeof(Attackable)) as Attackable != null)//checks to see if the object that has been hit is attackable
		{
			Attackable attackable = obj.gameObject.GetComponent(typeof(Attackable)) as Attackable; //if so call the onhit function and pass in the gameobject

			EnemyProjectile tempProjectile = new EnemyProjectile();

			Vector3 KnockBackDirection = new Vector3( m_CurrentVelocity.x, 0, m_CurrentVelocity.y);

			attackable.onHit(tempProjectile, KnockBackDirection);
		} 
		//destroy gameobject;
		Destroy(this.gameObject);
	}
}