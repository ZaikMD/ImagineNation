/*
 * Created by: Kris MAtis
 * data organizer class for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 28/10/2014 edit: script made
 * 
 * 28/10/2014 edit: script is filled out
 *  28/10/2014 edit finished debuging base functionality is in exept for saving the settings
 *  
 * * 29/10/2014 edit: added resets
 */
#endregion

using UnityEngine;
using System.Collections;

public class PlayerSelectArrow : MonoBehaviour 
{
    public Transform[] ArrowMountPoints;
    public Transform SelectionMountPoint;

    public PlayerSectionV2[] CharacterSelections;

	public Transform m_SummaryMountPoint;

    bool m_IsMounted = false;

	public int m_Player = 0;

    public bool IsMounted
    {
        get { return m_IsMounted; }
        set { m_IsMounted = value; }
    }

    static int m_IndexesUsed = 0;

    int m_Index = 0;

	// Use this for initialization
	void Start () 
    {
		changePosition ();
	}

    public void moveUp()
    {
		CharacterSelections[m_Index].resetMointPoint(m_Player);
        if (!m_IsMounted)
        {
            do
            {
                m_Index--;
                if (m_Index < 0)
                {
                    m_Index = ArrowMountPoints.Length - 1;
                }
            } while (CharacterSelections[m_Index].IsMounted);

            changePosition();
        }
    }

    public void moveDown()
    {
		CharacterSelections[m_Index].resetMointPoint(m_Player);
        if (!m_IsMounted)
        {
            do
            {
                m_Index++;
                if (m_Index >= ArrowMountPoints.Length)
                {
                    m_Index = 0;
                }
            } while (CharacterSelections[m_Index].IsMounted);
            changePosition();
        }
    }

    public void select()
    {
        if (!m_IsMounted)
        {
            if (!CharacterSelections[m_Index].IsMounted)
            {
                m_IsMounted = true;
                CharacterSelections[m_Index].IsMounted = true;
                changePosition();
            }
        }
    }

    public void deselect()
    {
        if (m_IsMounted)
        {
            m_IsMounted = false;
            CharacterSelections[m_Index].IsMounted = false;
            changePosition();
        }
    }

    void changePosition()
    {
        if (!m_IsMounted)
        {
            transform.position = ArrowMountPoints[m_Index].position;
			CharacterSelections[m_Index].setSummaryMountPoint(m_SummaryMountPoint.position, m_Player);
        }
        else
        {
			transform.position = transform.position + new Vector3(0.0f, 999.0f, 0.0f);
			CharacterSelections[m_Index].setMountPoint(SelectionMountPoint.position);
			CharacterSelections[m_Index].resetMointPoint(m_Player);
        }
    }

    public PlayerSectionV2 getSelection()
    {
        return CharacterSelections[m_Index];
    }

    public Characters getCharacterSetting()
    {
        return CharacterSelections[m_Index].Character;
    }

    public void reset()
    {
        m_IndexesUsed = 0;
        m_Index = m_IndexesUsed++;
        m_IsMounted = false;

        for (int i = 0; i < CharacterSelections.Length; i++)
        {
            CharacterSelections[i].reset(m_Player);
        }
    }
}
