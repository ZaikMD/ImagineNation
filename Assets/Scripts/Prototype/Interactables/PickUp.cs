//To use:

//The script accessing it needs a OnTriggerEnter
//Give the item it is attached to a trigger and collider

using UnityEngine;
using System.Collections;

public class PickUp : InteractableBaseClass, Observer
{
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
		Rigidbody rigid = gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = true;
		m_PlayerHolding.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		m_PlayerHolding = null;
	}

	//when it is being called, will check to pick up
	public void PickUpItem(GameObject other)
	{
		if(other.CompareTag("Player"))
		{
			this.transform.parent = other.transform.Find ("PickUpPoint");
			m_HasPickup = true;
			this.transform.localPosition = Vector3.zero;
			this.transform.localRotation = Quaternion.identity;
			rigidbody.useGravity = false;

			m_PlayerHolding = other;
			Destroy( gameObject.GetComponent<Rigidbody>());
		}
	}

	public void recieveEvent(Subject sender, ObeserverEvents recievedEvent)
	{
		if(sender.tag == "DropZone" && recievedEvent == ObeserverEvents.PickUpIsAtDropZone)
		{
			Destroy(this.gameObject);
		}
	}
}
