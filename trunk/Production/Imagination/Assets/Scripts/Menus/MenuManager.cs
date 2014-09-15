using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
	public static MenuManager Instance{ get; private set; }
	
	void Awake()
	{
		//if theres another instance (there shouldnt be) destroy this
		if(Instance != null && Instance != this)
		{
			//destroy all other instances
			Destroy(gameObject);
		}
		
		//set the instance
		Instance = this;
		
		//prevents this object being destroyed between scene loads
		DontDestroyOnLoad(this.gameObject);
	}

	//------------------------------------------------------------------------------
	
	Menu m_CurrentMenu;

	Camera MainCamera;

	public float CameraMoveSpeed = 0.05f;
	public float CameraRotationSpeed = 0.05f;

	bool m_RotatingMenu;

	// Use this for initialization
	void Start () 
	{
		MainCamera = (Camera) GameObject.FindGameObjectWithTag("MainCamera").GetComponent(typeof(Camera));

		if(MainCamera == null)
		{
			Debug.LogError("No Main Camera Found");
		}

		//get all the menu game objects
		GameObject[] menus = GameObject.FindGameObjectsWithTag("Menu");
		if(menus.Length != 0)
		{
			int i = 0;
			do
			{
				//get the menu object
				menus[i] = (Menu) menus[i].GetComponent(typeof(Menu));//.GetComponentInChildren(typeof(Menu));
				if(menus[i].gameObject.name.CompareTo("Splash Screen") == 0)
				{
					//if we found the slash screen set it as the current menu
					m_CurrentMenu = menus[i];
					m_RotatingMenu = ((SplashScreen)m_CurrentMenu).IsRotatingMenu;
					break;
				}
				else if (i == menus.Length - 1)
				{
					Debug.LogError("No Splash Screen Found");
				}
				i++;
			}while(i < menus.Length);
		}
		else
		{
			Debug.LogError("No Menus Found");
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
			Debug.Log("m_CurrentMenu is NULL");
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
			Debug.Log(m_CurrentMenu.name +  ":  cameraPos is NULL");
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
			Debug.Log(m_CurrentMenu.name +  ":  cameraPos is NULL");
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
			Debug.Log ("newMenu is NULL");
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
			Debug.Log ("newMenu is NULL");
		}
	}
}
