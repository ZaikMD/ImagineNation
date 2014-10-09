using UnityEngine;
using System.Collections;

/*
 * Created By: Kole
 *  
 * This Script loads all sound effects and tells the audio sources to play
 * a passed in sound.
 * 
 * To Play a sound effect, First get a reference to this script (there should be a
 * empty game Object in your scene with the SFXManager script and the Sound Manager tag).
 * call the Play Sound function and pass in ethier the game object that will be the origin,
 * or a vector3 repersenting the posityion of the orgin for the first argument.
 * for the second argument, pass in a sound as an enum of Sound( Sounds.nameOfSound )
 * 
 * 
 * Oct, 1, 2014, kole
 * Updated the class to work with two cameras. to play a sound will be done the same way
 * this now gets the player who is closest and gets a value based of off the distance to the player
 * 
 * 
 * Oct, 8, 2014, kole
 * Updated the class to use constants instead of strings
 * 
 */


//The name of all the sounds are below
public enum Sounds
{
	//Common
    Jump,
    Walk,
    Run,
	Collectable,
	WeaponWoosh,
	JumpPad,
	Zipper,

	//Alex
	AlexHitOne,
	AlexHitTwo,
	AlexHitThree,
	AlexHurt,
	AlexDeath,
	AlexJump,

	//Derek
	DerekHitOne,
	DerekHitTwo,
	DerekHitThree,
	DerekHurt,
	DerekDeath,
	DerekJump,

	//Zoey
	ZoeyHitOne,
	ZoeyHitTwo,
	ZoeyHitThree,
	ZoeyHurt,
	ZoeyDeath,
	ZoeyJump,
	ZoeyOpenWings,
	ZoeyCloseWings,
	ZoeyDeployedWings

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
	AudioClip m_Zipper;

	//Alex Sounds
	AudioClip m_AlexHitOne;
	AudioClip m_AlexHitTwo;
	AudioClip m_AlexHitThree;
	AudioClip m_AlexHurt;
	AudioClip m_AlexDeath;
	AudioClip m_AlexJump;

	//Derek Sounds
	AudioClip m_DerekHitOne;
	AudioClip m_DerekHitTwo;
	AudioClip m_DerekHitThree;
	AudioClip m_DerekHurt;
	AudioClip m_DerekDeath;
	AudioClip m_DerekJump;


	//Zoey Sounds
	AudioClip m_ZoeyHitOne;
	AudioClip m_ZoeyHitTwo;
	AudioClip m_ZoeyHitThree;
	AudioClip m_ZoeyHurt;
	AudioClip m_ZoeyDeath;
	AudioClip m_ZoeyJump;
	AudioClip m_ZoeyWingsOpen;
	AudioClip m_ZoeyWingsClose;
	AudioClip m_ZoeyWingsDeploy;



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
		m_JumpSFX = (AudioClip)Resources.Load(Constants.Sounds.ALEX_JUMP);
		m_WalkSFX = (AudioClip)Resources.Load(Constants.Sounds.WALK);
		m_RunSFX = (AudioClip)Resources.Load(Constants.Sounds.RUN);
		m_WeaponWoosh = (AudioClip)Resources.Load(Constants.Sounds.WEAPON_WOOSH);
		m_Collectable = (AudioClip)Resources.Load(Constants.Sounds.COLLECTABLE);
		m_JumpPad = (AudioClip)Resources.Load(Constants.Sounds.JUMPAD);
		
		//Alex Sounds
		m_AlexHitOne = (AudioClip)Resources.Load(Constants.Sounds.ALEX_FIRST_WEAPON_HIT);
		m_AlexHitTwo = (AudioClip)Resources.Load(Constants.Sounds.ALEX_SECOND_WEAPON_HIT);
		m_AlexHitThree = (AudioClip)Resources.Load(Constants.Sounds.ALEX_THIRD_WEAPON_HIT);
		m_AlexHurt = (AudioClip)Resources.Load (Constants.Sounds.ALEX_HURT);
		m_AlexDeath = (AudioClip)Resources.Load (Constants.Sounds.ALEX_DEATH);
		m_AlexJump = (AudioClip)Resources.Load (Constants.Sounds.ALEX_JUMP);
		
