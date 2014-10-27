/*
 * Created by: Kris MAtis
 * menu for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made
 */
#endregion

using UnityEngine;
using System.Collections;

public class MenuV2PlayerSelect : MenuV2 
{
    const int PLAYER_ONE = 0;
    const int PLAYER_TWO = 1;

    public GameObject[] CharacterPrefabMountPoints;
    public GameObject[] CharacterPrebafs;
    public GameObject[] CharacterSummaryPrefab;
    public Characters[] Characters;

    public GameObject[] PlayerOneArrowMountPoints;
    public GameObject[] PlayerTwoArrowMountPoints;

    public GameObject[] PlayerArrows;

    public GameObject[] PlayerSelectionMountPoint;
    public GameObject[] PlayerPedestalMountPoint;

    public GameObject PedestalKeyboardPrefab;
    public GameObject PedestalGamepadPreafab;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
