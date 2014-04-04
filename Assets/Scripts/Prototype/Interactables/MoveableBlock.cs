/*

Created by Zach


3/30/2014
	Now work with movement.
	Now allows you to exit the pushing block state.
	Now adds and removes a rigid body to the block.
	Now is no longer pushed if it collides with something else
	Is now moved in front of the player to avoid colliding with them
*/


using UnityEngine;
using System.Collections;

public class MoveableBlock : InteractableBaseClass
{
	//This get's set by designers
	public Size m_BlockSize;
	bool canInteract = true;

	void Start()
	{
		m_IsExitable = true;
		m_Type = InteractableType.MovingBlock;
	}

	//Get the size of the block
	Size returnSize()
	{
		return m_BlockSize;
	}

	/// <summary>
	/// Set block to being moved by a player
	/// </summary>
	public void onUse(GameObject obj)
	{
		if(obj.tag == "Player" || obj.tag == "RCCar")
		{
			if(obj.tag == "Player")
			{
				PlayerMovement movement = (PlayerMovement)obj.GetComponent<PlayerMovement> ();
				movement.setCanMove (false);
			}

			//Look at the block
			Vector3 lookAt = transform.position;
			Vector3 newPos;

			if(obj.tag == "RCCar")
			{
				GameObject rcCar = obj;//.transform.parent.gameObject;



				lookAt.y = rcCar.transform.position.y;
				rcCar.transform.LookAt(lookAt);
				
				//set the state of the block and player
				transform.parent = rcCar.transform;
				
				rigidbody.useGravity = false;
				
				//Move the block in front of the player
				newPos = rcCar.transform.position;
				newPos.y = transform.position.y;
				transform.position = newPos + ((rcCar.transform.forward * 1.2f) * transform.localScale.x);
				
				//Fix instantly exiting block pushing
				canInteract = false;
				return;
			}

			lookAt.y = obj.transform.position.y;
			obj.transform.LookAt(lookAt);

			//set the state of the block and player
			transform.parent = obj.transform;
			
			rigidbody.useGravity = false;

			//Move the block in front of the player
			newPos = obj.transform.position;
			newPos.y = transform.position.y;
			transform.position = newPos + ((obj.transform.forward * 1.2f) * transform.localScale.x);

			//Fix instantly exiting block pushing
			canInteract = false;

		}

		//TODO: rc car
	}
	
	/// <summary>
	/// Set block to not being moved by any player.
	/// </summary>
	public void onExit()
	{
		transform.parent = null;
		rigidbody.useGravity = true;
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}

		if(obj.tag == "RCCar")
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}

		if(obj.tag == "RCCar")
		{
			obj.transform.parent.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutOfRange(this);
		}
	}	
}