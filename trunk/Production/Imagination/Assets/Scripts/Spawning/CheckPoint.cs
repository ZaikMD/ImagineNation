using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{

	public CheckPoints m_Value;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == "Player")
		{
			GameData.Instance.CurrentCheckPoint = m_Value;
		}
	}
}
