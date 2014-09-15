using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
	public MenuButton CurrentSelection;

	public float DelayTime = 0.2f;
	protected float m_Timer = 0.0f;

	protected Menu m_LastMenu;
	public Menu LastMenu
	{
		get { return m_LastMenu; }
		set { m_LastMenu = value; }
	}

	public GameObject CameraPos;

	public Enums.PlayerInput AcceptInputFrom = Enums.PlayerInput.All;

	// Use this for initialization
	void Start () 
	{
		setSelection(CurrentSelection);

		start();
	}

	protected virtual void start()
	{
	}
	
	// Update is called once per frame
	public virtual void update () 
	{
		m_Timer += Time.deltaTime;

		if( m_Timer >= DelayTime)
		{
			changeSelection();
		}

		if (InputManager.getJumpDown(AcceptInputFrom))
		{
			useButton();
		}
		else if (InputManager.getShowHudDown(AcceptInputFrom))
		{
			if(m_LastMenu != null)
			{
				MenuManager.Instance.LastMenu();
				m_LastMenu = null;
			}
		}

		setSelection(CurrentSelection);
	}

	protected virtual void useButton()
	{
		CurrentSelection.use();
	}

	protected virtual void changeSelection()
	{
		Vector2 selectionInput = InputManager.getMove(AcceptInputFrom);
		
		if( selectionInput.x != 0.0f || selectionInput.y != 0.0f)
		{
			m_Timer = 0.0f;

			float angle = Vector2.Angle(selectionInput, new Vector2(1.0f, 0.0f));
			
			if ( selectionInput.y < 0)
			{
				angle *= -1;
			}
			
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

			setSelection(nextSelection);
		}
	}
	
	void setSelection(MenuButton selection)
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
