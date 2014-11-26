using UnityEngine;
using System.Collections;

public class ButtonV2CameraRotateSpeedDown: ButtonV2CameraRotationSpeed
{
	protected override void start ()
	{
		base.start ();
		m_Increment = -1.0f * Mathf.Abs (m_Increment);
	}
}
