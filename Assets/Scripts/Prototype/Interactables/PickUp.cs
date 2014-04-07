//To use:

//The script accessing it needs a OnTriggerEnter
//Give the item it is attached to a trigger and collider

using UnityEngine;
using System.Collections;

public class PickUp : InteractableBaseClass, Observer
{
	//initializing values
	public float m_BounceMultiplier = 3.0f;
	public bool m_HasPickup = false;
	public DropZone m_DropZone;

	public bool m_IsAdded = false;

	Vector3 m_StartPosition;

	GameObject m_PlayerHolding;
	
	// Use this for initialization
	void Start () 
	{
		m_Type = InteractableType.PickUp;		
		m_IsExitable = true;


		m_StartPosition = transform.position;

		m_DropZone.addObserver (this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the player has not picked up the pick up item. It will spin and bounce to help the player locate it. If the player drops it, it will not spin again
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
		if(!m_IsAdded)
		{
			if(other.tag == "Player")
			{
				m_IsAdded = true;
				other.gameObject.GetComponent<PlayerState>().interactionInRange(this);
			}
		}
	}

	//when the player exits the trigger, it says the interaction is out of range
	void OnTriggerExit(Collider obj)
	{
		if(m_IsAdded)
		{
			if(obj.tag == "Player")
			{
				m_IsAdded = false;
				obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
			}
		}
	}


	//when it is being called the item will be checked to be dropped
	public void DropItem()
	{
		this.transform.parent = null;
		Rigidbody rigid = gameObject.AddComponent<Rigidbody> (); // add a rigidbody so it can be dropped from heights and fall on the ground
		rigidbody.useGravity = true; //add gravity to the rigidbody
		m_PlayerHolding.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		m_PlayerHolding = null;
	}

	//when it is being called, will check to pick up
	public void PickUpItem(GameObject other)
	{
		if(other.CompareTag("Player"))
		{
			//moves the object to the players empty game object PickUpPoint
			this.transform.parent = other.transform.Find ("PickUpPoint");
			m_HasPickup = true;
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
			rigidbody.useGravity = false;// take off rotation

			m_PlayerHolding = other;
			Destroy( gameObject.GetComponent<Rigidbody>()); // delete the rigid body so that it does not fall out of the player's hands
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(sender.tag == "DropZone" && recievedEvent == ObeserverEvents.PickUpIsAtDropZone)
		{
			Destroy(this.gameObject);	// if the pick up hits the drop zone, the pick will be deleted (the reason it is deleted is because we enable a gameobject that was at the 
			//specified location from  the very start.
		}
	}
}
