using UnityEngine;
using System.Collections;

public class ButtonV2BrightnessDown: ButtonV2Brightness 
{
	protected override void start ()
	{
		base.start ();
		m_Increment = -1.0f * Mathf.Abs (m_Increment);
	}
}
