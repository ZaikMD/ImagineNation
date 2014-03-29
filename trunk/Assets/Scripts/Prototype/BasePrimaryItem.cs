using UnityEngine;
using System.Collections;

public abstract class BasePrimaryItem : MonoBehaviour 
{
	protected GameObject m_BaseProjectile;
	
	CameraController m_Camera;
	
	public virtual void fire(Vector3 target)
	{
		Instantiate(m_BaseProjectile);
	}
	
	public virtual void aimFire(Vector3 target)
	{
		m_Camera.enableAiming ();
		fire (target);
	}
}
