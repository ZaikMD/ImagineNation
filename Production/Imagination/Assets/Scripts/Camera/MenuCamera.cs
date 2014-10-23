/*
 * Created by: Kris MAtis
 * handles the camera behavior in the menus
 * 
 */

#region ChangeLog
/*
* 23/10/2014 edit: kris matis  created the script and commented it
*/
#endregion

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MenuCamera : MonoBehaviour
{
    #region Shutter Variables
    //used by other classes to check if the shutter is done moving
    bool m_IsDoneShutterMove = true;
    public bool IsDoneShutterMove
    {
        get { return m_IsDoneShutterMove; }
    }

    //whether or not we should be showing the shutter
    bool m_ShowShutter = false;
    public bool ShowShutter
    {
        get { return m_IsDoneShutterMove; }
        set { m_ShowShutter = value; }
    }

    //the difference in euler angles for the shutter when its shown
    Vector3 m_ShowShutterEulers = new Vector3(0.0f, 0.0f, -90.0f);
    //the shutters initail euler angles (hidden)
    Vector3 m_ShutterInitialEuler;

    //object used to rotate the shutter
    public GameObject ShutterRotationPoint;

    //timer for the shutter and constant for the shutter speed
    const float SHUTTER_SPEED = 0.15f;
    float m_ShutterTimer = SHUTTER_SPEED;
    #endregion

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

    void Start()
    {
        //set the inital euler angles for the shutter
        m_ShutterInitialEuler = ShutterRotationPoint.transform.eulerAngles;

        //set the initial position
        m_InitialPosition = transform.position;

        //pick an inital target position
        pickInitialTargetPosition();
    }

    void Update()
    {
        //update the shutter
        updateShutter();

        //update the position
        updatePosition();

        //look at the menu or other target
        transform.LookAt(LookAt);
    }

    public void changePosition(Vector3 newPosition, Transform lookAt)
    {
        //set the cameras position
        transform.position = newPosition;

        //set the look at
        LookAt = lookAt;

        //we have a new inital position
        m_InitialPosition = newPosition;

        //pick a new target position
        pickInitialTargetPosition();
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

    void updateShutter()
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
        else if(!m_ShowShutter && m_ShutterTimer > 0.0f)
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
        ShutterRotationPoint.transform.eulerAngles = m_ShutterInitialEuler + (m_ShowShutterEulers * (m_ShutterTimer/SHUTTER_SPEED));
    }
}
