/*
 * Created by: Kris MAtis
 * handles the camera shutter in the menus
 * 
 */

#region ChangeLog
/*
 * 
 * 30/10/2014 edit: kris matis  shutter stuff is now its own base class (formerly part of menu camrea)
 */
#endregion

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ShutterCamera : MonoBehaviour 
{
    #region Shutter Variables
    //used by other classes to check if the shutter is done moving
    protected bool m_IsDoneShutterMove = true;
    public bool IsDoneShutterMove
    {
        get { return m_IsDoneShutterMove; }
    }

    //whether or not we should be showing the shutter
    protected  bool m_ShowShutter = false;
    public bool ShowShutter
    {
		get { return m_ShowShutter; }
        set { m_ShowShutter = value; }
    }

    //the difference in euler angles for the shutter when its shown
    protected Vector3 m_ShowShutterEulers = new Vector3(0.0f, 0.0f, -90.0f);
    //the shutters initail euler angles (hidden)
    protected Vector3 m_ShutterInitialEuler = Vector3.zero;

    //object used to rotate the shutter
    public GameObject ShutterRotationPoint;

    //timer for the shutter and constant for the shutter speed
    protected const float SHUTTER_SPEED = 0.5f;
    protected float m_ShutterTimer = SHUTTER_SPEED;
    #endregion

    protected virtual void Start()
    {
#if !UNITY_EDITOR && !DEBUG
        //hide th cursor (no better spot to call this
        Screen.showCursor = false;
		Screen.lockCursor = true;
#endif
        //set the inital euler angles for the shutter
        //m_ShutterInitialEuler = ShutterRotationPoint.transform.eulerAngles;
    }

    protected virtual void Update()
    {
        //update the shutter
        updateShutter();
    }

    protected virtual void updateShutter()
    {
        //set that the shutter is not done
        m_IsDoneShutterMove = false;
        if (m_ShowShutter && m_ShutterTimer < SHUTTER_SPEED)
        {
            //need to show the shutter so increment the shutter timer up
            m_ShutterTimer += Time.deltaTime;
            if (m_ShutterTimer > SHUTTER_SPEED)
            {
                m_ShutterTimer = SHUTTER_SPEED;
            }
        }
        else if (!m_ShowShutter && m_ShutterTimer > 0.0f)
        {
            //need to hide the shutter so increment the shutter timer down
            m_ShutterTimer -= Time.deltaTime;
            if (m_ShutterTimer < 0.0f)
            {
                m_ShutterTimer = 0.0f;
            }
        }
        else
        {
            //dont need to move the shutter so set the bool
            m_IsDoneShutterMove = true;
        }
	

        //move the shutter
        ShutterRotationPoint.transform.eulerAngles = transform.eulerAngles + m_ShutterInitialEuler + (m_ShowShutterEulers * (m_ShutterTimer / SHUTTER_SPEED));
    }
}
