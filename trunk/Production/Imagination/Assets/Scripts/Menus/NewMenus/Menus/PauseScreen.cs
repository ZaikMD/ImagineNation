using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseScreen : MenuV2
{
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (InputManager.getMenuStartDown())
        {
            for(int i = 0; i < TPCamera.Cameras.Count; i++)
            {
                TPCamera.Cameras[i].ShowShutter = !TPCamera.Cameras[i].ShowShutter;
            }
        }
	}
}
