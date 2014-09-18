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

public struct AudioInfo
{
    public AudioClip m_AudioClip;
    public bool OneShot;    
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
        m_JumpSFX = (AudioClip)Resources.Load("Sounds/Alex_Jump");
        m_WalkSFX = (AudioClip)Resources.Load("Sounds/Jump_Pad");
        m_RunSFX = (AudioClip)Resources.Load("Sounds/footsteps_carpet_edit");
    }



    // Use this for initialization
	void Start ()
    {
        m_JumpSFX = (AudioClip)Resources.Load("Sounds/Alex_Jump");
        m_WalkSFX = (AudioClip)Resources.Load("Sounds/Jump_Pad");
        m_RunSFX = (AudioClip)Resources.Load("Sounds/footsteps_carpet_edit");


	}
	
	// Update is called once per frame
	void Update ()
    {
	


    }


    public void playSound(GameObject objectPlayingTheSound, Sounds sound)
    {
        AudioInfo tempSoundInfo = getClipFromList(sound);
        AudioSource tempSource = objectPlayingTheSound.gameObject.GetComponent<AudioSource>();

        print(tempSoundInfo.m_AudioClip);
        if (tempSoundInfo.m_AudioClip == null)
        {
            print("no Sound matching that name");
            return;
        }


        if (tempSource == null)
        {
            print("no Source found on that gameObject.");
            return;
        }

        tempSource.clip = tempSoundInfo.m_AudioClip;

        if (tempSoundInfo.OneShot)
        {
            tempSource.PlayOneShot(tempSoundInfo.m_AudioClip);
        }
        else
        {
            if(!tempSource.isPlaying)
                tempSource.Play();
        }

    }


    public void stopSound(GameObject objectPlayingTheSound)
    {
       // AudioInfo tempSoundInfo = getClipFromList(sound);
        AudioSource tempSource = objectPlayingTheSound.gameObject.GetComponent<AudioSource>();

        if (tempSource == null || !tempSource.isPlaying)
        {
            return;
        }

        tempSource.Stop();  

 
    }

    //Checks the name of the sound(enum) and returns the proper sounds.
    AudioInfo getClipFromList(Sounds sound)
    {
        AudioInfo tempAudioInfo = new AudioInfo();
        tempAudioInfo.m_AudioClip = null;
        tempAudioInfo.OneShot = false;

        switch(sound)
        {
            case Sounds.Jump:
               
            tempAudioInfo.m_AudioClip = m_JumpSFX;
            tempAudioInfo.OneShot = false;
            return tempAudioInfo;
            break;

            case Sounds.Walk:
            tempAudioInfo.m_AudioClip = m_WalkSFX;;
            tempAudioInfo.OneShot = false;
            return tempAudioInfo;
            break;

            case Sounds.Run:
            tempAudioInfo.m_AudioClip = m_RunSFX;;
            tempAudioInfo.OneShot = false;
            return tempAudioInfo;
            break;

            default:
            Debug.LogError("No regonized sound passed in");
            return tempAudioInfo;
            
            break;
        }
    }

}
