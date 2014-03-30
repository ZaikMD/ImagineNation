using UnityEngine;
using System.Collections;

public class SwitchTest : MonoBehaviour {

	public CameraController camera1;

	public Transform p1;
	public Transform p2;

	bool player1Active = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.U))
		{
			if (player1Active)
			{
				camera1.switchTo (p2);
				player1Active = false;
			}
			else
			{
				camera1.switchTo (p1);
				player1Active = true;
			}
		}

		if (Input.GetKeyDown ("x"))
		{
			camera1.toggleAiming ( );
		}
	}
}