		//Derek Sounds
<<<<<<< .mine
		m_DerekHitOne = (AudioClip)Resources.Load ("Sounds/Derek/Derek_First_Hit");
		m_DerekHitTwo = (AudioClip)Resources.Load ("Sounds/Derek/Derek_Second_Hit");
		m_DerekHitThree = (AudioClip)Resources.Load ("Sounds/Derek/Derek_Third_Hit");
		m_DerekHurt = (AudioClip)Resources.Load ("Sounds/Common/Derek_Painful_Grunt");
		m_DerekJump = (AudioClip)Resources.Load ("Sounds/Derek/Derek_Jump");


=======
		m_DerekHitOne = (AudioClip)Resources.Load (Constants.Sounds.DEREK_FIRST_WEAPON_HIT);
		m_DerekHitTwo = (AudioClip)Resources.Load (Constants.Sounds.DEREK_SECOND_WEAPON_HIT);
		m_DerekHitThree = (AudioClip)Resources.Load (Constants.Sounds.DEREK_THIRD_WEAPON_HIT);
		m_DerekHurt = (AudioClip)Resources.Load (Constants.Sounds.DEREK_HURT);
		m_DerekDeath = (AudioClip)Resources.Load (Constants.Sounds.DEREK_DEATH);
		m_DerekJump = (AudioClip)Resources.Load (Constants.Sounds.DEREK_JUMP);
		
		
>>>>>>> .r794
		//Zoey Sounds
<<<<<<< .mine
		m_ZoeyHitOne = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_First_Hit");
		m_ZoeyHitTwo = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Second_Hit");
		m_ZoeyHitThree = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Third_Hit");
		m_ZoeyHurt = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Painful_Grunt");
		m_ZoeyDeath = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Death");
		m_ZoeyJump = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Jump");
		m_ZoeyWingsOpen = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Open");
		m_ZoeyWingsClose = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Close");
		m_ZoeyWingsDeploy = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Deploy");
=======
		m_ZoeyHitOne = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_FIRST_WEAPON_HIT);
		m_ZoeyHitTwo = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_SECOND_WEAPON_HIT);
		m_ZoeyHitThree = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_THIRD_WEAPON_HIT);
		m_ZoeyHurt = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_HURT);
		m_ZoeyDeath = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_DEATH);
		m_ZoeyJump = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_JUMP);
		m_ZoeyWingsOpen = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_OPEN);
		m_ZoeyWingsClose = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_CLOSE);
		m_ZoeyWingsDeploy = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_DEPLOY);
>>>>>>> .r794

<<<<<<< .mine

=======
>>>>>>> .r794
    }



    /// <summary>
    /// Start this instance.
	/// using Start to load for testing, OnLevelLoad in future
    /// </summary>
	void Start ()
    {

		//TODO: load which players are player one and two

		m_PlayerOne = getPlayerTransform (GameData.Instance.PlayerOneCharacter);
		m_PlayerTwo = getPlayerTransform (GameData.Instance.PlayerTwoCharacter);


		//TODO: Delete for finale product, Onload will handle. OnLoad does not run when playing scene in editor
		//Load all sounds
		
		//Common Sounds
		m_JumpSFX = (AudioClip)Resources.Load(Constants.Sounds.ALEX_JUMP);
		m_WalkSFX = (AudioClip)Resources.Load(Constants.Sounds.WALK);
		m_RunSFX = (AudioClip)Resources.Load(Constants.Sounds.RUN);
		m_WeaponWoosh = (AudioClip)Resources.Load(Constants.Sounds.WEAPON_WOOSH);
		m_Collectable = (AudioClip)Resources.Load(Constants.Sounds.COLLECTABLE);
		m_JumpPad = (AudioClip)Resources.Load(Constants.Sounds.JUMPAD);
		
		//Alex Sounds
		m_AlexHitOne = (AudioClip)Resources.Load(Constants.Sounds.ALEX_FIRST_WEAPON_HIT);
		m_AlexHitTwo = (AudioClip)Resources.Load(Constants.Sounds.ALEX_SECOND_WEAPON_HIT);
		m_AlexHitThree = (AudioClip)Resources.Load(Constants.Sounds.ALEX_THIRD_WEAPON_HIT);
		m_AlexHurt = (AudioClip)Resources.Load (Constants.Sounds.ALEX_HURT);
		m_AlexDeath = (AudioClip)Resources.Load (Constants.Sounds.ALEX_DEATH);
		m_AlexJump = (AudioClip)Resources.Load (Constants.Sounds.ALEX_JUMP);
		
		//Derek Sounds
		m_DerekHitOne = (AudioClip)Resources.Load (Constants.Sounds.DEREK_FIRST_WEAPON_HIT);
		m_DerekHitTwo = (AudioClip)Resources.Load (Constants.Sounds.DEREK_SECOND_WEAPON_HIT);
		m_DerekHitThree = (AudioClip)Resources.Load (Constants.Sounds.DEREK_THIRD_WEAPON_HIT);
		m_DerekHurt = (AudioClip)Resources.Load (Constants.Sounds.DEREK_HURT);
		m_DerekDeath = (AudioClip)Resources.Load (Constants.Sounds.DEREK_DEATH);
		m_DerekJump = (AudioClip)Resources.Load (Constants.Sounds.DEREK_JUMP);

	
		//Zoey Sounds
<<<<<<< .mine
		m_ZoeyHitOne = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_First_Hit");
		m_ZoeyHitTwo = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Second_Hit");
		m_ZoeyHitThree = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Third_Hit");
		m_ZoeyHurt = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Painful_Grunt");
		m_ZoeyDeath = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Death");
		m_ZoeyJump = (AudioClip)Resources.Load ("Sounds/Zoey/Zoey_Jump");
		m_ZoeyWingsOpen = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Open");
		m_ZoeyWingsClose = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Close");
		m_ZoeyWingsDeploy = (AudioClip)Resources.Load ("Sounds/Zoey/Wings_Deploy");
=======
		m_ZoeyHitOne = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_FIRST_WEAPON_HIT);
		m_ZoeyHitTwo = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_SECOND_WEAPON_HIT);
		m_ZoeyHitThree = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_THIRD_WEAPON_HIT);
		m_ZoeyHurt = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_HURT);
		m_ZoeyDeath = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_DEATH);
		m_ZoeyJump = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_JUMP);
		m_ZoeyWingsOpen = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_OPEN);
		m_ZoeyWingsClose = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_CLOSE);
		m_ZoeyWingsDeploy = (AudioClip)Resources.Load (Constants.Sounds.ZOEY_WINGS_DEPLOY);
