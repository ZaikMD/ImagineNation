/*
 * Created by Jason Hein
 * Date: Oct, 24, 2014
 *  
 * A data structure that keeps track of launch movements.
 * 
 */


using UnityEngine;
using System.Collections;

//Launches keep track of how long they last
public class LaunchMovement
{
	public Vector3 launch;
	public float timer;
	public bool resetOnGrounded;
	
	//Launch movement constructor
	public LaunchMovement (Vector3 aLauch, float startTime, bool doesResetWhenGrounded)
	{
		launch = aLauch;
		timer = startTime;
		resetOnGrounded = doesResetWhenGrounded;
	}
}
