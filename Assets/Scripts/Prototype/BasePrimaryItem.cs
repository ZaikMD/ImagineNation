using UnityEngine;
using System.Collections;

public abstract class BasePrimaryItem : MonoBehaviour 
{
	protected GameObject m_BaseProjectile;
	
	CameraController m_Camera;
	
	public virtual void fire()
	{
		Instantiate(m_BaseProjectile);
	}
	
	public virtual void aimFire()
	{
		m_Camera.enableAiming ();
		fire ();
	}
}
