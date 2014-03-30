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
	PlayerMovement m_Movement;
	bool canInteract = true;

	void Start()
	{
		m_IsExitable = true;
		m_Type = InteractableType.MovingBlock;
	}

	void Update ()
	{
		if (m_Movement)
		{
			if (Input.GetKeyDown(KeyCode.F) && canInteract)
			{
				removeParent();
			}

			m_Movement.BlockHeldMovement(m_BlockSize);
		}
		if (!canInteract)
		{
			canInteract = true;
		}
	}

	//Get the size of the block
	Size returnSize()
	{
		return m_BlockSize;
	}

	/// <summary>
	/// Set block to being moved by a player
	/// </summary>
	public void makeChild(GameObject obj)
	{
		m_Movement = (PlayerMovement)obj.GetComponent<PlayerMovement> ();
		if (m_Movement)
		{
			//Look at the block
			Vector3 lookAt = transform.position;
			lookAt.y = obj.transform.position.y;
			obj.transform.LookAt(lookAt);

			//set the state of the block and player
			transform.parent = obj.transform;
			m_Movement.setCanMove (false);
			rigidbody.useGravity = false;

			//Move the block in front of the player
			Vector3 newPos = obj.transform.position;
			newPos.y = transform.position.y;
			transform.position = newPos + ((obj.transform.forward * 1.2f) * transform.localScale.x);

			//Fix instantly exiting block pushing
			canInteract = false;
		}
	}
	
	/// <summary>
	/// Set block to not being moved by any player.
	/// </summary>
	public void removeParent()
	{
		transform.parent = null;
		m_Movement.setCanMove (true);
		m_Movement = null;
		rigidbody.useGravity = true;
	}

	void OnCollisionEnter (Collision obj)
	{
		if (transform.parent && obj.gameObject != transform.parent)
		{
			//removeParent();
		}
	}
	
}