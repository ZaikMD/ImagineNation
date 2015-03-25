using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour 
{

	public int Section;
	private SFXManager m_SFX;

	// Use this for initialization
	void Start () 
	{
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
		m_SFX.PlaySong();
	}

	void Update()
	{
/*
		switch(Section)
		{
		case 0:
			m_SFX.playSound(this.transform, Sounds.SongMainMenu);
			break;
			
		case 1:
			m_SFX.playSound(this.transform, Sounds.SongSection1);
			break;
			
		case 2:
			m_SFX.playSound(this.transform, Sounds.SongSection2);
			break;
			
		case 3:
			m_SFX.playSound(this.transform, Sounds.SongSection3);
			break;
			
		}
*/
	}
}
	
