using UnityEngine;
using System.Collections;

public class DivingBoard : MonoBehaviour 
{

	public SeeSaw seeSaw;

	public void notifySeeSaw(GameObject obj)
	{
		//Notify the SeeSaw of the player interaction
		seeSaw.playerJumping (obj.gameObject);
	}

}
