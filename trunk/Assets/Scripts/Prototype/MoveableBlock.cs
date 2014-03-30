using UnityEngine;
using System.Collections;

public class MoveableBlock : InteractableBaseClass
{
	//This get's set by designers
	public Size m_BlockSize;

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
	public void makeChild(GameObject obj)
	{
		transform.parent = obj.transform.parent;
	}
	
	/// <summary>
	/// Set block to not being moved by any player.
	/// </summary>
	public void removeParent()
	{
		makeChild(null);
	}
	
}