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

    PlayerSectionV2[] m_PlayerSelections;

    int[] m_PlayerIndexs = { 0, 0 };

	// Use this for initialization
	void Start () 
    {
        m_PlayerSelections = new PlayerSectionV2[CharacterPrebafs.Length];
        for (int i = 0; i < m_PlayerSelections.Length; i++)
        {
            m_PlayerSelections[i] = new PlayerSectionV2((GameObject)GameObject.Instantiate(CharacterPrebafs[i], CharacterPrefabMountPoints[i].transform.position, CharacterPrefabMountPoints[i].transform.rotation), 
                                                        CharacterPrefabMountPoints[i].transform, 
                                                        Characters[i]);
        }
	}

    // Update is called once per frame
    protected override void  update()
    {
        int total = 0;
        for(int i = 0; i < m_PlayerSelections.Length; i++)
        {
            if(m_PlayerSelections[i].IsReady)
            {
                total++;
            }
        }

 	    if(total >= 2)
        {
            //TODO: set things
            //TODO: goto game scene
            return;
        }

        Vector2 playerOneMoveInput = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerOneInput);
        Vector2 playerTwoMoveInput = InputManager.getMenuChangeSelection(GameData.Instance.m_PlayerTwoInput);

        #region change selection
        if (playerOneMoveInput.y > 0.0f)
        {
            m_PlayerIndexs[PLAYER_ONE]++;
        }
        else if (playerOneMoveInput.y < 0.0f)
        {
            m_PlayerIndexs[PLAYER_ONE]--;
        }

        m_PlayerIndexs[PLAYER_ONE] = Mathf.Clamp(m_PlayerIndexs[PLAYER_ONE], 0, CharacterPrefabMountPoints.Length - 1);
        PlayerArrows[PLAYER_ONE].transform.position = PlayerOneArrowMountPoints[m_PlayerIndexs[PLAYER_ONE]].transform.position;


        if (playerTwoMoveInput.y > 0.0f)
        {
            m_PlayerIndexs[PLAYER_TWO]++;
        }
        else if (playerTwoMoveInput.y < 0.0f)
        {
            m_PlayerIndexs[PLAYER_TWO]--;
        }

        m_PlayerIndexs[PLAYER_TWO] = Mathf.Clamp(m_PlayerIndexs[PLAYER_TWO], 0, CharacterPrefabMountPoints.Length - 1);
        PlayerArrows[PLAYER_TWO].transform.position = PlayerTwoArrowMountPoints[m_PlayerIndexs[PLAYER_TWO]].transform.position;

        #endregion


    }
}
