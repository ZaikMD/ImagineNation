using UnityEngine;
using System.Collections;

public class TrampolineButton : MonoBehaviour 
{
	public Trampoline m_MyTrampoline;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Character")
		{
			if (other.gameObject.GetComponent<CharacterController>().velocity.y < 0)
			{

				if (Input.GetKeyDown(KeyCode.B))
				{

					m_MyTrampoline.m_DoubleJump = true;
				
				}
			}
		}
	}
}
