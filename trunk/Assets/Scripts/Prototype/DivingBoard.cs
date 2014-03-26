using UnityEngine;
using System.Collections;

public class DivingBoard : MonoBehaviour 
{

	public SeeSaw seeSaw;

	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public void notifySeeSaw(GameObject obj)
	{
		//Notify the SeeSaw of the player interaction
		seeSaw.playerJumping (obj.gameObject);
	}

}
