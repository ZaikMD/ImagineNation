using UnityEngine;
using System.Collections;

public class MoveableBlock : InteractableBaseClass
{


	public Size m_BlockSize; //This get's set by designers

	bool m_IsExitable;
	InteractableType m_Type;
	void Start()
	{
		m_IsExitable = true;
		m_Type = InteractableType.MovingBlock;
	}

	Size returnSize()
	{
		return m_BlockSize;
	}
	
	public void makeChild(GameObject obj)
	{
		transform.parent = obj.transform.parent;
	}
	
	public void removeParent()
	{
		//Remove the parent-child relation between the block and the player
		makeChild(null);             //Call makechild and pass in null to terminate the parent
	}

	public override bool getIsExitable ()
	{
		return m_IsExitable;
	}

	public override InteractableType getType ()
	{
		return m_Type;
	}
}