using UnityEngine;
using System.Collections;

public class PickUp : InteractableBaseClass
{
	public float m_BounceMultiplier = 3.0f;
	public bool m_HasPickup = false;

	Vector3 m_StartPosition;
	
	// Use this for initialization
	void Start () 
	{
		m_Type = InteractableType.PickUp;		
		m_IsExitable = true;


		m_StartPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_HasPickup == false)
		{
			float bounce = Mathf.Sin (Time.time * m_BounceMultiplier) * 0.2f + m_StartPosition.y;
		
			transform.position = new Vector3 (m_StartPosition.x, bounce, m_StartPosition.z);
		
			transform.Rotate (0, 1, Mathf.Sin (Time.time * m_BounceMultiplier) *0.2f + 1);
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "DropZone")
		{
			this.transform.parent = other.transform.Find ("DropZonePoint");
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
		}

		else if(other.gameObject.CompareTag("Player"))
		{
			//Destroy(this.gameObject);
			this.transform.parent = other.transform.Find ("ItemPickPoint");
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;

			m_HasPickup = true;
		}
	}
}
