using UnityEngine;
using System.Collections;

public class RCCarWheelAllignment : MonoBehaviour 
{
	public WheelCollider m_WheelCollider;
	float m_Rotation = 0.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hit;
		Vector3 colliderCenter = m_WheelCollider.transform.TransformPoint (m_WheelCollider.center);
		
		if (Physics.Raycast (colliderCenter, - m_WheelCollider.transform.up, out hit, m_WheelCollider.suspensionDistance + m_WheelCollider.radius))
		{
			transform.position = hit.point + (m_WheelCollider.transform.up * m_WheelCollider.radius);
		}
		else
		{
			transform.position = colliderCenter - (m_WheelCollider.transform.up * m_WheelCollider.suspensionDistance);
		}
		
		transform.rotation = m_WheelCollider.transform.rotation * Quaternion.Euler (m_Rotation, m_WheelCollider.steerAngle, 0);
		
		m_Rotation += m_WheelCollider.rpm * (360 / 60) * Time.deltaTime;
	}
}
