using UnityEngine;
using System.Collections;

public class ButtonV2Quit : ButtonV2
{
	public override void use ()
	{
		Application.Quit ();
	}
}
