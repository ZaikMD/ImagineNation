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

public class CheckPoint : MonoBehaviour 
{
	//Texture is for the player to acknowledge that they reached a checkpoint
	public GUITexture m_Texture;
	public Vector2 TextureScale =  new Vector2(1.0f, 1.0f);
	bool m_DrawGUI;
    bool m_WasUsed;

	float m_TimerGUI = 0.0f;
	float m_baseTimeValue = 5.0f;
	int m_DivideByTwo = 2;


	public Material m_OffMaterial;
	public Material m_OnMaterial;
	public GameObject m_ColorSection;

	public CheckPoints m_Value;

	SFXManager m_SFX;
	Hud m_Hud;

	// Use this for initialization
	void Start ()
	{
		m_ColorSection.renderer.material = m_OffMaterial;
		//gets sound manager
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
		//gets Hud
		m_Hud = GameObject.FindGameObjectWithTag(Constants.HUD).GetComponent<Hud>();

		m_DrawGUI = false;
        m_WasUsed = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
        updateGUI();	
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == Constants.PLAYER_STRING)
		{

			if(!m_DrawGUI && !m_WasUsed)
			{
				m_TimerGUI = m_baseTimeValue;			

			    //Plays the collectable sound
			    m_SFX.playSound(this.gameObject, Sounds.Collectable);
				m_Hud.ShowCheckpoint();

				GameData.Instance.CurrentCheckPoint = m_Value;
			    m_ColorSection.renderer.material = m_OnMaterial;

                m_DrawGUI = true;
                m_WasUsed = true;
            }
		}
	}

	void updateGUI()
	{
        if (m_DrawGUI)
        {
            if (m_TimerGUI > 0.0f)
            {
                m_Texture.transform.parent.gameObject.GetComponentInChildren<GUITexture>();
                m_Texture.pixelInset = new Rect(0, 0, (m_Texture.texture.width * TextureScale.x) / m_DivideByTwo, (m_Texture.texture.height * TextureScale.y) / m_DivideByTwo);
                m_TimerGUI -= Time.deltaTime;
                m_Texture.enabled = true;
   //             Debug.Log(m_TimerGUI);
            }
            else
            {
                m_DrawGUI = false;
                //m_TimerGUI = m_baseTimeValue;
                m_Texture.enabled = false;
            }
        }
	}
}
