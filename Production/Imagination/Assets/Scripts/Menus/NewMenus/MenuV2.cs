/*
*MenuV2
*
*resposible for basic menu functionality
*
*can be implemented as a basic menu
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 23/10/2014 Edit: no longer a stub
*
* 27/10/2014 edit: made the upate fully function based
 * 30/10/2014 edit made a variable proteceted
 * 
*/
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2 : MonoBehaviour 
{
    public bool IsStaringMenu = false;

    //is the menu the active one?
    protected bool m_IsActiveMenu = false;
    public bool IsActiveMenu
    {
        get { return m_IsActiveMenu; }
        set
        {
            m_IsActiveMenu = value;

            if (m_IsActiveMenu)
            {
                OnActivated();
            }
        }
    }
    //what inputs to read
    public PlayerInput[] ReadInputFrom;
    //the input to read combined (using |)
    protected int m_ReadInputFrom = 0;
    public int ReadInputFromBits
    {
        get { return m_ReadInputFrom; }
    }

    //current/ starting button
    public ButtonV2 m_CurrentButtonSelection = null;

    //timer for a delay when switching buttons
    protected const float DELAY_TIME = 0.25f;
    protected float m_Timer = 0.0f;

    //the previous menu
    protected MenuV2 m_LastMenu = null;
    public MenuV2 LastMenu
    {
        get { return m_LastMenu; }
        set { m_LastMenu = value; }
    }

    //the menu camera 
    protected static MenuCamera m_Camera;

    //the camera mount point
    public GameObject CameraMountPoint;

	// Use this for initialization
    protected virtual void Start() 
    {
        IsActiveMenu = IsStaringMenu;

        for (int i = 0; i < ReadInputFrom.Length; i++)
        {
            m_ReadInputFrom = m_ReadInputFrom | (int)ReadInputFrom[i];
        }

        m_Camera = Camera.main.GetComponent<MenuCamera>();
        if (m_Camera == null)
        {
            #if DEBUG || UNITY_EDITOR
                Debug.LogError("NO CAMERA FOUND");
            #endif
        }

        if (m_CurrentButtonSelection != null)
        {
            m_CurrentButtonSelection.ButtonState = ButtonV2.ButtonStates.Highlightled;
        }

        ButtonV2[] childButtons = gameObject.GetComponentsInChildren<ButtonV2>();
        for (int i = 0; i < childButtons.Length; i++)
        {
            childButtons[i].ParentMenu = this;
        }

        start();
	}

    protected virtual void start()
    {
        //used by inheriting classes to add functionality to start while keeping the old
    }

    // Update is called once per frame
    protected virtual void Update() 
    {
        //we dont update a menu that isnt the active one
        if (m_IsActiveMenu)
        {
            //dont update if the shutter is moving
            if (m_Camera.IsDoneShutterMove)
            {
                update();
            }
        }
	}

    /// <summary>
    /// main update call
    /// </summary>
    protected virtual void update()
    {
        //update Timer
        m_Timer += Time.deltaTime;

        if (m_Timer >= DELAY_TIME)
        {
            //change selection if needed
            changeSelection();
        }
       
		if (!useButton())
        {
            back();
        }       
    }

    protected virtual bool useButton()
    {
        PlayerInput inputRead;
        if (InputManager.getMenuAcceptDown(m_ReadInputFrom, out inputRead))//check if "A" was hit
        {
            if (m_CurrentButtonSelection != null)
            {
                m_CurrentButtonSelection.use(inputRead);
            }
            else
            {
#if DEBUG || UNITY_EDITOR
                Debug.LogError("NO BUTTON SET");
#endif
            }
            return true;
        }
        return false;
    }

    protected virtual void back()
    {
        if (InputManager.getMenuBackDown(m_ReadInputFrom)) // check if "B" was hit
        {
            if (m_LastMenu != null)
            {
                m_Camera.changeMenu(m_LastMenu);
                m_IsActiveMenu = false;
                m_LastMenu.IsActiveMenu = true;
                m_LastMenu = null;
            }
        }
    }

    protected virtual void changeSelection()
    {
        //get the change selection input
        Vector2 selectionInput = InputManager.getMenuChangeSelection(m_ReadInputFrom);

        //if there was input
        if (selectionInput.x != 0.0f || selectionInput.y != 0.0f)
        {
            //reset the timer
            m_Timer = 0.0f;

            //convert the input into an angle
            float angle = Vector2.Angle(selectionInput, new Vector2(1.0f, 0.0f));

            //the conversion doesnt do reflex angles or negatives so adjust the value if needed
            if (selectionInput.y < 0)
            {
                angle *= -1;
            }

            //the next button to be selected
            ButtonV2 nextSelection = null;

            if (angle < -157.5f)
            {
                //centre left
                nextSelection = m_CurrentButtonSelection.getCentreLeftNeighbor();
            }
            else if (angle < -112.5f)
            {
                //bottom left
                nextSelection = m_CurrentButtonSelection.getBottomLeftNeighbor();
            }
            else if (angle < -67.5f)
            {
                //bottom middle
                nextSelection = m_CurrentButtonSelection.getBottomMiddleNeighbor();
            }
            else if (angle < -22.5f)
            {
                //bottom right
                nextSelection = m_CurrentButtonSelection.getBottomRightNeighbor();
            }
            else if (angle < 22.5f)
            {
                //centre right
                nextSelection = m_CurrentButtonSelection.getCentreRightNeighbor();
            }
            else if (angle < 67.5f)
            {
                //top right
                nextSelection = m_CurrentButtonSelection.getTopRightNeighbor();
            }
            else if (angle < 112.5f)
            {
                //top middle
                nextSelection = m_CurrentButtonSelection.getTopMiddleNeighbor();
            }
            else if (angle < 157.5f)
            {
                //top left
                nextSelection = m_CurrentButtonSelection.getTopLeftNeighbor();
            }
            else
            {
                //centre left
                nextSelection = m_CurrentButtonSelection.getCentreLeftNeighbor();
            }
            //set the selection
            setSelection(nextSelection);
        }
    }

    protected virtual void setSelection(ButtonV2 selection)
    {
        if (selection == null)
        {
            return;
        }

        //resets old selection and displays the current one to the user
        m_CurrentButtonSelection.ButtonState = ButtonV2.ButtonStates.Default;
        m_CurrentButtonSelection = selection;
        m_CurrentButtonSelection.ButtonState = ButtonV2.ButtonStates.Highlightled;
    }

    protected virtual void OnActivated()
    {
    }

    public void setInputsToRead(int inputs)
    {
        m_ReadInputFrom = inputs;
    }
}