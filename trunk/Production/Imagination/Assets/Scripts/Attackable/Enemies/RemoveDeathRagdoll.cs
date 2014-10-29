using UnityEngine;
using System.Collections;

/*
 * Created by Joe Burchill
 * Date: Oct. 29, 2014
 *  
 * This script handles destroying the smoke effect
 * after a certain amount of time it will be destroyed
 * so the object doesn't stay in the scene after being
 * instantiated.
 * 
 */
#region ChangeLog
/* 
 * 
 */ 
#endregion

public class RemoveDeathRagdoll : MonoBehaviour 
{

	private float m_RagDollTimer = RAGDOLL_LIFETIME;
	private const float RAGDOLL_LIFETIME = 3.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_RagDollTimer <= 0.0f)
		{
			Destroy(this.gameObject);
		}
		else
		{
			m_RagDollTimer -= Time.deltaTime;
		}
	}
}
