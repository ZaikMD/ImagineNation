using UnityEngine;
using System.Collections;

//used to group variables for both players
public class PlayerInputSelection
{
	//currently selected input
	PlayerInput m_CurrentSelection = PlayerInput.None;
	//the mount point for showing their selection tpye
	public GameObject m_SelectionMountPoint;
	//the texture to show that theyre ready
	public GUITexture m_ReadyTexure;
	
	public PlayerInputSelection(GameObject selectionMountPoint, GUITexture readyTexture)
	{
		m_SelectionMountPoint = selectionMountPoint;
		m_ReadyTexure = readyTexture;
	}
	
	//resets all the variables
	public void reset()
	{
		m_ReadyTexure.enabled = false;
		m_CurrentSelection = PlayerInput.None;
		m_IsReady = false;
		
		foreach (Transform child in m_SelectionMountPoint.transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}
	
	public PlayerInput CurrentSelection
	{
		get { return m_CurrentSelection; }
		set { m_CurrentSelection = value; }
	}
	
	bool m_IsReady = false;
	public bool IsReady
	{
		get { return m_IsReady; }
		set { m_IsReady = value; }
	}
	
	public bool isSet()
	{
		return m_CurrentSelection != PlayerInput.None;
	}
}
