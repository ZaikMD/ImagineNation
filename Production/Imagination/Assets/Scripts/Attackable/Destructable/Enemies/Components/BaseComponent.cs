using UnityEngine;
using System.Collections;

public abstract class BaseComponent : MonoBehaviour 
{
	//YOU MUST CALL THIS START FOR INSIDE EVERY BEHAVIOUR THAT HAS A COMPONENT
    public abstract void start(BaseBehaviour baseBehaviour);
}
