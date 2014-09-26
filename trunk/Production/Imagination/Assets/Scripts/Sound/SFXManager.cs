using UnityEngine;
using System.Collections;

/*
 * Created By: Kole
 *  
 * This Script loads all sound effects and tells the audio sources to play
 * a passed in sound.
 * 
 * To Play a sound effect add a point from the class with the logic, or animation manager
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
    Run,
	Collectable,
	WeaponWoosh,
	JumpPad,
	AlexHitOne,
	AlexHitTwo,
	AlexHitThree,
	AlexHurt,
	DerekHitOne,
	DerekHurt
}

//this struct holds all the info needed to determine how to play are sounds
public struct AudioInfo
{
    public AudioClip m_AudioClip;
    public bool OneShot;    
}

/// <summary>
/// the SFX manager class plays all SFX for our game,
/// other classes that will use this class will have a
/// reference to it and call the PlaySound() Function.
/// </summary>
public class SFXManager : MonoBehaviour
{
    //Varibles to hold Audio clips needing to be loaded.
    AudioClip m_JumpSFX;
    AudioClip m_WalkSFX;
    AudioClip m_RunSFX;
	AudioClip m_Collectable;
	AudioClip m_WeaponWoosh;
	AudioClip m_JumpPad;
	AudioClip m_AlexHitOne;
	AudioClip m_AlexHitTwo;
	AudioClip m_AlexHitThree;
	AudioClip m_AlexHurt;
	AudioClip m_DerekHitOne;
	AudioClip m_DerekHurt;


	/// <summary>
	/// Raises the level load event.
	/// Loads in all the sounds from resources folder
	/// </summary>
    void OnLevelLoad()
    { 
    
        //Load all sounds
        
		//Common Sounds
		m_JumpSFX = (AudioClip)Resources.Load("Sounds/Alex/Alex_Jump");
        m_WalkSFX = (AudioClip)Resources.Load("Sounds/Common/Jump_Pad");
		m_RunSFX = (AudioClip)Resources.Load("Sounds/Common/footsteps_carpet_edit2");
		m_WeaponWoosh = (AudioClip)Resources.Load("Sounds/Common/Woosh");
		m_Collectable = (AudioClip)Resources.Load("Sounds/Common/Collectable");
		m_JumpPad = (AudioClip)Resources.Load("Sounds/Common/Jump_Pad");

		//Alex Sounds
		m_AlexHitOne = (AudioClip)Resources.Load("Sounds/Alex/First_Weapon_hit_Alex");
		m_AlexHitTwo = (AudioClip)Resources.Load("Sounds/Alex/Second_Weapon_Hit_Alex");
		m_AlexHitThree = (AudioClip)Resources.Load("Sounds/Alex/Final_Weapon_hit_Alex");
		m_AlexHurt = (AudioClip)Resources.Load ("Sounds/Alex/painful_grunts_boy");

		//Derek Sounds
		m_DerekHitOne = (AudioClip)Resources.Load ("Sounds/Derek/Realistic_Punch");
		m_DerekHurt = (AudioClip)Resources.Load ("Sounds/Alex/painful_grunts_boy");


		//Zoey Sounds

    }



    /// <summary>
    /// Start this instance.
	/// using Start to load for testing, OnLevelLoad in future
    /// </summary>
	void Start ()
    {
		//This is used when not loading a new level, needed for testing,

		//Common Sounds
		m_JumpSFX = (AudioClip)Resources.Load("Sounds/Alex/Alex_Jump");
		m_WalkSFX = (AudioClip)Resources.Load("Sounds/Common/Jump_Pad");
		m_RunSFX = (AudioClip)Resources.Load("Sounds/Common/footsteps_carpet_edit2");
		m_WeaponWoosh = (AudioClip)Resources.Load("Sounds/Common/Woosh");
		m_Collectable = (AudioClip)Resources.Load("Sounds/Common/Collectable");
		m_JumpPad = (AudioClip)Resources.Load("Sounds/Common/Jump_Pad");
		
		//Alex Sounds
		m_AlexHitOne = (AudioClip)Resources.Load("Sounds/Alex/First_Weapon_hit_Alex");
		m_AlexHitTwo = (AudioClip)Resources.Load("Sounds/Alex/Second_Weapon_Hit_Alex");
		m_AlexHitThree = (AudioClip)Resources.Load("Sounds/Alex/Final_Weapon_hit_Alex");
		m_AlexHurt = (AudioClip)Resources.Load ("Sounds/Alex/painful_grunts_boy");
		
		//Derek Sounds
		m_DerekHitOne = (AudioClip)Resources.Load ("Sounds/Derek/Realistic_Punch");
		m_DerekHurt = (AudioClip)Resources.Load ("Sounds/Alex/painful_grunts_boy");
		
		
		//Zoey Sounds

		
	}
	
	/// <summary>
	/// This function plays a sound where you want it to play.
	/// </summary>
	/// <param name="objectPlayingTheSound">Object playing the sound. pass in this.gameObject to have it originate from the object the script is attached</param>
	/// <param name="sound">The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere</param>
    public void playSound(GameObject objectPlayingTheSound, Sounds sound)
    {

		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
        AudioInfo tempSoundInfo = getClipFromList(sound);
        AudioSource tempSource = objectPlayingTheSound.gameObject.GetComponent<AudioSource>();

    //    print(tempSoundInfo.m_AudioClip);
        if (tempSoundInfo.m_AudioClip == null)
        {
        //    print("no Sound matching that name");
            return;
        }


        if (tempSource == null)
        {

			tempSource = objectPlayingTheSound.AddComponent<AudioSource>();
			print("no Source found on that gameObject.");
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
	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="Location">Pass in a vector3 for the location of the sound to play from don't use if it needs to move with them.</param>
	/// <param name="The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere">.</param>
	public void playSound(Vector3 Location, Sounds sound)
	{

		//Creates game object and places at location desired, then adds a Audio source to them;
		GameObject SoundEmitter = new GameObject ();
		SoundEmitter.transform.position = Location;
		SoundEmitter.AddComponent<AudioSource> ();
		AudioSource tempSource = SoundEmitter.gameObject.GetComponent<AudioSource>();


		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
		AudioInfo tempSoundInfo = getClipFromList(sound);


		
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

	/// <summary>
	/// Checks sent in enum, returns apropriote data 
	/// such as if it is a one shot and the AudioClip
	/// </summary>
	/// <returns>The clip from list.</returns>
	/// <param name="sound"> An enum of the Sound. Put Sounds.NameOfSound</param>
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
            tempAudioInfo.m_AudioClip = m_WalkSFX;
            tempAudioInfo.OneShot = false;
            return tempAudioInfo;
            break;

            case Sounds.Run:
            tempAudioInfo.m_AudioClip = m_RunSFX;
            tempAudioInfo.OneShot = false;
            return tempAudioInfo;
            break;

			case Sounds.Collectable:
            tempAudioInfo.m_AudioClip = m_Collectable;
            tempAudioInfo.OneShot = true;
            return tempAudioInfo;
            break;

			case Sounds.WeaponWoosh:
			tempAudioInfo.m_AudioClip = m_WeaponWoosh;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.JumpPad:
			tempAudioInfo.m_AudioClip = m_JumpPad;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.AlexHitOne:
			tempAudioInfo.m_AudioClip = m_AlexHitOne;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.AlexHitTwo:
			tempAudioInfo.m_AudioClip = m_AlexHitTwo;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.AlexHitThree:
			tempAudioInfo.m_AudioClip = m_AlexHitThree;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.AlexHurt:
			tempAudioInfo.m_AudioClip = m_AlexHurt;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

			case Sounds.DerekHitOne:
			tempAudioInfo.m_AudioClip = m_DerekHitOne;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
		

			case Sounds.DerekHurt:
			tempAudioInfo.m_AudioClip = m_AlexHurt;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
			

			
		default:
			Debug.LogError("No regonized sound passed in");
            return tempAudioInfo;
            
            break;
        }
    }

}
