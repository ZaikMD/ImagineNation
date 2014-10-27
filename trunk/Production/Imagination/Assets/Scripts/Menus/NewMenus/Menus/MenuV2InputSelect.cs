/*
 * Created by: Kris MAtis
 * the menu for selecting player input
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made and commented
 */
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2InputSelect : TwoPlayerSelectionMenu
{
    protected override void changeMenu()
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
    }
}
