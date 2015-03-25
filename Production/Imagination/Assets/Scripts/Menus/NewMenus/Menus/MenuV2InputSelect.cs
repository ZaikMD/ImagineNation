/*
 * Created by: Kris MAtis
 *  menu for multi player input selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made and commented
 */
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2InputSelect : MenuV2
{
    //the data to create the input slections
    public GameObject[] MountPoints;
    public GameObject[] SelectionPrefabs;
    public PlayerInput[] InputSelections;

    //the mount points and array to keep track of whats mounted where
    public GameObject[] PlayerSelectionMountPoints;
    protected int[] m_CurrentlyMounted = { -1, -1 };

    //the input selects
    protected InputSelectV2[] m_InputSelects;

    //the next menu
    public MenuV2 NextMenu;

    //what the delay is being modified by
    protected const float INPUT_DELAY_MODIFYER = 2.0f;

	public Texture Prompt;
	public Vector2 PromptLocation = new Vector2(0.5f, 0.0f);
	public Vector2 PromptSize = new Vector2(0.25f, 0.25f);

	// Use this for initialization
	protected override void start () 
    {
        //create the input selects
        m_InputSelects = new InputSelectV2[MountPoints.Length];

        for (int i = 0; i < m_InputSelects.Length; i++)
        {
            m_InputSelects[i] = new InputSelectV2(MountPoints[i].transform, (GameObject)GameObject.Instantiate(SelectionPrefabs[i], MountPoints[i].transform.position, MountPoints[i].transform.rotation), InputSelections[i]);
        }
	}

    protected override void update()
    {
        for (int i = 0; i < m_InputSelects.Length; i++)
        {
            //update each input select
            m_InputSelects[i].update();

            Vector2 moveInput = Vector2.zero;


            //update the timer
            m_Timer -= Time.deltaTime;

            if (m_Timer < 0.0f)
            {
                //get the move input
                moveInput = InputManager.getMenuChangeSelection(m_InputSelects[i].InputType);
                if (moveInput != Vector2.zero)
                {//we got input
                    //due to the lerping we need significantly more delay time than other menus
                    m_Timer = DELAY_TIME * INPUT_DELAY_MODIFYER;
                }
            }

            if (!m_InputSelects[i].IsMounted)
            {//not mounted, so see if we can mount the input
                unmountedBehavior(moveInput, i);
            }
            else
            {//mounted                
                mountedBehavior(moveInput, i);
            }
        }

        if (m_CurrentlyMounted[0] != -1 && m_CurrentlyMounted[1] != -1)
        {//two inputs mounted
            if (InputManager.getMenuAcceptDown())
            {//both players ready                
                changeMenu();                
            }
        }
    }

	void OnGUI()
	{
		if (m_CurrentlyMounted[0] != -1 && m_CurrentlyMounted[1] != -1)
		{//two inputs mounted
			GUI.DrawTexture(new Rect(Screen.width * PromptLocation.x, Screen.height * PromptLocation.y,
			                         Screen.width * PromptSize.x,     Screen.height * PromptSize.y), 
			                Prompt,
			                ScaleMode.ScaleToFit);
		}
	}

    protected virtual void mountedBehavior(Vector2 moveInput, int index)
    {
	    if (moveInput.x > 0 && m_CurrentlyMounted[0] == index) // trying to move right and on the left
	    {//unmount
	        m_CurrentlyMounted[0] = -1;
	        m_InputSelects[index].resetMountPoint();
	        return;
	    }
	    else if (moveInput.x < 0 && m_CurrentlyMounted[1] == index) // trying to move left and on the right
	    {//unmount
	        m_CurrentlyMounted[1] = -1;
	        m_InputSelects[index].resetMountPoint();
	        return;
	    }
	    else if(InputManager.getMenuBackDown(m_InputSelects[index].InputType))
	    {//unmount
	        m_CurrentlyMounted[0] = -1;
	        m_InputSelects[index].resetMountPoint();
	        return;
	    }
        
    }

    protected virtual void unmountedBehavior(Vector2 moveInput, int index)
    {
        if (moveInput.x > 0)
        {//right
            selectInput(index, 1);
        }
        else if (moveInput.x < 0)
        {//left
            selectInput(index, 0);
        }

        //not mounted so see if the player want to go to the last menu
        if (InputManager.getMenuBackDown(m_InputSelects[index].InputType))
        {//they hit back
            //reset this class and the input selects
            fullReset();

            //change the menu
            m_Camera.changeMenu(LastMenu);
            this.IsActiveMenu = false;
            LastMenu.IsActiveMenu = true;
            LastMenu = null;
        }
    }

    protected virtual void selectInput(int index, int mountPointIndex)
    {
        //if theres nothing mounted at the target mount the current index
        if (m_CurrentlyMounted[mountPointIndex] < 0)
        {
            m_CurrentlyMounted[mountPointIndex] = index;
            m_InputSelects[index].MountPoint = PlayerSelectionMountPoints[mountPointIndex].transform;
        }
    }

    protected virtual void fullReset()
    {
        //reset the input selects
        for (int i = 0; i < m_InputSelects.Length; i++)
        {
            m_InputSelects[i].fullReset();
        }
        //nothing should be mounted
        m_CurrentlyMounted[0] = -1;
        m_CurrentlyMounted[1] = -1;
    }

    //called when both players continue
    protected virtual void changeMenu()
    {
        //tell the camera to chan ge the menu
        m_Camera.changeMenu(NextMenu);
        //set the next menu to active
        NextMenu.LastMenu = this;
        this.IsActiveMenu = false;
        NextMenu.IsActiveMenu = true;

        //set game data's settings
        GameData.Instance.m_PlayerOneInput = m_InputSelects[m_CurrentlyMounted[0]].InputType;
        GameData.Instance.m_PlayerTwoInput = m_InputSelects[m_CurrentlyMounted[1]].InputType;

        fullReset();
    }
}
