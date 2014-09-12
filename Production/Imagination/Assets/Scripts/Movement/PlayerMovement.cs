using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour {

    //Component pointers
    private CharacterController m_Controller;
    private Transform m_Camera;

    public GamepadInput.GamePad.Index m_Index;
    public float m_Speed;
    
	// Use this for initialization
	void Start ()
    {

        m_Controller = GetComponent<CharacterController>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();   
 
        print(InputManager.getMove(m_Index));
	}

    Vector3 GetProjection()
    {
        Vector3 projection = m_Camera.forward * InputManager.getMove(m_Index).y;
        projection += m_Camera.right * InputManager.getMove(m_Index).x;

        projection.y = 0;
        return projection.normalized;        
    }

    void Movement()
    {
        Vector3 move = Vector3.zero;
        m_Controller.SimpleMove(move);

        if (InputManager.getMove(m_Index) == Vector2.zero)
         {
           return;
         }

        transform.LookAt(transform.position + GetProjection());
        m_Controller.Move(transform.forward * m_Speed * Time.deltaTime);
       
    }



}
