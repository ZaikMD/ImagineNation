using UnityEngine;
using System.Collections;

public abstract class BasePrimaryItem : MonoBehaviour 
{
	protected GameObject m_BaseProjectile;
	protected Reticle m_Reticle;
	
    CameraController m_Camera;
	
	//Load Reticle
	void Awake()
	{
		Invoke ("Load", 0.001f);
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
		return (m_Reticle.getTargetPosition() - transform.position).normalized;
	}
}
