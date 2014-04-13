using UnityEngine;
using System.Collections;

public abstract class BasePrimaryItem : MonoBehaviour 
{
	protected GameObject m_BaseProjectile;
	protected Reticle m_Reticle;
	protected Range m_ProjectileRange;
	
    CameraController m_Camera;
	PlayerAIStateMachine m_AIStateMachine;

	//Load Reticle
	void Awake()
	{
		Invoke ("Load", 0.001f);
		m_AIStateMachine = gameObject.GetComponent<PlayerAIStateMachine> ();
	}

	//Fire weapon
	public virtual void fire()
	{
		Instantiate(m_BaseProjectile);
	}

	//Fire weapon and aim
	public virtual void aimFire()
	{
		m_Camera.enableAiming ();
		fire ();
	}

	//Loads the reticle
	void Load()
	{
		GameObject reticle = (GameObject)GameObject.Find("Reticle(Clone)");
		m_Reticle = (Reticle)reticle.GetComponent<Reticle> ();
	}

	/// <summary>
	/// Gets the direction to fire.
	/// </summary>
	protected Vector3 getTargetDirection()
	{
		if (m_AIStateMachine.m_IsActive)
		{
			return transform.forward;
		}
		return (m_Reticle.getTargetPosition() - transform.position).normalized;
	}

	public abstract float getRange();


}
