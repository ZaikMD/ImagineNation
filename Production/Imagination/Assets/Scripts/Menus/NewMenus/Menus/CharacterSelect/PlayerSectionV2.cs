/*
 * Created by: Kris MAtis
 * data organizer class for multi player character selection
 * 
 */

#region ChangeLog
/*
 * 27/10/2014 edit: script made
 * 
 * 28/10/2014 edit: what i did while sick was terrible so im resarting 
 * 28/10/2014 edit: mostly done
 *  28/10/2014 edit finished debuging base functionality is in exept for saving the settings
 *  
 * * 29/10/2014 edit: added resets
 */
#endregion

using UnityEngine;
using System.Collections;

public class PlayerSectionV2 : MonoBehaviour
{
    public GameObject Model;
    

    public Characters Character;

    Vector3 m_MountPoint;
    Vector3 m_OriginalMountpoint;


	public GameObject[] CharacterSummary = new GameObject[2];

	Vector3[] m_SummaryMountPoint = new Vector3[2];
	Vector3[] m_OriginalSummaryMountpoint = new Vector3[2];


    bool m_IsMounted = false;
    public bool IsMounted
    {
        get { return m_IsMounted; }
        set { m_IsMounted = value; }
    }

    const float SCALE_AMOUNT = 3.0f;
	Vector3 m_InitialScale;

    const float SCALE_LERP_SPEED = 0.12f;
    const float MOVE_LERP_SPEED = 0.12f;

    void Start()
    {
        m_OriginalMountpoint = transform.position;

		m_InitialScale = transform.localScale;


		m_OriginalSummaryMountpoint[0] = CharacterSummary[0].transform.position;
		m_SummaryMountPoint[0] = m_OriginalSummaryMountpoint[0];

		m_OriginalSummaryMountpoint[1] = CharacterSummary[1].transform.position;
		m_SummaryMountPoint[1] = m_OriginalSummaryMountpoint[1];
    }

    void Update()
    {
		CharacterSummary[0].transform.position = Vector3.Lerp(CharacterSummary[0].transform.position, m_SummaryMountPoint[0], MOVE_LERP_SPEED);
		CharacterSummary[1].transform.position = Vector3.Lerp(CharacterSummary[1].transform.position, m_SummaryMountPoint[1], MOVE_LERP_SPEED);

        if (m_IsMounted)
        {
            transform.position = Vector3.Lerp(transform.position, m_MountPoint, MOVE_LERP_SPEED);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * SCALE_AMOUNT, SCALE_LERP_SPEED);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_OriginalMountpoint, MOVE_LERP_SPEED);
			transform.localScale = Vector3.Lerp(transform.localScale, m_InitialScale, SCALE_LERP_SPEED);
        }
    }

    public void setMountPoint(Vector3 mountPoint)
    {
        m_IsMounted = true;
        m_MountPoint = mountPoint;
    }

	public void setSummaryMountPoint(Vector3 mountPoint, int index)
	{
		m_SummaryMountPoint[index] = mountPoint;
	}

	public void reset(int index)
	{
		m_IsMounted = false;
		m_MountPoint = m_OriginalMountpoint;
		resetMointPoint (index);
	}

    public void resetMointPoint(int index)
    {
		m_SummaryMountPoint [index] = m_OriginalSummaryMountpoint [index];
    }
}
