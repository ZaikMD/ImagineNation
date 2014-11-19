/*
 * Created by: Kole Tackney
 * Date: Nov, 17, 2014
 * 
 * This script was create to control aiming for dereks grappling hook,
 * it takes the cameras forward to check the which way we are facing, and
 * checks for the object that is within range, aswell as closest angle to 
 * the direction we are facing
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour {

    public string[] m_TargetableTags;
    public float m_FieldOfView;
    public float m_ViewableDistance;
    public Color m_TargetColor;

    public SphereCollider m_Collider;
    public Camera m_Camera;
    private Color m_CurrentTargetOriginalColor;
    private GameObject m_CurrentTarget;
    private List<GameObject> m_PossibleTargets;

    private int m_LayerMask;

	// Use this for initialization
	void Start ()
    {
        m_PossibleTargets = new List<GameObject>();
        m_Collider.radius = m_ViewableDistance;
        m_LayerMask = LayerMask.GetMask(Constants.PLAYER_STRING);
        m_LayerMask = ~m_LayerMask;
    }
	
	// Update is called once per frame
	void Update () 
    {
	//	CheckDistanceOfTargets();
		Debug.Log (m_PossibleTargets.Count);
		CalcCurrentTarget();
        PaintTarget();    
    }

    void CalcCurrentTarget()
    {
        //reset our current target
        if(m_CurrentTarget != null)
        m_CurrentTarget.renderer.material.color = m_CurrentTargetOriginalColor;

        //Set our target to null so if we can't see our target anymore, we know
        m_CurrentTarget = null;

        //Do Calc
        //get the value of the angle between 
        float AngleOfCurrentTarget = m_FieldOfView * 0.5f;

        Vector3 LookVector = GetCameraForward();

        //Loop through all of the possible targets and see if its the most viable
        for(int i = 0; i < m_PossibleTargets.Count; i++)
        {
			Vector3 DirectionOfTarget = m_PossibleTargets[i].transform.position - m_Camera.transform.position;
            //set angle to the angle between our facing angle and the other object
            float Angle = Vector3.Angle(LookVector, DirectionOfTarget);
            //Check if this object is viewable or if current target is a better target
            if(Angle > m_FieldOfView * 0.5f || Angle > AngleOfCurrentTarget)
            {
                continue;
            }
            
            //RayCast to object to see if this is object is viewable
            Ray rayToObject = new Ray();
            RaycastHit HitInfo;

            rayToObject.direction = LookVector;
            rayToObject.origin = m_Camera.transform.position;
			Debug.DrawRay(rayToObject.origin, rayToObject.direction);

            if (Physics.Raycast(rayToObject, out HitInfo, m_ViewableDistance, m_LayerMask))
            {
				Debug.DrawRay(rayToObject.origin, rayToObject.direction);

                //Check if object hit is ours
                if (HitInfo.collider.gameObject != m_PossibleTargets[i])
                {
                    continue;
                }

                Debug.Log("What we got");

                //Object was hit, has better angle, and is within range
                m_CurrentTarget = m_PossibleTargets[i];
                AngleOfCurrentTarget = Angle;
            }
        }

        //have we found anything?
        if(m_CurrentTarget == null)
        {
            { 
                //We have not found anything. 
            }
        }

    }

    public GameObject GetCurrentTarget()
    {
        return m_CurrentTarget;
    }

    //change color of target
    void PaintTarget()
    {
        if (m_CurrentTarget == null)
        {
            return;
        }


        m_CurrentTargetOriginalColor = m_CurrentTarget.renderer.material.color;
        m_CurrentTarget.renderer.material.color = m_TargetColor;
    }

    Vector3 GetCameraForward()
    {
        return m_Camera.transform.forward;
    }

	void CheckDistanceOfTargets()
	{
		for(int i = 0; i < m_PossibleTargets.Count; i++)
		{
			if(10 < Vector3.Distance(m_PossibleTargets[i].transform.position, this.transform.position))
			{
				m_PossibleTargets.RemoveAt(i);
			}
		}
	}

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < m_TargetableTags.Length; i++)
        {
            if (other.tag == m_TargetableTags[i])
            {
                m_PossibleTargets.Add(other.gameObject);
                other.renderer.material.color = m_TargetColor;
            }
        }
    }

	void OnTriggerExit(Collider other)
	{
		if(m_PossibleTargets.Contains(other.gameObject))
		{	
			m_PossibleTargets.Remove(other.gameObject);
		}
	}



}
