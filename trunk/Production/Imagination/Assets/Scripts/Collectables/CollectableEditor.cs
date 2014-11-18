using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CollectableEditor : EditorWindow
{
    //The previous checkpoint
    public int CheckPoint;

    struct CollectableInfo
	{
		Vector3 Pos;
	}

    //Varibales
	GameObject m_LightPegPrefab;
	GameObject m_PuzzlePiecePrefab;

	List<CollectableInfo> m_LightPegs;
	List<CollectableInfo> m_PuzzlePieces;

	[MenuItem("Collectables")]

	void CreateNewLightPeg()
	{

	}

	void CreateNewPuzzlePiece()
	{

	}

	void DeleteCollectable()
	{

	}
}
