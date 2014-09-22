using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	public int m_CheckPointNumber;

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
			GameData.Instance.CurrentCheckPoint = this;
		}
	}

	public int getCheckPointNumber()
	{
		return m_CheckPointNumber;
	}
}
