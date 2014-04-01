using UnityEngine;
using System.Collections;

public class Lever : InteractableBaseClass 
{

	public bool m_IsOn;
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
}
