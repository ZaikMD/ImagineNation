using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float timer;
    const ScriptPauseLevel PAUSE_LEVEL = ScriptPauseLevel.Cutscene;

	// Update is called once per frame
	void Update ()
	{
        if (PauseScreen.shouldPause(PAUSE_LEVEL)) { return; }

		timer -= Time.deltaTime;

		if(timer <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
