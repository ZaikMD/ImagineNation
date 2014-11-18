/*
*MenuButton
*
*base class for buttons, has basic button functionality
*
*keeps track of the buttons neighboring buttons
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public abstract class MenuButton : MonoBehaviour 
{
    //the game objects displayed for each state
	public GameObject Default;
	public GameObject Disabled;
	public GameObject Highlighted;

	public enum ButtonStates
	{
		Default = 0,
		Highlightled,
		Disabled
	};

	protected ButtonStates m_ButtonState;
	public ButtonStates ButtonState
	{
		get{ return m_ButtonState; }
		set
		{ 
            //set the current state's gameobject to active and disable the others
			switch(value)
			{
			case ButtonStates.Default:
			{
				if(m_ButtonState != ButtonStates.Disabled)
				{
					if(Disabled != null)
					{
						Disabled.SetActive(false);
					}

					if(Highlighted != null)
					{
						Highlighted.SetActive(false);
					}

					if(Default != null)
					{
						Default.SetActive(true);
					}
				}
				break;
			}
			case ButtonStates.Highlightled:
			{
				if(m_ButtonState != ButtonStates.Disabled)
				{
					if(Default != null)
					{
						Default.SetActive(false);
					}
				
					if(Disabled != null)
					{
						Disabled.SetActive(false);
					}

					if(Highlighted != null)
					{
						Highlighted.SetActive(true);
					}
				}
				break;
			}

			case ButtonStates.Disabled:
			{
				if(Disabled != null)
				{
					Disabled.SetActive(true);
				}
				
				if(Highlighted != null)
				{
					Highlighted.SetActive(false);
				}
				
				if(Default != null)
				{
					Default.SetActive(false);
				}
				
				break;
			}
			}
			m_ButtonState = value; 
		}
	}

	void Start()
	{
        ButtonState = m_ButtonState;
		start();
	}

	protected virtual void start()
	{
        //used for inheritance
	}

	void Update()
	{
		switch(m_ButtonState)
		{
			case ButtonStates.Default:
			{
				defaultState();
				break;
			}
			case ButtonStates.Highlightled:
			{
				highlightedState();
				break;
			}
			case ButtonStates.Disabled:
			{
				disabledState();
				break;
			}
		}
		update();
	}

	protected virtual void update()
	{
        //used for inheritance
	}

	protected virtual void defaultState()
    {
        //used for inheritance
	}

	protected virtual void highlightedState()
    {
        //used for inheritance
	}

	protected virtual void disabledState()
    {
        //used for inheritance
	}

    //the neighboring buttons  (in the same menu)
	public MenuButton TopLeftNeighbor = null;
	public MenuButton TopMiddleNeighbor = null;
	public MenuButton TopRightNeighbor = null;
	
	public MenuButton CentreLeftNeighbor = null;
	public MenuButton CentreRightNeighbor = null;
	
	public MenuButton BottomLeftNeighbor = null;
	public MenuButton BottomMiddleNeighbor = null;
	public MenuButton BottomRightNeighbor = null;

	//---------------------------------------------------
    //top row of buttons

    public MenuButton getTopLeftNeighbor()
	{
		return TopLeftNeighbor;
	}

	public MenuButton getTopMiddleNeighbor()
	{
		return TopMiddleNeighbor;
	}

	public MenuButton getTopRightNeighbor()
	{
		return TopRightNeighbor;
	}

	//---------------------------------------------------
	//centre row of buttons
    public MenuButton getCentreLeftNeighbor()
	{
		return CentreLeftNeighbor;
	}
	
	public MenuButton getCentreRightNeighbor()
	{
		return CentreRightNeighbor;
	}

	//---------------------------------------------------
	//bottom row of buttons
    public MenuButton getBottomLeftNeighbor()
	{
		return BottomLeftNeighbor;
	}
	
	public MenuButton getBottomMiddleNeighbor()
	{
		return BottomMiddleNeighbor;
	}
	
	public MenuButton getBottomRightNeighbor()
	{
		return BottomRightNeighbor;
	}

    //--------------------------------------------------
    //the function that 
	public abstract void use();
}
