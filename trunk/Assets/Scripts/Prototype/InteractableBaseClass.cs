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



	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	public abstract InteractableType getType();
	public abstract bool getIsExitable();

}
