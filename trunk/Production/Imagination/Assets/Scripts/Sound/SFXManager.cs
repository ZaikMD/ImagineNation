using UnityEngine;
using System.Collections;

/*
 * Created By: Kole
 *  
 * This Script will hold all the sound effects.
 * 
 * Do Play a sound effect add a point from the class with the logic, or animation manager
 * call the Play Sound function and pass in this.Gameobject for the first argument.
 * for the second argument, pass in a sound as an enum of Sound( Sounds.nameOfSound )
 * 
 * set up a pointer to this class from the class that will be calling the function.
 * this class will likely be setup on the main camera.
 * 
 * 
 */


//The name of all the sounds are below
public enum Sounds
{
    Jump,
    Walk,
    Run
}

public class SFXManager : MonoBehaviour
{
    //Varibles to hold Audio clips needing to be loaded.
    AudioClip m_JumpSFX;
    AudioClip m_WalkSFX;
    AudioClip m_RunSFX;


    void OnLevelLoad()
    { 
    
        //Load all sounds
        //m_JumpSFX = Resources.Load("Jump_Sound");
    }



    // Use this for initialization
	void Start ()
    {
	


	}
	
	// Update is called once per frame
	void Update ()
    {
	


    }


    public void playSound(GameObject objectPlayingTheSound, Sounds sound)
    {
        AudioClip tempSound = getClipFromList(sound);
        AudioSource tempSource = objectPlayingTheSound.gameObject.GetComponent<AudioSource>();

        print(tempSound);
        if (tempSound == null)
        {
            print("no Sound matching that name");
            return;
        }

        if (tempSource == null)
        {
            print("no Source found on that gameObject.");
            return;
        }

        tempSource.PlayOneShot(tempSound);

    }


    //Checks the name of the sound(enum) and returns the proper sounds.
    AudioClip getClipFromList(Sounds sound)
    { 
        switch(sound)
        {
            case Sounds.Jump:
               
            return m_JumpSFX;
            break;

            case Sounds.Walk:
            return m_WalkSFX;
            break;

            case Sounds.Run:
            return m_RunSFX;
            break;

            default:
            return m_JumpSFX;
            
            break;
        }
    }

}
