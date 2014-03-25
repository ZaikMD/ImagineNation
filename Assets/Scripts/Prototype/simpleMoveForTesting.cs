using UnityEngine;
using System.Collections;

public class simpleMoveForTesting : MonoBehaviour {

    public float m_MoveSpeed = 5;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal") * m_MoveSpeed * Time.deltaTime;
        float verticle = Input.GetAxis("Vertical") * m_MoveSpeed * Time.deltaTime;

        transform.Translate(horizontal, 0, verticle);

	}
}
