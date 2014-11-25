using UnityEngine;
using System.Collections;
/// <summary>
/// Pressure plates.
/// 
/// created by Zach Dubuc
/// 
/// Pressure plates work like switches and will activate when a player or moving block it on them
/// </summary>

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented and changed strings to constants- Zach Dubuc
*
* 12/11/2014 Edit: Added in beenHit Overide- Zach Dubuc
*/
#endregion
public class PressurePlates : SwitchBaseClass
{

    //Bool to ignore players
	public bool m_IngorePlayers;

	ArrayList m_List; //List of gameobjects that have stepped on the pressure plate
	public GameObject m_MovingBoxStopPoint; //Stop point for the moving block

	// Use this for initialization
	void Start () 
	{
		m_List = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () 
	{ 
        if (PauseScreen.IsGamePaused){return;}
	}

	void OnTriggerEnter(Collider obj)
	{
		if(!m_IngorePlayers) //If players aren't ignored
		{

			if(obj.gameObject.tag == Constants.PLAYER_STRING)
			{
				m_List.Add(obj.gameObject); //Add the gameobject to the list if it was a player

			}
		}

		if(obj.gameObject.tag == Constants.MOVING_BLOCK_TAG_STRING) //If the gameobject is a moving block
		{
			m_List.Add(obj.gameObject); //Add  it to the list and set it's destination as the stop point
			MovingBlock block = obj.gameObject.GetComponent(typeof(MovingBlock)) as MovingBlock;
			
			block.setPressurePlateDestination(m_MovingBoxStopPoint.transform.position);
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.gameObject.tag == Constants.PLAYER_STRING) //If the object that leaves the trigger is a player, remove it from the list
		{
			m_List.Remove(obj.gameObject);
		}
		
		if(obj.gameObject.tag == Constants.MOVING_BLOCK_TAG_STRING) //If the object that leaves the trigger is a moving block, remove it from the list
		{
			m_List.Remove(obj.gameObject);
		}
	}

	public override bool beenHit()
	{
		return false;
	}

    public override bool getActive()
    {
        return m_List.Count > 0; //Return whether or not the plate is active
    }
}
