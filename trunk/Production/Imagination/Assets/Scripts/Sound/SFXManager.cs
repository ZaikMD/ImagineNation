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
 * 
 * Oct, 1, 2014, 
 * Updated the class to work with two cameras. to play a sound will be done the same way
 * this now gets the player who is closest and gets a value based of off the distance to the player
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


	//Variables for class
	AudioSource m_Source;

//TODO: load which players are one and two
	public Transform m_PlayerOne;
	public Transform m_PlayerTwo;

	public Object m_SoundObject;

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
		//Instantiate varibles
		m_Source = this.gameObject.GetComponent<AudioSource> ();
//TODO: load which players are player one and two

		m_PlayerOne = getPlayerTransform (GameData.Instance.PlayerOneCharacter);
		m_PlayerTwo = getPlayerTransform (GameData.Instance.PlayerTwoCharacter);

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
	/// Gets the player transform by c
	/// </summary>
	/// <returns>The player transform.</returns>
	/// <param name="charater">Charater.</param>
	Transform getPlayerTransform(Characters charater)
	{
		switch(charater)
		{
			case Characters.Alex:
			return GameObject.FindGameObjectWithTag("Alex").transform;
			break;


			case Characters.Derek:
			return GameObject.FindGameObjectWithTag("Derek").transform;
			break;

			case Characters.Zoey:
			return GameObject.FindGameObjectWithTag("Zoe").transform;
			break;

			default:
			Debug.LogError("Enum is out of range");
			return null;
			break;
		}
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

		//Checks if there is a audio source on the player, if not adds one
		if(objectPlayingTheSound.GetComponent<AudioSource> () == null)
		{
			objectPlayingTheSound.AddComponent<AudioSource>();
		}

		AudioSource tempAudioSource = objectPlayingTheSound.GetComponent<AudioSource> ();


		//Safety check to make sure we have a sound
		if (tempSoundInfo.m_AudioClip == null)
        {
            Debug.LogError("no Sound matching that name");
            return;
        }
		    

		tempAudioSource.volume = getSoundVolume (objectPlayingTheSound.transform.position);


        tempAudioSource.clip = tempSoundInfo.m_AudioClip;

        if (tempSoundInfo.OneShot)
        {
			tempAudioSource.PlayOneShot(tempSoundInfo.m_AudioClip);
        }
        else
        {
			if(!tempAudioSource.isPlaying)
				tempAudioSource.Play();
        }

    }
	/// <summary>
	/// Plaies the sound.
	/// </summary>
	/// <param name="Location">Pass in a vector3 for the location of the sound to play from don't use if it needs to move with them.</param>
	/// <param name="The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere">.</param>
	public void playSound(Vector3 Location, Sounds sound)
	{

		GameObject soundObject = (GameObject)Instantiate (m_SoundObject);

		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
		AudioInfo tempSoundInfo = getClipFromList(sound);
		AudioSource tempAudioSource = soundObject.GetComponent<AudioSource> ();

				
		
		//Safety check to make sure we have a sound
		if (tempSoundInfo.m_AudioClip == null)
		{
			Debug.LogError("no Sound matching that name");
			return;
		}

		soundObject.GetComponent<AutoDestroy> ().timer = tempSoundInfo.m_AudioClip.length;


		tempAudioSource.volume = getSoundVolume (Location);
		
		
		tempAudioSource.clip = tempSoundInfo.m_AudioClip;
		
		if (tempSoundInfo.OneShot)
		{
			tempAudioSource.PlayOneShot(tempSoundInfo.m_AudioClip);
		}
		else
		{
			if(!tempAudioSource.isPlaying)
				tempAudioSource.Play();
		}


	}
	

	/// <summary>
	/// Stops the sound.
	/// </summary>
	/// <param name="objectPlayingTheSound">Object playing the sound.</param>
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
	/// Gets the sound volume in reference to the player distance;
	/// </summary>
	/// <returns>The sound volume.</returns>
	private float getSoundVolume(Vector3 SoundLocation)
	{
		float DisToP1 = Vector3.Distance(m_PlayerOne.position, SoundLocation);
		float DisToP2 = Vector3.Distance(m_PlayerTwo.position, SoundLocation);
	
		if(DisToP1 <= DisToP2)
		{
			//Player one is closer to the sound, base calculation of volume of there distance;
			return (100 - DisToP1) / 100 ;
		}
		else
		{
			//Player Two is closer, use there distance;
			return (100 - DisToP2) / 100;
		}


		return 1.0f;
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
