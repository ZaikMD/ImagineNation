// Created by Kris and Jason - Dec 2014
//
// Randomizes an objects rotation speed to give the appearance of a free form spin.
//
// TO-USE
// 1. Add this component to the object.
// 2. You have the option of change the initial settings of how the randomizing is done.

#region Changelog
/*
 * edit: Added options such as which axis to rotate on, and added random rotation on the y axis, and random Constant - Kole 
 * 
 */ 
#endregion


using UnityEngine;
using System.Collections;

public class RandomSpin : MonoBehaviour
{
	//Options
	public bool RandomizeXAxis;
	public bool RandomizeYAxis;
	public bool RandomizeZAxis;
	public bool RandomizeOnlyAtStart;


	//Magnitude of rotation
	public Vector3 m_MaxRandomizedSpeed;
	Vector3 m_RandomizedSpeedMagnitude = Vector3.one;
	public Vector3 m_ConstantSpeed = new Vector3 (0.0f, 0.0f, 50.0f);

	public bool UseRandomConstantSpeed;
	public Vector3 m_MaxConstantSpeed;
	public Vector3 m_MinConstantSpeed;

	//Timer
	const float MAX_DELAY = 1.0f;
	Vector3 i_Delay = Vector3.one;
	Vector3 m_Timer;

	//Set randomized rotation values
	protected void Start()
	{	
		float X;
		float MagX;
		float Y;
		float MagY;
		float Z;
		float MagZ;

		if(RandomizeXAxis)
		{
			X = Random.Range (0.0f, 360.0f);
		}
		else
		{
			X = transform.rotation.x;
		}


		if(RandomizeYAxis)
		{
			Y = Random.Range (0.0f, 360.0f);
			//Debug.Log(Y);
		}
		else
		{
			Y = transform.rotation.y;
		}

		if(RandomizeZAxis)
		{
			Z = Random.Range (0.0f, 360.0f);
		}
		else
		{
			Z = transform.rotation.z;
		}

		if(UseRandomConstantSpeed)
		{
			float x = Random.Range(m_MinConstantSpeed.x, m_MaxConstantSpeed.x);
			float y = Random.Range(m_MinConstantSpeed.y, m_MaxConstantSpeed.y);
			float z = Random.Range(m_MinConstantSpeed.z, m_MaxConstantSpeed.z);

			m_ConstantSpeed.Set(x, y, z);
		}


		//Set the initial rotation to something random
		transform.rotation = new Quaternion(X, Y, Z, 1.0f);

		transform.Rotate(transform.rotation.x, Y, transform.rotation.z);

		//Set the new speed of the object
		newSpeed ();

		//Set timers and magnitude to be random for each peg
		i_Delay = new Vector3 (Random.Range (0.0f, MAX_DELAY), Random.Range (0.0f, MAX_DELAY), Random.Range (0.0f, MAX_DELAY));
		m_RandomizedSpeedMagnitude = new Vector3 (Random.Range (0.0f, m_MaxRandomizedSpeed.x), Random.Range (0.0f, m_MaxRandomizedSpeed.y), Random.Range (0.0f, m_MaxRandomizedSpeed.z));

		//Set the timer
		m_Timer = i_Delay;
	}
	
	//Update the roation of the object and its speed
	protected void Update () 
	{
		//Rotate the object
		if(!RandomizeOnlyAtStart)
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
			newSpeedY();
			m_Timer.y = i_Delay.y;
		}

		if(m_Timer.z < 0.0f)
		{
			newSpeedZ();
			m_Timer.z = i_Delay.z;
		}

	}

	//Set a new speed
	void newSpeed()
	{
		newSpeedX ();
		newSpeedZ ();
		newSpeedY ();
	}

	//Set a new speed on the x axis
	void newSpeedX()
	{
		if(RandomizeXAxis)
		{
			m_RandomizedSpeedMagnitude.x = Random.Range (0.0f, m_MaxRandomizedSpeed.x);
		}
		else
		{
			m_RandomizedSpeedMagnitude.x = 0;
		}		
	}

	//Set a new speed on the z axis
	void newSpeedZ()
	{
		if(RandomizeZAxis)
		{
			m_RandomizedSpeedMagnitude.z = Random.Range (0.0f, m_MaxRandomizedSpeed.z);
		}
		else
		{
			m_RandomizedSpeedMagnitude.z = 0;
		}
	}

	void newSpeedY()
	{
		if(RandomizeYAxis)
		{
			m_RandomizedSpeedMagnitude.y = Random.Range (0.0f, m_MaxRandomizedSpeed.y);
		}
		else
		{
			m_RandomizedSpeedMagnitude.y = 0;
		}

	}
}
