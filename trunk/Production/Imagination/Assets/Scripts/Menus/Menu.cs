/*
*Menu
*
*resposible for basic menu functionality
*
*can be implemented as a basic menu
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

public class Menu : MonoBehaviour 
{
    //the current selection of the menu (initial selection)
	public MenuButton CurrentSelection;

    //used to delay the swaping of the selected button 
    //other wise the buttons spaw way too fast
	public float DelayTime = 0.2f;
	protected float m_Timer = 0.0f;

	protected Menu m_LastMenu;
	public Menu LastMenu
	{
		get { return m_LastMenu; }
		set { m_LastMenu = value; }
	}

	public GameObject CameraPos;

	public PlayerInput AcceptInputFrom = PlayerInput.All;

	// Use this for initialization
	void Start () 
	{
        //makes sure the correct button state is shown
		setSelection(CurrentSelection);

		start();
	}

	protected virtual void start()
	{
        //used for inheritance
	}
	
	// Update is called once per frame
	public virtual void update () 
	{
        //update Timer
		m_Timer += Time.deltaTime;

		if( m_Timer >= DelayTime)
		{
            //change selection if needed
			changeSelection();
		}

        //check if button is being used
		if (InputManager.getJumpDown(AcceptInputFrom))
		{
			useButton();
		}
		else if (InputManager.getShowHudDown()) // check if "B" was hit
		{
			if(m_LastMenu != null)
			{
				MenuManager.Instance.LastMenu();
				m_LastMenu = null;
			}
		}
        //make sure the current selection is set
		setSelection(CurrentSelection);
	}

	protected virtual void useButton()
	{
        //call the buttons on use function
		CurrentSelection.use();
	}


	protected virtual void changeSelection()
	{
        //get the left stick input
		Vector2 selectionInput = InputManager.getMove(AcceptInputFrom);
		
        //if there was input
		if( selectionInput.x != 0.0f || selectionInput.y != 0.0f)
		{
            //reset the timer
			m_Timer = 0.0f;

            //convert the input into an angle
			float angle = Vector2.Angle(selectionInput, new Vector2(1.0f, 0.0f));
			
            //the conversion doesnt do reflex angles or negatives so adjust the value if needed
			if ( selectionInput.y < 0)
			{
				angle *= -1;
			}
			
            //the next button to be selected
			MenuButton nextSelection = null;

			if( angle < -157.5f)
			{
				//centre left
				nextSelection = CurrentSelection.getCentreLeftNeighbor();
			}
			else if( angle < -112.5f)
			{
				//bottom left
				nextSelection = CurrentSelection.getBottomLeftNeighbor();
			}
			else if (angle < -67.5f)
			{
				//bottom middle
				nextSelection = CurrentSelection.getBottomMiddleNeighbor();
			}
			else if (angle < -22.5f)
			{
				//bottom right
				nextSelection = CurrentSelection.getBottomRightNeighbor();
			}
			else if (angle < 22.5f)
			{
				//centre right
				nextSelection = CurrentSelection.getCentreRightNeighbor();
			}
			else if (angle < 67.5f)
			{
				//top right
				nextSelection = CurrentSelection.getTopRightNeighbor();
			}
			else if (angle < 112.5f)
			{
				//top middle
				nextSelection = CurrentSelection.getTopMiddleNeighbor();
			}
			else if (angle < 157.5f)
			{
				//top left
				nextSelection = CurrentSelection.getTopLeftNeighbor();
			}
			else
			{
				//centre left
				nextSelection = CurrentSelection.getCentreLeftNeighbor();
			}
            //set the selection
			setSelection(nextSelection);
		}
	}
	
	protected virtual void setSelection(MenuButton selection)
	{
		if( selection == null)
		{
			return;
		}
		//resets old selection and displays the current one to the user
		CurrentSelection.ButtonState = MenuButton.ButtonStates.Default;
		CurrentSelection = selection;
		CurrentSelection.ButtonState = MenuButton.ButtonStates.Highlightled;
	}
}
