/*
*AcceptInputFrom
*
*resposible for keepint track of what input scripts are going to ne reading
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


public class AcceptInputFrom : MonoBehaviour 
{
	//what input scripts on the same object are going to read
	public PlayerInput ReadInputFrom = PlayerInput.GamePadOne;

	//literally what it sounds like
    public AcceptInputFrom CopySettingFrom;

    void Start()
    {
        if (CopySettingFrom != null)
        {
			//copy the input
            ReadInputFrom = CopySettingFrom.ReadInputFrom;
        }
    }
}
