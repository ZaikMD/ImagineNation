using UnityEngine;
using System.Collections;

public class RCCarMovement : MonoBehaviour 
{
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;

	public float[] GearRatio;
	public int CurrentGear;
	
	public float EngineTorque = 800;
	public float MaxEngineRPM = 800;
	public float MinEngineRPM = 500;
	
	public float EngineRPM;

	public bool m_CanMove;

	public RCCar m_RCCarManager{ get; set;}

	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass = new Vector3 (0, 0, 0);
		m_CanMove = true;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void RegularMove()
	{
		if (m_CanMove)
		{
		    //this.rigidbody.AddForce (new Vector3 (0, -9000, 0));

			rigidbody.drag = rigidbody.velocity.magnitude / 250;
	
			EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio [CurrentGear - 1];
	
			ShiftGear ();
			FrontLeftWheel.motorTorque = EngineTorque / GearRatio [CurrentGear - 1] * -PlayerInput.Instance.getMovementInput ().y;
			FrontRightWheel.motorTorque = EngineTorque / GearRatio [CurrentGear - 1] * -PlayerInput.Instance.getMovementInput ().y;
	
			FrontLeftWheel.steerAngle = 15 * PlayerInput.Instance.getMovementInput ().x;
			FrontRightWheel.steerAngle = 15 * PlayerInput.Instance.getMovementInput ().x;
		}

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
