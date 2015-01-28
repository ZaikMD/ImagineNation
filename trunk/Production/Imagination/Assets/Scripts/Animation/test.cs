using UnityEngine;
using System.Collections;

public class test : MonoBehaviour 
{
	public int animToPlay = 0;
	// Update is called once per frame
	void Update () 
	{
		BaseAnimator test = gameObject.GetComponent<BaseAnimator> ();
		test.playAnimation (animToPlay);
	}
}
