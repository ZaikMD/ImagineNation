using UnityEngine;
using System.Collections;

public class GnomeKnockedBackBehaviour : BaseKnockedBackBehavouir 
{

	// Use this for initialization
	protected override void start ()
	{
		m_LaunchAmount = 10.0f;
		base.start ();
	}
}
