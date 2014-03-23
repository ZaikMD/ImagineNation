using UnityEngine;
using System.Collections;

public class MoveableBlock : MonoBehaviour
{


	public Size m_BlockSize; //This get's set by designers

	void start()
	{
		
	}
	void Update()
	{

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
}