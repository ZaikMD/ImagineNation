using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float timer;

	// Update is called once per frame
	void Update ()
	{
        if (PauseScreen.IsGamePaused) { return; }

		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
