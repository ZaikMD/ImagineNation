
/*
 * Created by Kole Tackney
 * Date: Sept, 12, 2014
 *  
 * This script controls the movment of the character on the x and z axis
 * For our players, to Add movement for a player, Add this script to the character 
 * in the unity editor. then set the speed in the unity editor.  
 * 
 * For best results, Delete other Colliders attached to the Object, Also make sure
 * that the Volume of the character controller attached to the character is the right size.
 * 
 * NOTE: the AcceptedInputFrom script will need its GamePad set in the unity editor.
 * 
 * 
 * 
 * 15/9/2014 - Jason H.
 * 			Switched simple move to move, to fix a gravity issue
 * 
 */

using UnityEngine;
using System.Collections;


//Making Sure Design can't forget needed components
[RequireComponent(typeof(CharacterController))]

//Make sure that you set the character accepted input in the unity editor.
[RequireComponent(typeof(AcceptInputFrom))]


public class PlayerMovement : MonoBehaviour {

    //Component pointers
    private CharacterController m_Controller;
    private Transform m_Camera;
    private AcceptInputFrom m_Accepted;
    private Animation m_Anim;

    //Set Speed in unity editor
    public float m_Speed;
    
	// Use this for initialization
	void Start ()
    {
        //Setting all our pointers, all are ethier required components or need.
        m_Controller = GetComponent<CharacterController>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_Accepted = GetComponent<AcceptInputFrom>();
        m_Anim = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //We Call the Movment function.
        Movement();

    

        //Animation Stuff
        PlayAnimation();
    }

    //This function get a vector3 for the direction we should be facing based of off the camera.
    Vector3 GetProjection()
    {
        Vector3 projection = m_Camera.forward * InputManager.getMove(m_Accepted.ReadInputFrom).y;
        projection += m_Camera.right * InputManager.getMove(m_Accepted.ReadInputFrom).x;

        projection.y = 0;
        return projection.normalized;        
    }

   
    void Movement()
    {

        //if we do not have any values, no need to continue
        if (InputManager.getMove(m_Accepted.ReadInputFrom) == Vector2.zero)
         {
           return;
         }
        
        //First we look at the direction from GetProjection, our forward is now that direction, so we move forward. 
        transform.LookAt(transform.position + GetProjection());
        m_Controller.Move(transform.forward * m_Speed * Time.deltaTime);
       
    }

    void PlayAnimation()
    {
        if (InputManager.getMove(m_Accepted.ReadInputFrom) == Vector2.zero)
        {
            m_Anim.Play("Idle");
            return;
        }

        if (InputManager.getMove(m_Accepted.ReadInputFrom).x < 0.3f 
            && InputManager.getMove(m_Accepted.ReadInputFrom).x > -0.3f
            && InputManager.getMove(m_Accepted.ReadInputFrom).y < 0.3f
            && InputManager.getMove(m_Accepted.ReadInputFrom).y > -0.3f)
        {

            m_Anim.Play("Walk");
            return;
        }

        m_Anim.Play("Run");
       
    
    }


}
