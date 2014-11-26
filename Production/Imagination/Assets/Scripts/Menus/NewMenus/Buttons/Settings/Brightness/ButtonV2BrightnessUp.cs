using UnityEngine;
using System.Collections;

public class ButtonV2BrightnessUp : ButtonV2Brightness 
{
	protected override void start ()
	{
		base.start ();
		m_Increment = Mathf.Abs (m_Increment);
	}
}
