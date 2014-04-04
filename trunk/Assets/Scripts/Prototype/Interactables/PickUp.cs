//To use:

//The script accessing it needs a OnTriggerEnter
//Give the item it is attached to a trigger and collider

using UnityEngine;
using System.Collections;

public class PickUp : InteractableBaseClass
{
	public float m_BounceMultiplier = 3.0f;
	public bool m_HasPickup = false;
	public SeeSaw m_SeeSaw;

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

	//checks if the item being carried has hit the drop zone
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "DropZone")
		{
			//this.transform.parent = other.transform.Find ("DropZonePoint");
			this.transform.localPosition = Vector3.zero;
		//	this.transform.localRotation = Quaternion.identity;
			this.transform.localRotation = Quaternion.Euler( new Vector3(90,0,0));
			//Call this in seesaw to attach the 
			m_SeeSaw.placePieces(this.gameObject);

			m_HasPickup = false;

		}
		PickUpItem (other);
	}

	//when it is being called the item will be checked to be dropped
	public void DropItem()
	{
		//TODO Change the key to be pressed
		if(Input.GetKeyDown(KeyCode.T) && m_HasPickup == true)
		{
			this.transform.parent = null;
			rigidbody.useGravity = true;

			//m_HasPickup = false;
		}
	}

	//when it is being called, will check to pick up
	public void PickUpItem(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			this.transform.parent = other.transform.Find ("PickUpPoint");
			m_HasPickup = true;
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.Euler( new Vector3(90,0,0));
			//this.transform.localRotation = Quaternion.identity;
			rigidbody.useGravity = false;

			//this.transform.parent = other.transform.Find ("ItemPickPoint");

		}
	}
}
