using UnityEngine;
using System.Collections;


public class AcceptInputFrom : MonoBehaviour 
{
	public PlayerInput ReadInputFrom = PlayerInput.GamePadOne;

    public AcceptInputFrom CopySettingFrom;

    void Start()
    {
        if (CopySettingFrom != null)
        {
            ReadInputFrom = CopySettingFrom.ReadInputFrom;
        }
    }
}
