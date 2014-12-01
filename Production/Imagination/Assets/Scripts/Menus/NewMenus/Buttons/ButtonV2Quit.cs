using UnityEngine;
using System.Collections;

public class ButtonV2Quit : ButtonV2
{
	public override void use (PlayerInput usedBy = PlayerInput.None)
	{
		Application.Quit ();
	}
}
