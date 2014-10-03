using UnityEngine;
using System.Collections;

public class PressurePlates : SwitchBaseClass
{


	public bool m_IngorePlayers;

	ArrayList m_List;
	public GameObject m_MovingBoxStopPoint;

	// Use this for initialization
	void Start () 
	{
		m_List = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter(Collider obj)
	{
		if(!m_IngorePlayers)
		{

			if(obj.gameObject.tag == "Player")
			{
				m_List.Add(obj.gameObject);

			}
		}

		if(obj.gameObject.tag == "MovingBlock")
		{
			m_List.Add(obj.gameObject);
			MovingBlock block = obj.gameObject.GetComponent(typeof(MovingBlock)) as MovingBlock;
			
			block.setPressurePlateDestination(m_MovingBoxStopPoint.transform.position);
		}
	}

	void OnTriggerExit(Collider obj)
	{
		if(obj.gameObject.tag == "Player")
		{
			m_List.Remove(obj.gameObject);
		}
		
		if(obj.gameObject.tag == "MovingBlock")
		{
			m_List.Remove(obj.gameObject);
		}
	}

    public override bool getActive()
    {
        return m_List.Count > 0;
    }
}
