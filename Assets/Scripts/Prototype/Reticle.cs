using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {

	protected float RETICLE_DISTANCE = 10.0f;

	protected Vector3 m_ReticlePosition = Vector3.zero;
	protected GUITexture m_ReticleTexture;

	public Vector3 getReticlePosition()
	{
		return m_ReticlePosition;
	}

	protected virtual void updateReticle ()
	{
	}
}
