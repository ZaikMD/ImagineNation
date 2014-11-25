using UnityEngine;
using System.Collections;

public class TEST_AutoLoad : MonoBehaviour 
{
	public string scene;
	public float timer = 2.0f;

	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;
		if(timer < 0)
		{
			Application.LoadLevel(scene);
		}
	}
}
