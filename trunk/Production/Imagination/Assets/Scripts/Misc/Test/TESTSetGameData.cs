using UnityEngine;
using System.Collections;

public class TESTSetGameData : MonoBehaviour 
{
	public PlayerInput PlayerOneInput = PlayerInput.None;
	public PlayerInput PlayerTwoInput = PlayerInput.None;

	public Characters PlayerOneCharacter = Characters.Alex;
	public Characters PlayerTwoCharacter = Characters.Zoe;

	public Levels m_CurrentLevel = Levels.Level_1;
	public Sections m_CurrentSection = Sections.Sections_1;
	public CheckPoints m_CurrentCheckPoint = CheckPoints.CheckPoint_1;

	// Use this for initialization
	void Awake () 
	{
		GameData.Instance.m_PlayerOneInput = PlayerOneInput;
		GameData.Instance.m_PlayerTwoInput = PlayerTwoInput;

		GameData.Instance.PlayerOneCharacter = PlayerOneCharacter;
		GameData.Instance.PlayerTwoCharacter = PlayerTwoCharacter;

		GameData.Instance.CurrentLevel = m_CurrentLevel;
		GameData.Instance.CurrentSection = m_CurrentSection;
		GameData.Instance.CurrentCheckPoint = m_CurrentCheckPoint;
	}
}
