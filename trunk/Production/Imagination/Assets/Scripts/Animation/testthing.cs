using UnityEngine;
using System.Collections;

public class testthing : MonoBehaviour 
{
	public AnimatorGnomeMage.Animations animToPlay = AnimatorGnomeMage.Animations.Clone;
	public AnimatorGnomeMage testy;

	// Update is called once per frame
	void Update () 
	{
		testy.playAnimation (animToPlay);
	}
}
