using UnityEngine;
using System.Collections;

public class Lever : InteractableBaseClass 
{

	private bool m_IsOn;
	// Use this for initialization
	void Start () 
	{
		m_Type = InteractableType.Lever;
		m_IsExitable = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool getIsOn()
	{
		return m_IsOn;
	}

	public void toggleIsOn()
	{
		m_IsOn = !m_IsOn;
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionInRange(this);
		}
	}
	
	void OnTriggerExit(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.gameObject.GetComponent<PlayerState>().interactionOutOfRange(this);
		}
	}
}
