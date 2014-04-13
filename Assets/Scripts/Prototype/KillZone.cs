using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerStay(Collider obj)
	{
		if(obj.tag == "Player")
		{
			obj.GetComponent<Health>().takeDamage(10);

		}
	}
}
