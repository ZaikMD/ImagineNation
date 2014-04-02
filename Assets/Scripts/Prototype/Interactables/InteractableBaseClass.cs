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
	CrawlSpace,
	Unknown
};

public abstract class InteractableBaseClass : MonoBehaviour 
{


	protected InteractableType m_Type;

	protected bool m_IsExitable;


	public  InteractableType getType()
	{
		return m_Type;
	}

	public  bool getIsExitable()
	{
		return m_IsExitable;
	}

}
