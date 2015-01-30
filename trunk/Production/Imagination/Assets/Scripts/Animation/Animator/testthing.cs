using UnityEngine;
using System.Collections;

public class testthing : MonoBehaviour 
{
	public int animToPlay = 0;
	public AnimatorController testy;
	// Update is called once per frame
	void Update () 
	{
		testy.playAnimation (animToPlay);
	}
}
