using UnityEngine;
using System.Collections;

public class LevelGoal : BaseGoal 
{


	public override void LoadNext()
	{
		//Tell Game Data Stuff

		Application.LoadLevel (m_NextScene);

	}


}
