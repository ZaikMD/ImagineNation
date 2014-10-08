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
