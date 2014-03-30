using UnityEngine;
using System.Collections;

public enum InteractableType
{
	MovingBlock,
	Lever,
	DivingBoard, 
	SeeSaw,
	PickUp,
	NPC,
	CrawlSpace
};

public abstract class InteractableBaseClass : MonoBehaviour 
{

	public InteractableType m_Type;

	public bool m_IsExitable;


	public  InteractableType getType()
	{
		return m_Type;
	}

	public  bool getIsExitable()
	{
		return m_IsExitable;
	}

}
