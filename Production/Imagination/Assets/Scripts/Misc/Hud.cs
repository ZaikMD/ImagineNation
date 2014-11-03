using UnityEngine;
using System.Collections;

/* Created by: kole
 * 
 * this class is will display all the hud elements to the player
 * when they are needed to be shown, such as how many collectables
 * has the player found and player health
 * 
 */


public class Hud : MonoBehaviour {

    bool m_ShowCheckPoint;
    bool m_ShowHiddenHud;

   //Time for showing and hiding hud
    float m_HudDisplayLength;
    float m_HudDisplayTimer;
    float m_CheckPointDisplayTimer;

    //Varibles to display
    //health
    int LightPegCollected;
    int PuzzlePiecesCollected;

	//Images for our hud
    public Texture m_LightPegHudImage;
    public Texture m_PuzzlePieceHudNoneImage;
    public Texture m_PuzzlePieceHudOneImage;
    public Texture m_PuzzlePieceHudTwoImage;
    public Texture m_PuzzlePieceHudThreeImage;
    public Texture m_PuzzlePieceHudFourImage;
    public Texture m_PuzzlePieceHudFiveImage;
    public Texture m_PuzzlePieceHudSixImage;
    public Texture m_LifeCounterImage;
    public Texture m_CheckpointImage;
   
    //Font for numbers
    public Font m_NumberFont;


    // Use this for initialization
	void Start ()
    {
        //Setting all varibles to desired starting stat
        m_HudDisplayLength = Constants.HUD_ON_SCREEN_TIME;
        m_HudDisplayTimer = 0;
        m_CheckPointDisplayTimer = 0;
        m_ShowCheckPoint = false;
        m_ShowHiddenHud = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(m_HudDisplayTimer);
        if (m_HudDisplayTimer > 0)
        {
            m_HudDisplayTimer -= Time.deltaTime;
        }
        else
        {
            m_HudDisplayTimer = 0;
            m_ShowHiddenHud = false;
        }

        if (m_CheckPointDisplayTimer > 0)
        {
            m_CheckPointDisplayTimer -= Time.deltaTime;
        }
        else
        {
            m_CheckPointDisplayTimer = 0;
            m_ShowCheckPoint = false;
        }
	}

    public void ShowCheckpoint()
    {
        m_ShowCheckPoint = true;
        m_CheckPointDisplayTimer = m_HudDisplayLength;
    }

    public void ShowHiddenHud()
    {
        m_ShowHiddenHud = true;
        m_HudDisplayTimer = m_HudDisplayLength;
    }

    public void UpdateLightPegs(int NumberOfLightPegs)
    {
        LightPegCollected = NumberOfLightPegs;
        ShowHiddenHud();
    }

    //All our graphics have to be done in on gui
    void OnGUI()
    {

        if(m_ShowHiddenHud)
        {
            //our health will be about a tenth of the screen wide and tall
            float SizeOfHudElements = Screen.width / 10;
            Rect PositionRect = new Rect(0, 0, SizeOfHudElements, SizeOfHudElements);
          //GUI.skin.font = m_NumberFont;
            
         //GUI.DrawTexture(PositionRect, m_LightPegHudImage);

            PositionRect.Set(PositionRect.width, 0, SizeOfHudElements, SizeOfHudElements);
            if (LightPegCollected < 10)
            {
                GUI.Label(PositionRect, "0" + LightPegCollected.ToString());
            }
            else
            {
                GUI.Label(PositionRect, LightPegCollected.ToString());
            }

            PositionRect.x = Screen.width - SizeOfHudElements;         
        }
    }
}
