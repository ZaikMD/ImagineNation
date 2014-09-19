using UnityEngine;
using System.Collections;

public abstract class MenuButton : MonoBehaviour 
{
	public GameObject Default;
	public GameObject Disabled;
	public GameObject Highlighted;

	protected Menu m_ParentMenu;
	public Menu ParentMenu
	{
		get{ return m_ParentMenu; }
		set{ m_ParentMenu = value; }
	}

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
	//	m_ParentMenu = (Menu)gameObject.GetComponentInParent(typeof(Menu));

		start();
	}

	protected virtual void start()
	{
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
	}

	protected virtual void defaultState()
	{
	}

	protected virtual void highlightedState()
	{
	}

	protected virtual void disabledState()
	{
	}

	public MenuButton TopLeftNeighbor = null;
	public MenuButton TopMiddleNeighbor = null;
	public MenuButton TopRightNeighbor = null;
	
	public MenuButton CentreLeftNeighbor = null;
	public MenuButton CentreRightNeighbor = null;
	
	public MenuButton BottomLeftNeighbor = null;
	public MenuButton BottomMiddleNeighbor = null;
	public MenuButton BottomRightNeighbor = null;

	//---------------------------------------------------
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
	public MenuButton getCentreLeftNeighbor()
	{
		return CentreLeftNeighbor;
	}
	
	public MenuButton getCentreRightNeighbor()
	{
		return CentreRightNeighbor;
	}

	//---------------------------------------------------
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

	public abstract void use();
}
