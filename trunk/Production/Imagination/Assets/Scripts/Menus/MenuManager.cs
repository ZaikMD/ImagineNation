/*
*MenuManager
*
*resposible for updating the current menu
*
*moveing the camera to the menus mount point
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
		//DontDestroyOnLoad(this.gameObject);
	}

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
