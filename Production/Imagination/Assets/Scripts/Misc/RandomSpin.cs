// Created by Kris and Jason - Dec 2014
//
// Randomizes an objects rotation speed to give the appearance of a free form spin.
//
// TO-USE
// 1. Add this component to the object.
// 2. You have the option of change the initial settings of how the randomizing is done.

#region Changelog
/*
 * 
 */ 
#endregion


using UnityEngine;
using System.Collections;

public class RandomSpin : MonoBehaviour
{
	//Magnitude of rotation
	public float m_MaxRandomizedSpeed = 50.0f;
	Vector3 m_RandomizedSpeedMagnitude = Vector3.one;
	public Vector3 m_ConstantSpeed = new Vector3 (50.0f, 0.0f, 50.0f);

	//Timer
	const float MAX_DELAY = 1.0f;
	Vector2 i_Delay = Vector3.one;
	Vector2 m_Timer;

	//Set randomized rotation values
	protected void Start()
	{
		//Set the initial rotation to something random
		transform.rotation = new Quaternion(Random.Range (0.0f, 360.0f), Random.Range (0.0f, 360.0f), Random.Range (0.0f, 360.0f), 1.0f);

		//Set the new speed of the object
		newSpeed ();

		//Set timers and magnitude to be random for each peg
		i_Delay = new Vector2 (Random.Range (0.0f, MAX_DELAY), Random.Range (0.0f, MAX_DELAY));
		m_RandomizedSpeedMagnitude = new Vector3 (Random.Range (0.0f, m_MaxRandomizedSpeed), 0.0f, Random.Range (0.0f, m_MaxRandomizedSpeed));

		//Set the timer
		m_Timer = i_Delay;
	}
	
	//Update the roation of the object and its speed
	protected void Update () 
	{
		//Rotate the object
		transform.Rotate (m_RandomizedSpeedMagnitude * Time.deltaTime);
		transform.Rotate (m_ConstantSpeed * Time.deltaTime);

		//Update timers
		m_Timer.x -= Time.deltaTime;
		m_Timer.y -= Time.deltaTime;

		if(m_Timer.x < 0.0f)
		{
			newSpeedX();
			m_Timer.x = i_Delay.x;
		}

		if(m_Timer.y < 0.0f)
		{
			newSpeedZ();
			m_Timer.y = i_Delay.y;
		}
	}

	//Set a new speed
	void newSpeed()
	{
		newSpeedX ();
		newSpeedZ ();
	}

	//Set a new speed on the x axis
	void newSpeedX()
	{
		m_RandomizedSpeedMagnitude.x = Random.Range (0.0f, m_MaxRandomizedSpeed);
	}

	//Set a new speed on the z axis
	void newSpeedZ()
	{
		m_RandomizedSpeedMagnitude.z = Random.Range (0.0f, m_MaxRandomizedSpeed);
	}
}
