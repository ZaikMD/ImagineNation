using UnityEngine;
using System.Collections;

public class simpleMoveForTesting : MonoBehaviour {

    public float m_MoveSpeed = 15;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal") * m_MoveSpeed * Time.deltaTime;



        float verticle = Input.GetAxis("Vertical") * m_MoveSpeed * Time.deltaTime;
		transform.Rotate(0, horizontal, 0);
        transform.Translate(0, 0, verticle);

	}


}
