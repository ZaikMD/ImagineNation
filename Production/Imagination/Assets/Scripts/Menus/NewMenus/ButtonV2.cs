/*
*ButtonV2
*
*has button functionality and keeps track of its neighboring buttons
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 23/10/2014 Edit: no longer a stub, based heavily on the original with some additional functionality added
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public abstract class ButtonV2 : MonoBehaviour 
{
    //the game objects displayed for each state
    public GameObject Default;
    public GameObject Disabled;
    public GameObject Highlighted;

    MenuV2 m_ParentMenu;
    public MenuV2 ParentMenu 
    {
        get { return m_ParentMenu; }
        set { m_ParentMenu = value; }
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
        get { return m_ButtonState; }
        set
        {
            //set the current state's gameobject to active and disable the others
            switch (value)
            {
                case ButtonStates.Default:
                    {
                        if (m_ButtonState != ButtonStates.Disabled)
                        {
                            if (Disabled != null)
                            {
                                Disabled.SetActive(false);
                            }

                            if (Highlighted != null)
                            {
                                Highlighted.SetActive(false);
                            }

                            if (Default != null)
                            {
                                Default.SetActive(true);
                            }
                        }
                        break;
                    }
                case ButtonStates.Highlightled:
                    {
                        if (m_ButtonState != ButtonStates.Disabled)
                        {
                            if (Default != null)
                            {
                                Default.SetActive(false);
                            }

                            if (Disabled != null)
                            {
                                Disabled.SetActive(false);
                            }

                            if (Highlighted != null)
                            {
                                Highlighted.SetActive(true);
                            }
                        }
                        break;
                    }

                case ButtonStates.Disabled:
                    {
                        if (Disabled != null)
                        {
                            Disabled.SetActive(true);
                        }

                        if (Highlighted != null)
                        {
                            Highlighted.SetActive(false);
                        }

                        if (Default != null)
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
        switch (m_ButtonState)
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
    public ButtonV2 TopLeftNeighbor = null;
    public ButtonV2 TopMiddleNeighbor = null;
    public ButtonV2 TopRightNeighbor = null;

    public ButtonV2 CentreLeftNeighbor = null;
    public ButtonV2 CentreRightNeighbor = null;

    public ButtonV2 BottomLeftNeighbor = null;
    public ButtonV2 BottomMiddleNeighbor = null;
    public ButtonV2 BottomRightNeighbor = null;

    //---------------------------------------------------
    //top row of buttons

    public ButtonV2 getTopLeftNeighbor()
    {
        if (TopLeftNeighbor == null || TopLeftNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return TopLeftNeighbor;
        }
        else
        {
            return TopLeftNeighbor.getTopLeftNeighbor();
        }        
    }

    public ButtonV2 getTopMiddleNeighbor()
    {
        if (TopMiddleNeighbor == null || TopMiddleNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return TopMiddleNeighbor;
        }
        else
        {
            return TopMiddleNeighbor.getTopMiddleNeighbor();
        }
    }

    public ButtonV2 getTopRightNeighbor()
    {
        if (TopRightNeighbor == null || TopRightNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return TopRightNeighbor;
        }
        else
        {
            return TopRightNeighbor.getTopRightNeighbor();
        } 
    }

    //---------------------------------------------------
    //centre row of buttons
    public ButtonV2 getCentreLeftNeighbor()
    {
        if (CentreLeftNeighbor == null || CentreLeftNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return CentreLeftNeighbor;
        }
        else
        {
            return CentreLeftNeighbor.getCentreLeftNeighbor();
        } 
    }

    public ButtonV2 getCentreRightNeighbor()
    {
        if (CentreRightNeighbor == null || CentreRightNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return CentreRightNeighbor;
        }
        else
        {
            return CentreRightNeighbor.getCentreRightNeighbor();
        } 
    }

    //---------------------------------------------------
    //bottom row of buttons
    public ButtonV2 getBottomLeftNeighbor()
    {
        if (BottomLeftNeighbor == null || BottomLeftNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return BottomLeftNeighbor;
        }
        else
        {
            return BottomLeftNeighbor.getBottomLeftNeighbor();
        } 
    }

    public ButtonV2 getBottomMiddleNeighbor()
    {
        if (BottomMiddleNeighbor == null || BottomMiddleNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return BottomMiddleNeighbor;
        }
        else
        {
            return BottomMiddleNeighbor.getBottomMiddleNeighbor();
        } 
    }

    public ButtonV2 getBottomRightNeighbor()
    {
        if (BottomRightNeighbor == null || BottomRightNeighbor.ButtonState != ButtonStates.Disabled)
        {
            return BottomRightNeighbor;
        }
        else
        {
            return BottomRightNeighbor.getBottomRightNeighbor();
        } 
    }

    //--------------------------------------------------
    //the function that 
    public abstract void use();
}
