/*
*CheckPoint
*
*resposible for informing GameData that a checkpoint might need changeing
*
*Created by: Kris Matis
*
*Edit: Oct 29th 2014- Adding a text message that pops up when the player activates the checkpoint -Greg Fortier
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 16/10/2014 Edit: Added in materials for the checkpoints
* 
* 28/11/2014 Edit: Removed anything to do with textures since the hud now takes care of it
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
	//Texture is for the player to acknowledge that they reached a checkpoint
	bool m_DrawGUI;
    bool m_WasUsed;

	float m_TimerGUI = 0.0f;
	float m_baseTimeValue = 5.0f;
	int m_DivideByTwo = 2;
	
	public GameObject m_ColorSection;

	public CheckPoints m_Value;

	SFXManager m_SFX;
	Hud m_Hud;

	//Lights to turn on
	SceneLights[] m_LightsToTurnOn;


	// Use this for initialization
	void Start ()
	{
		//m_ColorSection.renderer.material = m_OffMaterial;
		//gets sound manager
		m_SFX = SFXManager.Instance;
		//gets Hud
		m_Hud = GameObject.FindGameObjectWithTag(Constants.HUD).GetComponent<Hud>();

		m_DrawGUI = false;
        m_WasUsed = false;

		m_LightsToTurnOn = GetComponentsInChildren<SceneLights> ();
	}


	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == Constants.PLAYER_STRING)
		{
			if(!m_DrawGUI && !m_WasUsed)
			{
				m_TimerGUI = m_baseTimeValue;			

			    //Plays the collectable sound
				m_SFX.playSound(transform, Sounds.Checkpoint);
				m_Hud.ShowCheckpoint();

				GameData.Instance.CurrentCheckPoint = m_Value;

				//Turn lights on
				for (int i = 0; i < m_LightsToTurnOn.Length; i++)
				{
					m_LightsToTurnOn[i].SetLightActive(true);
				}

                m_DrawGUI = true;
                m_WasUsed = true;
            }
		}
	}
	
}
