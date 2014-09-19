using UnityEngine;
using System.Collections;

public class SectionGoal : BaseGoal {

	public override void LoadNext()
	{
		//Tell Game Data Stuff
		
		Application.LoadLevel (m_NextScene);
		
	}
}
