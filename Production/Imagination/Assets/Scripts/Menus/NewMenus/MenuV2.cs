using UnityEngine;
using System.Collections;

public class MenuV2 : MonoBehaviour 
{
    public bool IsActiveMenu = false;

    public PlayerInput[] ReadInputFrom;
    int m_ReadInputFrom = 0;

    bool m_IsReadingInput = true;
    public bool IsReadingInput
    {
        get { return m_IsReadingInput; }
        set { m_IsReadingInput = value; }
    }

    public ButtonV2 m_CurrentButton = null;
    
	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < ReadInputFrom.Length; i++)
        {
            m_ReadInputFrom = m_ReadInputFrom | (int)ReadInputFrom[i];
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsActiveMenu)
        {
            if (m_IsReadingInput)
            {
                update();
            }
        }
	}

    /// <summary>
    /// main update call
    /// </summary>
    void update()
    {
        //if(changeSelection)
        {
            //change button
        }

        //if(use)
        {
            //use button
        }
    }
}

/*

*MenuManagerV2
*
* resposible for updating the current menu
*
* moveing the camera to the menus mount point
*
* change menu camera animations
* the shutter for menu changing
*
*Created by: Kris Matis


using UnityEngine;
using System.Collections;

public class MenuManagerV2 : MonoBehaviour
{
    #region Singleton
    public static MenuManagerV2 Instance { get; private set; }

    //ID is used to ensure that the older GamaData is kept in the case of duplicates
    static int m_InstanceCounter = 0;
    int m_ID = int.MaxValue;
    public int ID
    {
        get { return m_ID; }
        private set { m_ID = value; }
    }

    void Awake()
    {
        ID = m_InstanceCounter++;

        //if theres another instance (there shouldnt be) destroy this
        if (Instance != null && Instance != this)
        {
            if (ID > Instance.ID)
            {
                //destroy all other instances
                Destroy(gameObject);
            }
            else
            {
                Destroy(Instance.gameObject);
            }
        }

        //set the instance
        Instance = this;
    }
    #endregion

    //the main camera
    Camera m_MainCamera;

    /// <summary>
    /// the menu to be updated should be set to the starting menu from unity
    /// </summary>
    public MenuV2 StartMenu = null;
    MenuV2 m_CurrentMenu = null;
    MenuV2 m_NextMenu = null;

    public GUITexture Shutter;
    GUITexture m_Shutter;

    bool ShutterUp = false;
    bool Teleported = false;
    bool ShutterDown = false;

    const float SHUTTER_SPEED = 0.5f;
    const float SHUTTER_UP_DELAY = 0.3f;
    float m_Timer = SHUTTER_SPEED;

    // Use this for initialization
	void Start ()
    {
        #region Camera
        //set the main camera
        m_MainCamera = Camera.main;

        #if DEBUG || UNITY_EDITOR
            if (m_MainCamera == null)
            {
                Debug.LogError("No Main Camera");
                Debug.Break();
            }
        #endif
        #endregion
        #region Start Menu
        m_CurrentMenu = StartMenu;
        #if DEBUG || UNITY_EDITOR
            if (m_CurrentMenu == null)
            {
                Debug.LogError("No Start Menu");
                Debug.Break();
            }
        #endif
        #endregion
        #region Shutter
            m_Shutter = Shutter;
            #if DEBUG || UNITY_EDITOR
                Debug.LogError("no shutter");
                Debug.Break();
            #endif
        #endregion

        #region Change Menu Animation
        //m_ChangeMenuAnimation = new ChangeMenuAnimation(this);
        #endregion

        resetShutterVariables();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (m_NextMenu == null)
        {
            //TODO: update current menu here
        }
        else
        {
            
            if(!getIsShutterDone())
            {
                //update the change animation
                updateShutter();
            }
            else
            {
                changeMenu();
            }
        }
	}

    public void changeMenu(MenuV2 nextMenu)
    {
        m_NextMenu = nextMenu;
        #if DEBUG || UNITY_EDITOR
            if (m_NextMenu == null)
            {
                Debug.LogError("no menu passed to this function");
                Debug.Break();
            }
        #endif
    }

    void changeMenu()
    {
        m_CurrentMenu = m_NextMenu;
        m_NextMenu = null;
        resetShutterVariables();
    }

        
    void updateShutter()
    {
        if (!ShutterUp)
        {
            //rotate texture over screen

            float rotationAmount = Mathf.Clamp01(m_Timer / SHUTTER_SPEED);
            rotationAmount = 1 - rotationAmount;

        }
        else if (!Teleported)
        {
            //teleprt camera here

            m_Timer = SHUTTER_SPEED + SHUTTER_UP_DELAY;
        }
        else if (!ShutterDown)
        {
            //rotate shuter down here

            float rotationAmount = Mathf.Clamp01(m_Timer / SHUTTER_SPEED);
        }
    }

    void resetShutterVariables()
    {
        ShutterUp = false;
        Teleported = false;
        ShutterDown = false;

        m_Timer = SHUTTER_SPEED;
    }

    bool getIsShutterDone()
    {
        return ShutterUp || Teleported || ShutterDown;
    }
}



/*


	

	//------------------------------------------------------------------------------

	const string MENU_STRING = "Menu";
	const string SPLASH_SCREEN_STRING = "Splash Screen";

	Menu m_CurrentMenu;

	Camera MainCamera;

	public float CameraMoveSpeed = 0.05f;
	public float CameraRotationSpeed = 0.05f;

	bool m_RotatingMenu;

	// Use this for initialization
	void Start () 
	{
		//get the camera
		MainCamera = (Camera) GameObject.FindGameObjectWithTag(Constants.MAIN_CAMERA_STRING).GetComponent(typeof(Camera));

		if(MainCamera == null)
		{
			#if DEBUG || UNITY_EDITOR
				Debug.LogError("No Main Camera Found");
			#endif
		}

		//get all the menu game objects
		GameObject[] menus = GameObject.FindGameObjectsWithTag(MENU_STRING);
		if(menus.Length != 0)
		{
			Menu[] Menus;
			Menus = new Menu[menus.Length];
			int i = 0;
			do
			{
				//get the menu object
				Menus[i] = (Menu) menus[i].GetComponent(typeof(Menu));//.GetComponentInChildren(typeof(Menu));
				if(Menus[i].gameObject.name.CompareTo(SPLASH_SCREEN_STRING) == 0)
				{
					//if we found the slash screen set it as the current menu
					m_CurrentMenu = Menus[i];
					m_RotatingMenu = ((SplashScreen)m_CurrentMenu).IsRotatingMenu;
					break;
				}
				else if (i == Menus.Length - 1)
				{
					#if DEBUG || UNITY_EDITOR
						Debug.LogError("No Splash Screen Found");
					#endif
				}
				i++;
			}while(i < Menus.Length);
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.LogError("No Menus Found");
			#endif
		}
	}

	void OnLevelWasLoaded(int level)
	{
		//find a "splash Scren" (might be just used to redirect to a pause menu or something
		Start();
	}

	// Update is called once per frame
	void Update () 
	{
		if(m_CurrentMenu != null)
		{
			if(!GameData.Instance.m_GameIsRunnging)
			{
                //we update the camera differently if were using a rotating menu (like the view master)
				if(m_RotatingMenu)
				{
					updateViewRotating();
				}
				else
				{
					updateViewNormal();
				}

				//update the current menu 
				m_CurrentMenu.update();
			}
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log("m_CurrentMenu is NULL");
			#endif
		}
	}

	protected virtual void updateViewNormal()
	{
		//if theres a mount point for the camera move the camera towards it
		if(m_CurrentMenu.CameraPos != null)
		{
			MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, m_CurrentMenu.CameraPos.transform.position, CameraMoveSpeed * Time.deltaTime);
			MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, m_CurrentMenu.CameraPos.transform.rotation, CameraRotationSpeed * Time.deltaTime);
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log(m_CurrentMenu.name +  ":  cameraPos is NULL");
			#endif
		}
	}

	protected virtual void updateViewRotating()
	{
		//if theres a mount point for the camera move the camera towards it
		if(m_CurrentMenu.CameraPos != null)
		{
			//MainCamera.transform.parent.transform.position = Vector3.Lerp(MainCamera.transform.position, m_CurrentMenu.CameraPos.transform.position, CameraMoveSpeed * Time.deltaTime);
			MainCamera.transform.parent.transform.rotation = Quaternion.Lerp(MainCamera.transform.parent.transform.rotation, m_CurrentMenu.CameraPos.transform.rotation, CameraRotationSpeed * Time.deltaTime);
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log(m_CurrentMenu.name +  ":  cameraPos is NULL");
			#endif
		}
	}

	public void changeMenu(Menu newMenu)
	{
		if(newMenu != null)
		{
			//record the "old" menu 
			newMenu.LastMenu = m_CurrentMenu;

			//set the new current menu
			m_CurrentMenu = newMenu;

			//make sure the current selection is selected
			if(m_CurrentMenu.CurrentSelection != null)
			{
				//make sure the selected button is highlighted
				m_CurrentMenu.CurrentSelection.ButtonState = MenuButton.ButtonStates.Highlightled;
			}
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log ("newMenu is NULL");
			#endif
		}
	}

	public void LastMenu()
	{
		if(m_CurrentMenu.LastMenu != null)
		{
			//store the current menu
			Menu temp = m_CurrentMenu;

			//change to the last menu
			m_CurrentMenu = m_CurrentMenu.LastMenu;

			//make sure the now previous menu has its last menu variable set to null
			temp.LastMenu = null;
			
			//make sure the current selection is selected
			if(m_CurrentMenu.CurrentSelection != null)
			{
				//make sure the selected button is highlighted
				m_CurrentMenu.CurrentSelection.ButtonState = MenuButton.ButtonStates.Highlightled;
			}
		}
		else
		{
			#if DEBUG || UNITY_EDITOR
				Debug.Log ("newMenu is NULL");
			#endif
		}
	}
}
*/