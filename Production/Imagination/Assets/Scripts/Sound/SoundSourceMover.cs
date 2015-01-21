using UnityEngine;
using System.Collections;

/*
 * Created By: Kris
 *  
 * this script is intended to be used to move sounds around the listener in the scene to create a pseudo 3d effect for 2 players
 * it moves the source based off the positions of the closest player and the object we're pretending is playing the sound relative to the listener
 * 
 * jan 20 2015 kris
 * implemented
 */

public class SoundSourceMover : MonoBehaviour 
{
    //this objects audio source
    AudioSource m_AudioSource;
    public AudioSource AudioSource
    {
        get { return m_AudioSource; }
        set { m_AudioSource = value; }
    }

    //the listener (should be the transform of the sfxManager)
    Transform m_AudioListenerTransform;
    public Transform AudioListenerTransform
    {
        get{ return m_AudioListenerTransform; }
        set { m_AudioListenerTransform = value; }
    }

    //the object that we're pretending is playing the sound
    Transform m_SourceObject;
    public Transform SourceObject
    {
        get { return m_SourceObject; }
        set { m_SourceObject = value; }
    }

    //the players
    PlayerInfo[] m_Players = null;
    PlayerInfo[] Players
    {
        get
        {
            if(m_Players == null)
            {
                m_Players = new PlayerInfo[2];
                m_Players[0] = PlayerInfo.getPlayer(GameData.Instance.PlayerOneCharacter);
                m_Players[1] = PlayerInfo.getPlayer(GameData.Instance.PlayerTwoCharacter);
            }
            return m_Players;
        }
    }

	public void initialize(Transform sourceObject, Transform audioListener, AudioClip clip, bool isOneShot = true, bool isLooping = false)
    {
        m_SourceObject = sourceObject;
        m_AudioListenerTransform = audioListener;

        AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.clip = clip;
        m_AudioSource.loop = isLooping;
        if(isOneShot)
        {
            m_AudioSource.PlayOneShot(clip);
        }
        else
        {
            m_AudioSource.Play();
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (m_AudioSource.isPlaying && m_SourceObject != null && m_AudioListenerTransform != null)
        {
            //figure out wich is the closest player
            PlayerInfo closestPlayer = Players[0];
            if (Vector3.Distance(Players[0].transform.position, SourceObject.position) > Vector3.Distance(Players[1].transform.position, SourceObject.position))
            {
                closestPlayer = Players[1];
            }

            //moves the position of the source to be the correct distance
            transform.position = (m_AudioListenerTransform.position + SourceObject.position - closestPlayer.transform.position);

            //makes sure the sound is playing on the correct side since the players rotate but the listener does not
            if(Vector3.Dot(m_AudioListenerTransform.forward, closestPlayer.transform.forward) < 0)
            {
                transform.position = -(transform.position - m_AudioListenerTransform.position);
            }
        }
        else
        {
            //sound is no longer playering so destroy this
            GameObject.Destroy(this.gameObject);
        }
	}
}
