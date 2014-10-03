using UnityEngine;
using System.Collections;

public class MenuSelectInput : Menu
{
    PlayerInputSelection m_PlayerOne = new PlayerInputSelection();
    PlayerInputSelection m_PlayerTwo = new PlayerInputSelection();

    public GameObject PlayerOneSelection;
    public GameObject PlayerTwoSelection;

    public GameObject GamePadPrefab;
    public GameObject KeyboardPrefab;


    public Menu NextMenu;

    protected override void start()
	{
		//used for inheritance
	}
	
	// Update is called once per frame
	public override void update () 
	{
        if (m_PlayerOne.IsReady && m_PlayerTwo.IsReady)
        {
            if (NextMenu != null)
            {
                MenuManager.Instance.changeMenu(NextMenu);
            }
            else
            {
                Debug.LogError("Adam you set this up wrong");
            }
        }
        else
        {
            m_PlayerOne = setInputSelection(m_PlayerOne, m_PlayerTwo);
            m_PlayerTwo = setInputSelection(m_PlayerTwo, m_PlayerOne);
        }
	}

    PlayerInputSelection setInputSelection(PlayerInputSelection playerToSet, PlayerInputSelection playerToNotSet)
    {
        if (playerToSet.IsReady)
        {
            if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
            {
                playerToSet.IsReady = false;
            }
        }
        else
        {
            if (playerToSet.isSet())
            {
                if (InputManager.getJumpDown())
                {
                    playerToSet.IsReady = true;
                }
                else
                {
                    if (InputManager.getShowHudDown(playerToSet.CurrentSelection))
                    {
                        playerToSet.CurrentSelection = PlayerInput.None;
                    }
                }
            }
            else
            {
                playerToSet = getInput(playerToSet, playerToNotSet);
            }
        }
        return playerToSet;
    }

    PlayerInputSelection getInput(PlayerInputSelection playerToSet, PlayerInputSelection playerToNotSet)
    {
        switch(playerToNotSet.CurrentSelection)
        {
            case PlayerInput.Keyboard:
                if (InputManager.getPauseDown(PlayerInput.GamePadOne))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadOne;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
                }
                break;

            case PlayerInput.GamePadOne:
                if (InputManager.getPauseDown(PlayerInput.Keyboard))
                {
                    playerToSet.CurrentSelection = PlayerInput.Keyboard;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
                }
                break;

            case PlayerInput.GamePadTwo:
                if (InputManager.getPauseDown(PlayerInput.Keyboard))
                {
                    playerToSet.CurrentSelection = PlayerInput.Keyboard;
                }
                else if (InputManager.getPauseDown(PlayerInput.GamePadOne))
                {
                    playerToSet.CurrentSelection = PlayerInput.GamePadOne;
                }
                break;

            default:
            if (InputManager.getPauseDown(PlayerInput.Keyboard))
            {
                playerToSet.CurrentSelection = PlayerInput.Keyboard;
            }
            else if (InputManager.getPauseDown(PlayerInput.GamePadOne))
            {
                playerToSet.CurrentSelection = PlayerInput.GamePadOne;
            }
            else if (InputManager.getPauseDown(PlayerInput.GamePadTwo))
            {
                playerToSet.CurrentSelection = PlayerInput.GamePadTwo;
            }
            break;
        }
        return playerToSet;

    }

    protected class PlayerInputSelection
    {
        PlayerInput m_CurrentSelection = PlayerInput.None;
        public PlayerInput CurrentSelection
        {
            get { return m_CurrentSelection; }
            set { m_CurrentSelection = value; }
        }

        bool m_IsReady = false;
        public bool IsReady
        {
            get { return m_IsReady; }
            set { m_IsReady = value; }
        }

        public bool isSet()
        {
            return m_CurrentSelection != PlayerInput.None;
        }

    }
}
