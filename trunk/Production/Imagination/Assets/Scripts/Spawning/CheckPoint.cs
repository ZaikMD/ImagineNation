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
*/
#endregion

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class CheckPoint : MonoBehaviour 
{
	//Texture is for the player to acknowledge that they reached a checkpoint
	GUITexture m_Texture;
	public Vector2 TextureScale =  new Vector2(1.0f, 1.0f);
	bool m_DrawGUI;
	float m_TimerGUI = 3.0f;
	float m_baseTimeValue;


	public Material m_OffMaterial;
	public Material m_OnMaterial;
	public GameObject m_ColorSection;

	public CheckPoints m_Value;

	SFXManager m_SFX;

	// Use this for initialization
	void Start ()
	{
		m_baseTimeValue = m_TimerGUI;
		m_ColorSection.renderer.material = m_OffMaterial;
		//gets sound manager
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();

		m_DrawGUI = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == Constants.PLAYER_STRING)
		{
			//Plays the collectable sound
			m_SFX.playSound(this.gameObject, Sounds.Collectable);
			GameData.Instance.CurrentCheckPoint = m_Value;
			m_ColorSection.renderer.material = m_OnMaterial;
			m_DrawGUI = true;

		}
	}

	void OnGui()
	{
		if(m_DrawGUI)
		{
			if(m_TimerGUI >=0)
			{
				m_Texture = gameObject.GetComponent<GUITexture> ();
				m_Texture.pixelInset = new Rect (0, 0, m_Texture.texture.width * TextureScale.x, m_Texture.texture.height * TextureScale.y);
				m_TimerGUI -= Time.deltaTime;
			}

			else 
			{
				m_DrawGUI = false;
				m_TimerGUI = m_baseTimeValue;
			}
		}

	}
}
