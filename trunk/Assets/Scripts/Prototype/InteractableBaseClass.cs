using UnityEngine;
using System.Collections;

public abstract class InteractableBaseClass : MonoBehaviour 
{
	public enum InteractableType
	{
		MovingBlock,
		Lever,
		DivingBoard, 
		SeeSaw
	};


	public InteractableType m_Type;
	public bool m_IsExitable;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	public  InteractableType getType()
	{
		return m_Type;
	}
	public  bool getIsExitable()
	{
		return m_IsExitable;
	}

}
