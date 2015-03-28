using UnityEngine;
using System.Collections;

public class BossAnimationManager : MonoBehaviour , CallBack
{
	public GameObject i_AlexPrefab;
	public GameObject i_ZoePrefab;
	public GameObject i_DerekPrefab;

	public Transform[] StartPos;

	enum PlayerIndecies
	{
		Alex = 1,
		Derek = 2,
		Zoe = 3,
		Boss = 3
	};

	public Transform[] i_Player_Dummys;
	Transform[] m_ChildToBe = new Transform[2];

	const float DELAY = 0.5f;

	string[] m_AnimationStarts = new string[] {"Start_One", "Start_Two", "Start"};

	public Animator i_BossAnimator;

	public AudioSource m_AudioSource;
	public ScreenFade m_ScreenFade;

	// Use this for initialization
	void Start () 
	{
		i_BossAnimator.Play (m_AnimationStarts[(int)PlayerIndecies.Boss - 1]);
		gameObject.GetComponent<AnimationCallBackManager> ().registerCallBack (this);

		if((GameData.Instance.PlayerOneCharacter == Characters.Zoe || GameData.Instance.PlayerTwoCharacter == Characters.Zoe) &&
		   (GameData.Instance.PlayerOneCharacter == Characters.Derek || GameData.Instance.PlayerTwoCharacter == Characters.Derek))
		{//Zoe and Derek
			GameObject Zoe = (GameObject)GameObject.Instantiate(i_ZoePrefab, 
			                                                    StartPos[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek) - 1].position,
			                                                    StartPos[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek) - 1].rotation);
			Animator Zoe_Anim = Zoe.GetComponentInChildren<Animator>();

			GameObject Derek = (GameObject)GameObject.Instantiate(i_DerekPrefab, 
			                                                      StartPos[(int)PlayerIndecies.Derek - 1].position,
			                                                      StartPos[(int)PlayerIndecies.Derek - 1].rotation);
			Animator Derek_Anim = Derek.GetComponentInChildren<Animator>();

			m_ChildToBe[0] = Zoe.transform;
			m_ChildToBe[1] = Derek.transform;
			//Derek.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Derek - 1];
			//Derek.transform.position = Derek.transform.parent.position;
			//Derek.transform.rotation = Derek.transform.parent.rotation;

			//Zoe.transform.parent = i_Player_Dummys[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek) - 1];
			//Zoe.transform.position = Zoe.transform.parent.position;
			//Zoe.transform.rotation = Zoe.transform.parent.rotation;

			Derek_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Derek - 1]);
			Zoe_Anim.Play(m_AnimationStarts[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Derek) - 1]);
		}
		else if ((GameData.Instance.PlayerOneCharacter == Characters.Alex || GameData.Instance.PlayerTwoCharacter == Characters.Alex) &&
		         (GameData.Instance.PlayerOneCharacter == Characters.Derek || GameData.Instance.PlayerTwoCharacter == Characters.Derek))
		{//Derek and Alex
			GameObject Derek = (GameObject)GameObject.Instantiate(i_DerekPrefab, 
			                                                      StartPos[(int)PlayerIndecies.Derek - 1].position,
			                                                      StartPos[(int)PlayerIndecies.Derek - 1].rotation);
			Animator Derek_Anim = Derek.GetComponentInChildren<Animator>();

			GameObject Alex = (GameObject)GameObject.Instantiate(i_AlexPrefab, 
			                                                     StartPos[(int)PlayerIndecies.Alex - 1].position,
			                                                     StartPos[(int)PlayerIndecies.Alex - 1].rotation);
			Animator Alex_Anim = Alex.GetComponentInChildren<Animator>();


			m_ChildToBe[0] = Alex.transform;
			m_ChildToBe[1] = Derek.transform;
			//Derek.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Derek - 1];
			//Derek.transform.position = Derek.transform.parent.position;
			//Derek.transform.rotation = Derek.transform.parent.rotation;

			//Alex.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Alex - 1];
			//Alex.transform.position = Alex.transform.parent.position;
			//Alex.transform.rotation = Alex.transform.parent.rotation;

			Derek_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Derek - 1]);
			Alex_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Alex - 1]);
		}
		else
		{//Zoe and alex
			GameObject Zoe = (GameObject)GameObject.Instantiate(i_ZoePrefab, 
			                                                    StartPos[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex) - 1].position,
			                                                    StartPos[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex) - 1].rotation);
			Animator Zoe_Anim = Zoe.GetComponentInChildren<Animator>();

			GameObject Alex = (GameObject)GameObject.Instantiate(i_AlexPrefab, 
			                                                     StartPos[(int)PlayerIndecies.Alex - 1].position,
			                                                     StartPos[(int)PlayerIndecies.Alex - 1].rotation);
			Animator Alex_Anim = Alex.GetComponentInChildren<Animator>();

			m_ChildToBe[0] = Alex.transform;
			m_ChildToBe[1] = Zoe.transform;

			//Alex.transform.parent = i_Player_Dummys[(int)PlayerIndecies.Alex - 1];
			//Alex.transform.position = Alex.transform.parent.position;
			//Alex.transform.rotation = Alex.transform.parent.rotation;

			//Zoe.transform.parent = i_Player_Dummys[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex) - 1];
			//Zoe.transform.position = Zoe.transform.parent.position;
			//Zoe.transform.rotation = Zoe.transform.parent.rotation;

			Alex_Anim.Play(m_AnimationStarts[(int)PlayerIndecies.Alex - 1]);
			Zoe_Anim.Play(m_AnimationStarts[((int)PlayerIndecies.Zoe ^ (int)PlayerIndecies.Alex) - 1]);
		}
		//TODO:Playe Sound
		m_AudioSource.Play();
		//Debug.Break ();

		StartCoroutine ("delayedParenting");
	}

	IEnumerator delayedParenting()
	{
		yield return new WaitForSeconds (DELAY);

		m_ChildToBe [0].parent = i_Player_Dummys [0];
		m_ChildToBe [1].parent = i_Player_Dummys [1];
	}

	public void CallBack(CallBackEvents callBack)
	{
		if(callBack == CallBackEvents.Cutscene_Done)
		{
			m_ScreenFade.BeginFadeOut();
		}
	}
}
