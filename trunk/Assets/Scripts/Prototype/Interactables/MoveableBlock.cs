/*

Created by Zach


3/30/2014
	Now work with movement.
	Now allows you to exit the pushing block state.
	Now adds and removes a rigid body to the block.
	Now is no longer pushed if it collides with something else
	Is now moved in front of the player to avoid colliding with them
4/6/2014
	Fixed the block sometime flying
	Fixed movable block movement (in Playermovement)
4/8/2014
	Fixed block teleporting
	Fixed blocks colliding with projectiles
4/14/2014
	Block now moves a set distance in front of player and has it's local pos only effect distance (not y pos)
*/


using UnityEngine;
using System.Collections;

public class MoveableBlock : InteractableBaseClass
{
	//This get's set by designers
	public Size m_BlockSize;
	bool canInteract = true;
	Vector3 m_SavedLocalPos;
	Quaternion m_SavedLocalRotation;
	bool m_InUse = false;
	const float PUSH_DISTANCE = 1.85f;
	

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

	void Update ()
	{
		if (m_InUse)
		{
			transform.localPosition = new Vector3 (m_SavedLocalPos.x, transform.localPosition.y, m_SavedLocalPos.z);
			transform.localRotation = m_SavedLocalRotation;
		}

		if (!rigidbody.isKinematic && !m_InUse && isGrounded())
		{
			rigidbody.isKinematic = true;
		}
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
			lookAt.y = obj.transform.position.y;
			obj.transform.LookAt(lookAt);

			//Make sure block does not collide with ground
			rigidbody.isKinematic = false;

			//set the state of the block and obj
			transform.parent = obj.transform;

			//Move the block in front of the obj
			Vector3 newPos = obj.transform.position;
			newPos.y = transform.position.y;
			transform.position = newPos + obj.transform.forward * obj.transform.localScale.x * 1.35f;
			m_SavedLocalPos = transform.localPosition;

			Vector3 pos = m_SavedLocalPos;
			pos.y = 0.0f;
			m_SavedLocalPos = PUSH_DISTANCE * pos.normalized;

			m_SavedLocalRotation = transform.localRotation;
			Physics.IgnoreCollision(collider, obj.collider);

			//Fix instantly exiting block pushing
			canInteract = false;

			m_InUse = true;
		}
	}
	
	/// <summary>
	/// Set block to not being moved by any player.
	/// </summary>
	public void onExit()
	{
		Physics.IgnoreCollision(collider, transform.parent.collider, false);
		transform.parent = null;
		m_InUse = false;

		if (!isGrounded())
		{
			//Back to normal
			rigidbody.isKinematic = false;
		}
	}

	bool isGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, 0.1f - rigidbody.velocity.y / 30.0f);
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
		else if(obj.tag == "RCCar")
		{
			obj.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
		else if(obj.tag == "RCCar")
		{
			obj.gameObject.GetComponent<RCCarMovement>().m_RCCarManager.interactionOutOfRange(this);
		}
	}	
}