>>>>>>> .r794

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
			return GameObject.FindGameObjectWithTag(Constants.ALEX_STRING).transform;
			break;


			case Characters.Derek:
			return GameObject.FindGameObjectWithTag(Constants.DEREK_STRING).transform;
			break;

			case Characters.Zoe:
			return GameObject.FindGameObjectWithTag(Constants.ZOE_STRING).transform;
			break;

			default:
#if DEBUG
			Debug.LogError("Enum is out of range");
#endif
			return null;
			break;
		}
	}



	
	/// <summary>
	/// This function plays a sound from the gameObject you want it to play from.
	/// </summary>
	/// <param name="objectPlayingTheSound">Object playing the sound. pass in this.gameObject to have it originate from the object the script is attached</param>
	/// <param name="sound">The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere, Sound names are all one word</param>
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
#if DEBUG
            Debug.LogError("no Sound matching that name");
#endif
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
	/// This function plays a sound from the location specified
	/// </summary>
	/// <param name="Location">Pass in a vector3 for the location of the sound to play from don't use if it needs to move with them.</param>
	/// <param name="The sound that is played, it is an enum, to select a sound, Type Sounds.insertSoundNameHere, Sound names are all one word">.</param>
	public void playSound(Vector3 Location, Sounds sound)
	{

		GameObject soundObject = (GameObject)Instantiate (m_SoundObject);

		//this class takes the enum passed in and passes it to anouther function to get all data it needs.
		AudioInfo tempSoundInfo = getClipFromList(sound);
		AudioSource tempAudioSource = soundObject.GetComponent<AudioSource> ();

				
		
		//Safety check to make sure we have a sound
		if (tempSoundInfo.m_AudioClip == null)
		{
#if DEBUG
			Debug.LogError("no Sound matching that name");
#endif
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


	//Common Sounds
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
	
		
	//Alex Sounds
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

		case Sounds.AlexDeath:
			tempAudioInfo.m_AudioClip = m_AlexDeath;
			tempAudioInfo.OneShot = false;
			return tempAudioInfo;
			break;

		case Sounds.AlexJump:
			tempAudioInfo.m_AudioClip = m_AlexJump;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;


	//Derek Sounds
		case Sounds.DerekHitOne:
			tempAudioInfo.m_AudioClip = m_DerekHitOne;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.DerekHitTwo:
			tempAudioInfo.m_AudioClip = m_DerekHitTwo;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
		
		case Sounds.DerekHitThree:
			tempAudioInfo.m_AudioClip = m_DerekHitThree;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.DerekHurt:
			tempAudioInfo.m_AudioClip = m_DerekHurt;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.DerekDeath:
			tempAudioInfo.m_AudioClip = m_DerekDeath;
			tempAudioInfo.OneShot = false;
			return tempAudioInfo;
			break;

		case Sounds.DerekJump:
			tempAudioInfo.m_AudioClip = m_DerekJump;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;


	//Zoey Sounds
		case Sounds.ZoeyHitOne:
			tempAudioInfo.m_AudioClip = m_ZoeyHitOne;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
			
		case Sounds.ZoeyHitTwo:
			tempAudioInfo.m_AudioClip = m_ZoeyHitTwo;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
			
		case Sounds.ZoeyHitThree:
			tempAudioInfo.m_AudioClip = m_ZoeyHitThree;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;
			
		case Sounds.ZoeyHurt:
			tempAudioInfo.m_AudioClip = m_ZoeyHurt;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;


		case Sounds.ZoeyDeath:
			tempAudioInfo.m_AudioClip = m_ZoeyDeath;
			tempAudioInfo.OneShot = false;
			return tempAudioInfo;
			break;

		case Sounds.ZoeyJump:
			tempAudioInfo.m_AudioClip = m_ZoeyJump;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.ZoeyOpenWings:
			tempAudioInfo.m_AudioClip = m_ZoeyWingsOpen;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.ZoeyCloseWings:
			tempAudioInfo.m_AudioClip = m_ZoeyWingsClose;
			tempAudioInfo.OneShot = true;
			return tempAudioInfo;
			break;

		case Sounds.ZoeyDeployedWings:
			tempAudioInfo.m_AudioClip = m_ZoeyWingsDeploy;
			tempAudioInfo.OneShot = false;
			return tempAudioInfo;
			break;




		default:
#if DEBUG
			Debug.LogError("No regonized sound passed in");
#endif
			return tempAudioInfo;
            
            break;
        }
    }

}
