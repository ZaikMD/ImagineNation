using UnityEngine;
using System.Collections;

public class BossAnimationManager : MonoBehaviour , CallBack
{
	GameObject i_AlexPrefab;
	GameObject i_ZoePrefab;
	GameObject i_DerekPrefab;
	
	enum PlayerIndecies
	{
		Alex = 1,
		Derek = 2,
		Zoe = 3,
		Boss = 3
	};

	public Transform[] i_Player_Dummys;
	string[] m_AnimationStarts = new string[] {"Start_One", "Start_Two", "Start"};

	public Animator i_BossAnimator;

	// Use this for initialization
	void Start () 
	{
		gameObject.GetComponent<AnimationCallBackManager> ().registerCallBack (this);

		if((GameData.Instance.PlayerOneCharacter == Characters.Zoe || GameData.Instance.PlayerTwoCharacter == Characters.Zoe) &&
		   (GameData.Instance.PlayerOneCharacter == Characters.Derek || GameData.Instance.PlayerTwoCharacter == Characters.Derek))
		{//Zoe and Derek
			GameObject Zoe = (GameObject)GameObject.Instantiate(i_ZoePrefab);
			Animator Zoe_Anim = Zoe.GetComponentInChildren<Animator>();

			GameObject Derek = (GameObject)GameObject.Instantiate(i_DerekPrefab);
			Animator Derek_Anim = Derek.GetComponentInChildren<Animator>();

			Derek.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Derek];
			Zoe.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek];

			Derek_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Derek]);
			Zoe_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek]);
		}
		else if ((GameData.Instance.PlayerOneCharacter == Characters.Alex || GameData.Instance.PlayerTwoCharacter == Characters.Alex) &&
		         (GameData.Instance.PlayerOneCharacter == Characters.Derek || GameData.Instance.PlayerTwoCharacter == Characters.Derek))
		{//Derek and Alex
			GameObject Derek = (GameObject)GameObject.Instantiate(i_DerekPrefab);
			Animator Derek_Anim = Derek.GetComponentInChildren<Animator>();

			GameObject Alex = (GameObject)GameObject.Instantiate(i_AlexPrefab);
			Animator Alex_Anim = Alex.GetComponentInChildren<Animator>();

			Derek.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Derek];
			Alex.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Alex];

			Derek_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Derek]);
			Alex_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Alex]);
		}
		else
		{//Zoe and alex
			GameObject Zoe = (GameObject)GameObject.Instantiate(i_ZoePrefab);
			Animator Zoe_Anim = Zoe.GetComponentInChildren<Animator>();

			GameObject Alex = (GameObject)GameObject.Instantiate(i_AlexPrefab);
			Animator Alex_Anim = Alex.GetComponentInChildren<Animator>();

			Alex.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Alex];
			Zoe.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex];

			Alex_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Alex]);
			Zoe_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex]);
		}
		i_BossAnimator.Play (m_AnimationStarts[(int)PlayerIndecies.Boss]);
		//TODO:Playe Sound
	}

	public void CallBack(CallBackEvents callBack)
	{
		if(callBack == CallBackEvents.Cutscene_Done)
		{
			Application.LoadLevel(Constants.MAIN_MENU_NAME);
		}
	}
}
