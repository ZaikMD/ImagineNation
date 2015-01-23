using UnityEngine;
using System.Collections;

public class GnomeKnockedBackBehaviour : BaseKnockedBackBehavouir 
{

	// Use this for initialization
	protected override void start ()
	{
		m_LaunchAmount = 15.0f;
		base.start ();
	}
}
