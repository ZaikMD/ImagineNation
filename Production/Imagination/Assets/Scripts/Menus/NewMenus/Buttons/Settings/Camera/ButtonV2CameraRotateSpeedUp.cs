using UnityEngine;
using System.Collections;

public class ButtonV2CameraRotateSpeedUp: ButtonV2CameraRotationSpeed
{
	protected override void start ()
	{
		base.start ();
		m_Increment = Mathf.Abs (m_Increment);
	}
}