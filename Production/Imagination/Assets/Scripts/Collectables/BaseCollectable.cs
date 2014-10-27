﻿using UnityEngine;
using System.Collections;

enum CollectableType
{
	LightPeg,
	PuzzlePiece
}

public abstract class BaseCollectable : MonoBehaviour {

	int m_ID;
	CollectableType m_Type;	
				
	void OnTriggerEnter(Collider other)
	{
		if(other.tag = Constants.PLAYER_STRING)
		{
			PassInfoToGameData();
			Destroy(this.gameObject);
		}
	}
		
	public void SetInfo(int id, CollectableType type)
	{
		m_ID = id;
		m_Type = type;
	}

	/// <summary>
	/// sends data to gameData so game data knows its been collected.
	/// </summary>
	void PassInfoToGameData()
	{

	}
}
