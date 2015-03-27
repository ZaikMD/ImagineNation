using UnityEngine;
using System.Collections;

public class Credits : MenuV2
{
	public Spin i_CreditsReel;

	protected override void OnActivated()
	{
		i_CreditsReel.resetAngles ();
	}
}
