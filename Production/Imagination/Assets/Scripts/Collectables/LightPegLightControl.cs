using UnityEngine;
using System.Collections;

/*
 * This class control's if a light peg will affect lights in the game 
 */


public class LightPegLightControl : MonoBehaviour
{
	public bool m_AffectsLight; // will this light peg affect lights?

	public SceneLights[] m_LightsToAffect; //An array of each of the lights we are going to affect
	public float m_IntensityToAdd; // the intensity that each light will increase when the light peg is picked up.
}
