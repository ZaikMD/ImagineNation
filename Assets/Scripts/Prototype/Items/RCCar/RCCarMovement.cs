using UnityEngine;
using System.Collections;

public class RCCarMovement : MonoBehaviour 
{
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;

	public float[] GearRatio;
	public int CurrentGear;
	
	public float EngineTorque = 800;
	public float MaxEngineRPM = 1000;
	public float MinEngineRPM = 500;
	
	public float EngineRPM;

	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass = new Vector3 (rigidbody.centerOfMass.x, 0, rigidbody.centerOfMass.z);

	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	public void RegularMove()
	{
			rigidbody.AddForce (new Vector3 (0, -20, 0));
			rigidbody.drag = rigidbody.velocity.magnitude / 250;
	
			EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio [CurrentGear - 1];
	
			ShiftGear ();
			FrontLeftWheel.motorTorque = EngineTorque / GearRatio [CurrentGear - 1] * -PlayerInput.Instance.getMovementInput ().y;
			FrontRightWheel.motorTorque = EngineTorque / GearRatio [CurrentGear - 1] * -PlayerInput.Instance.getMovementInput ().y;
	
			FrontLeftWheel.steerAngle = 15 * PlayerInput.Instance.getMovementInput ().x;
			FrontRightWheel.steerAngle = 15 * PlayerInput.Instance.getMovementInput ().x;
	}

	void ShiftGear()
	{
		if (EngineRPM >= MaxEngineRPM)
		{
			CurrentGear++;
			if(CurrentGear >= GearRatio.Length)
			{
				CurrentGear = GearRatio.Length;
			}
		}
		if (EngineRPM <= MinEngineRPM)
		{
			CurrentGear--;
			if(CurrentGear <= 1)
			{
				CurrentGear = 1;
			}
		}
	}
}
