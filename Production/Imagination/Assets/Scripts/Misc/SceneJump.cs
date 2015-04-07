using UnityEngine;
using System.Collections;

public class SceneJump : MonoBehaviour 
{
	bool approved = false;
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.Minus) || Input.GetKey (KeyCode.KeypadMinus))
						approved = true;
				else
						approved = false;

		if (!approved)
			return;

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			GameData.Instance.CurrentSection = Sections.Sections_1;
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			GameData.Instance.CurrentSection = Sections.Sections_2;
		}

		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			GameData.Instance.CurrentSection = Sections.Sections_3;
		}

		//==============================================================================

		if(Input.GetKeyDown(KeyCode.F1))
		{
			Application.LoadLevel("Menusv2");
		}

		if(Input.GetKeyDown(KeyCode.F12))
		{
			Application.LoadLevel("Boss Animation");
		}

		//==============================================================================

		if(Input.GetKeyDown(KeyCode.Keypad1))
		{
			GameData.Instance.CurrentSection = GameData.Instance.CurrentSection;
			GameData.Instance.CurrentCheckPoint = CheckPoints.CheckPoint_1;
		}

		if(Input.GetKeyDown(KeyCode.Keypad2))
		{
			GameData.Instance.CurrentSection = GameData.Instance.CurrentSection;
			GameData.Instance.CurrentCheckPoint = CheckPoints.CheckPoint_2;
		}

		if(Input.GetKeyDown(KeyCode.Keypad3))
		{
			if(GameData.Instance.CurrentSection != Sections.Sections_3)
				return;

			GameData.Instance.CurrentSection = GameData.Instance.CurrentSection;
			GameData.Instance.CurrentCheckPoint = CheckPoints.CheckPoint_3;
		}

		if(Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
		{
			Application.LoadLevel("LoadingScreen");
		}
	}
}
