using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public bool m_isMenuActive = true;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnGUI()
    {
        if (m_isMenuActive)
        {
            Rect button = new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 4 - Screen.height / 5, Screen.width / 2, Screen.height / 4);
            string buttonText = "Play";
            if (GUI.Button(button, buttonText))
                {
                    m_isMenuActive = false;
                    //call camera controller and switch to game camera, 
                }
        }
    }
}
