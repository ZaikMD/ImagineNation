/*
 * Created by: Kris MAtis
 * handles the camera behavior in the menus
 * 
 */

#region ChangeLog
/*
* 23/10/2014 edit: kris matis  created the script and commented it
 * 
 * 27/10/2014 edit: kris matis  set isDoneMoving to false as soon as a new menu is set
 * 
 * 30/10/2014 edit: kris matis  shutter stuff is now in a base class
 */
#endregion

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MenuCamera : ShutterCamera
{
    protected MenuV2 m_NewMenu = null;

    #region Random Move
    //the inital position
    Vector3 m_InitialPosition = Vector3.zero;

    //the last target position
    Vector3 m_LastTargetPosition = Vector3.zero;

    //the current target position
    Vector3 m_TargetPosition = Vector3.zero;

    //max distance the target can be from the intial position
    const float MAX_TARGET_DISTANCE = 0.55f;

    //the speed the camera moves at
    const float MOVE_SPEED = 0.75f;
    #endregion

    public Transform LookAt;

    protected override void Start()
    {
        base.Start();

        //set the initial position
        m_InitialPosition = transform.position;

        //pick an inital target position
        pickInitialTargetPosition();
    }

    protected override void Update()
    {
        base.Update();

		if(IsDoneShutterMove && ShowShutter)
		{
            if(m_NewMenu!= null)
			    changePosition(m_NewMenu.CameraMountPoint.transform.position, m_NewMenu.transform);
		}

        //update the position
        updatePosition();

        //look at the menu or other target
        transform.LookAt(LookAt);
    }

    public void changeMenu(MenuV2 newMenu)
    {
        ShowShutter = true;
		m_NewMenu = newMenu;
        m_IsDoneShutterMove = false;
    }

    void changePosition(Vector3 newPosition, Transform lookAt)
    {
        //set the cameras position
        transform.position = newPosition;

        //set the look at
        LookAt = lookAt;

        //we have a new inital position
        m_InitialPosition = newPosition;

        //pick a new target position
        pickInitialTargetPosition();

        ShowShutter = false;
    }
    
    void updatePosition()
    {
        //the vector that were going to use to move this frame
        Vector3 moveVector = (m_TargetPosition - m_LastTargetPosition) * MOVE_SPEED * Time.deltaTime;
        if (moveVector.magnitude > (m_TargetPosition - transform.position).magnitude)
        {
            //distance to the target position was less than the position + move vector
            //set the cameras position
            transform.position = m_TargetPosition;
            //pick a new target
            pickTargetPosition();
        }
        else
        {
            //move the camera
            transform.position += moveVector;
        }
    }

    void pickInitialTargetPosition()
    {
        //sets the last position to the initial position (should be the current position if this function is called)
        m_LastTargetPosition = m_InitialPosition;
        //pick a new target
        m_TargetPosition = m_InitialPosition + new Vector3(Random.value % MAX_TARGET_DISTANCE, Random.value % MAX_TARGET_DISTANCE, 0.0f);
    }

    void pickTargetPosition()
    {
        //set the last target
        m_LastTargetPosition = m_TargetPosition;
        //pick the new target
        m_TargetPosition = m_InitialPosition + new Vector3(Random.value % MAX_TARGET_DISTANCE, Random.value % MAX_TARGET_DISTANCE, 0.0f);
    }
}
