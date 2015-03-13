/*
 * Created by: Kris MAtis
 * menu for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made
 * 
 * 28/10/2014 edit: what i did while sick was terrible so im resarting
 * 28/10/2014 edit: mostly done
 * 28/10/2014 edit: finished debuging base functionality is in exept for saving the settings
 * 
 * 29/10/2014 edit: added resets
 * 29/10/2014 edit: can load into the level now
 * 29/10/2014 edit: put the stuff for instructions in (not tested yet)
 * 
 * 30/10/2014 edit: added back and fixed a null check
 */
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2PlayerSelect : MenuV2 
{
    const int PLAYER_ONE = 0;
    const int PLAYER_TWO = 1;

    public PlayerSelectArrow[] PlayerArrows;

    public string SceneToLoad;

    new const float DELAY_TIME = 0.5f;
    float m_PlayerOneTimer = 0.0f;
    float m_PlayerTwoTimer = 0.0f;


    struct input
    {
        public Vector2 Move;
        public bool Accept;
        public bool Back;

        public input(Vector2 move, bool accept, bool back)
        {
            Move = move;
            Accept = accept;
            Back = back;
        }
    };

	protected override void start ()
	{
		base.start ();

		PlayerArrows [0].m_Player = 0;
		PlayerArrows [1].m_Player = 1;
	}


    // Update is called once per frame
    protected override void  update()
    {
        for (int i = 0; i < PlayerArrows.Length; i++)
        {
            //get the input
            input input = new input(Vector2.zero, false, false);

            m_PlayerOneTimer += Time.deltaTime;
            m_PlayerTwoTimer += Time.deltaTime;

            if (i == PLAYER_ONE)
            {
                input.Move = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerOneInput);
                input.Accept = InputManager.getMenuAcceptDown(GameData.Instance.m_PlayerOneInput);
                input.Back = InputManager.getMenuBackDown(GameData.Instance.m_PlayerOneInput);

                if (input.Move != Vector2.zero)
                {
                    if (m_PlayerOneTimer < DELAY_TIME)
                    {
                        input.Move = Vector2.zero;
                    }
                    else
                    {
                        m_PlayerOneTimer = 0.0f;
                    }
                }
            }
            else if (i == PLAYER_TWO)
            {
                input.Move = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerTwoInput);
                input.Accept = InputManager.getMenuAcceptDown(GameData.Instance.m_PlayerTwoInput);
                input.Back = InputManager.getMenuBackDown(GameData.Instance.m_PlayerTwoInput);

                if (input.Move != Vector2.zero)
                {
                    if (m_PlayerTwoTimer < DELAY_TIME)
                    {
                        input.Move = Vector2.zero;
                    }
                    else
                    {
                        m_PlayerTwoTimer = 0.0f;
                    }
                }
            }       


            if (!PlayerArrows[i].IsMounted)
            {
                //check if moving
                if (input.Move.y > 0.0f)
                {
                    PlayerArrows[i].moveUp();
                    return;
                }
                else if (input.Move.y < 0.0f)
                {
                    PlayerArrows[i].moveDown();
                    return;
                }


                //check if mounting
                if (input.Move.x != 0.0f)
                {
                    Vector2 middleToMount = new Vector2((PlayerArrows[i].SelectionMountPoint.position - PlayerArrows[i].transform.position).x, 0.0f);

                    if (Vector2.Dot(input.Move.normalized, middleToMount.normalized) > 0.0f)
                    {//player swiped towards thier side
                        if (!PlayerArrows[i].IsMounted)
                        {
                            PlayerArrows[i].select();
                            return;
                        }
                    }
                }

                back();
                if (!m_IsActiveMenu)
                {
                    return;
                }
            }
            else
            {                
	            //check if unmounting
	            if (input.Move.x != 0.0f)
	            {
	                Vector2 middleToMount = new Vector2((PlayerArrows[i].SelectionMountPoint.position - PlayerArrows[i].transform.position).x, 0.0f);

	                if (Vector2.Dot(input.Move.normalized, middleToMount.normalized) < 0.0f)
	                {//player swiped away from their side
	                    if (PlayerArrows[i].IsMounted)
	                    {
	                        PlayerArrows[i].deselect();
	                    }
	                    return;
	                }
	            }                
            }
        }

		if (PlayerArrows[PLAYER_ONE].getSelection() != PlayerArrows[PLAYER_TWO].getSelection()
		    && PlayerArrows[PLAYER_ONE].IsMounted && PlayerArrows[PLAYER_TWO].IsMounted 
		    && InputManager.getMenuAcceptDown())
        {
            //set the characters
            GameData.Instance.PlayerOneCharacter = PlayerArrows[PLAYER_ONE].getCharacterSetting();
            GameData.Instance.PlayerTwoCharacter = PlayerArrows[PLAYER_TWO].getCharacterSetting();            

            //reset the menu
            for (int i = 0; i < PlayerArrows.Length; i++)
            {
                PlayerArrows[i].reset();
            }

            //go to next scene
            if (string.Compare(SceneToLoad, "") != 0)
            {
                Application.LoadLevel(SceneToLoad);
            }
        }
    }

    protected override void back()
    {
        if (InputManager.getMenuBackDown((int)GameData.Instance.m_PlayerOneInput | (int)GameData.Instance.m_PlayerTwoInput))
        {
            m_Camera.changeMenu(m_LastMenu);
            m_IsActiveMenu = false;
            m_LastMenu.IsActiveMenu = true;
            m_LastMenu = null;

            //reset the menu
            for (int i = 0; i < PlayerArrows.Length; i++)
            {
                PlayerArrows[i].reset();
            }
        }
    }

}
