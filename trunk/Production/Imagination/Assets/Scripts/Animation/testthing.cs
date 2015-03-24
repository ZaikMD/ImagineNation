using UnityEngine;
using System.Collections;

public class testthing : MonoBehaviour 
{
	public AnimatorGnomeMage.Animations animToPlay = AnimatorGnomeMage.Animations.Clone;
	public AnimatorGnomeMage testy;

	public bool Cylcing = false;

	// Update is called once per frame
	void Update () 
	{
		if(Cylcing)
		{
			if(Input.GetKeyDown(KeyCode.Q))
			{
				animToPlay = AnimatorGnomeMage.Animations.Attack;
			}
			else if(Input.GetKeyDown(KeyCode.W))
			{
				animToPlay = AnimatorGnomeMage.Animations.Clone;
			}
			else if(Input.GetKeyDown(KeyCode.E))
			{
				animToPlay = AnimatorGnomeMage.Animations.Hover;
			}
		}

		testy.playAnimation (animToPlay);
	}
}
