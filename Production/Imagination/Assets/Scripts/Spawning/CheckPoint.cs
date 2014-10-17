/*
*CheckPoint
*
*resposible for informing GameData that a checkpoint might need changeing
*
*Created by: Kris Matis
*/

#region ChangeLog
/*
* 8/10/2014 Edit: Fully Commented - Kris Matis.
*
* 16/10/2014 Edit: Added in materials for the checkpoints
*/
#endregion

using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{

	public Material m_OffMaterial;
	public Material m_OnMaterial;
	public GameObject m_ColorSection;

	public CheckPoints m_Value;

	SFXManager m_SFX;

	// Use this for initialization
	void Start ()
	{
		m_ColorSection.renderer.material = m_OffMaterial;
		m_SFX = GameObject.FindGameObjectWithTag(Constants.SOUND_MANAGER).GetComponent<SFXManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnTriggerEnter(Collider obj)
	{
		if(obj.tag == Constants.PLAYER_STRING)
		{
			m_SFX.playSound(this.gameObject, Sounds.Collectable);
			GameData.Instance.CurrentCheckPoint = m_Value;
			m_ColorSection.renderer.material = m_OnMaterial;
		}
	}
}
