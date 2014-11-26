/*
*ButtonChangeMenu
*
*resposible for loading the next scene
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 24/10/2014 Edit: script made - Kris Matis.
*
* 
*/
#endregion

using UnityEngine;
using System.Collections;

public class ButtonChangeMenuV2 : ButtonV2
{
    public MenuV2 NextMenu;

    static MenuCamera m_Camera;

	// Use this for initialization
	protected override void start () 
    {
        #if DEBUG || UNITY_EDITOR
            if (NextMenu == null)
            {
                Debug.LogError("NO MENU SET");
            }
        #endif

        if (m_Camera == null)
        {
            m_Camera = Camera.main.GetComponent<MenuCamera>();
        }

        if (m_Camera == null)
        {
            #if DEBUG || UNITY_EDITOR
            Debug.LogError("NO CAMERA FOUND");
            #endif
        }
	}

    public override void use()
    {
        m_Camera.changeMenu(NextMenu);
		NextMenu.LastMenu = ParentMenu;
        ParentMenu.IsActiveMenu = false;
        NextMenu.IsActiveMenu = true;
    }
}