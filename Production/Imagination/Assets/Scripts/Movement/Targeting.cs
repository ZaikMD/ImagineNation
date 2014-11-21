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

    public Camera m_Camera;
    private Color m_CurrentTargetOriginalColor;
    private GameObject m_CurrentTarget;
    private List<GameObject> m_PossibleTargets;

    private int m_LayerMask;

	// Use this for initialization
	void Start ()
    {
		//set up our list of possible targets
        m_PossibleTargets = new List<GameObject>();
		for (int i = 0; i < m_TargetableTags.Length; i++)
		{
			//create an array of all the objects of the tag at our current index
			GameObject[] objectsToAdd = GameObject.FindGameObjectsWithTag(m_TargetableTags[i]);
        
			//Loop through and add to our list
			for(int n = 0; n < objectsToAdd.Length; n++)
			{
				m_PossibleTargets.Add(objectsToAdd[n]);
			}
		}
		//getting the mask of the player
        m_LayerMask = LayerMask.GetMask(Constants.PLAYER_STRING);
		//setting our layermask to the inverse of players
        m_LayerMask = ~m_LayerMask;
    }
	
	// Update is called once per frame
	void Update () 
    {
	//	CheckDistanceOfTargets();
	//	Debug.Log (m_PossibleTargets.Count);
		CalcCurrentTarget();
        PaintTarget();    
    }

	//find the current target
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

			//setting our raycast direction settings
            rayToObject.direction = DirectionOfTarget;
            rayToObject.origin = m_Camera.transform.position;

			//Raycasting to see if there is anything between us and the object we want to head, 
			//we are also passing in our viewable distance to see if they are within range
            if(Physics.Raycast(rayToObject, out HitInfo, m_ViewableDistance, m_LayerMask))
            {
				//Check if object hit is ours
                if (HitInfo.collider.gameObject != m_PossibleTargets[i])
                {
                    continue;
                }
				//Object was hit, has better angle, and is within range
                m_CurrentTarget = m_PossibleTargets[i];
                AngleOfCurrentTarget = Angle;
            }
        }
    }

	//Returns our current target
    public GameObject GetCurrentTarget()
    {
        return m_CurrentTarget;
    }

    //change color of target
    void PaintTarget()
    {
		//safety check if we have a target
        if (m_CurrentTarget == null)
        {
            return;
        }
		//get the objects color so we can reset it
        m_CurrentTargetOriginalColor = m_CurrentTarget.renderer.material.color;
        //change the color so we know what is our target
		m_CurrentTarget.renderer.material.color = m_TargetColor;
    }

	//get the cameras forward vector
    Vector3 GetCameraForward()
    {
        return m_Camera.transform.forward;
    }
}
