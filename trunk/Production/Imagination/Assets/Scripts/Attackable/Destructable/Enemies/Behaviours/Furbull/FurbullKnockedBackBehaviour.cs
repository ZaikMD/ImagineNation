using UnityEngine;
using System.Collections;

public class FurbullKnockedBackBehaviour : BaseKnockedBackBehavouir {
	
	// Use this for initialization
	protected override void start ()
	{
		m_LaunchAmount = 12.5f;
		base.start ();
	}
}
