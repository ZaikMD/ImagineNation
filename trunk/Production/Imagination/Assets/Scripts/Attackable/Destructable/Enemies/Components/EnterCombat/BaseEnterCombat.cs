using UnityEngine;
using System.Collections;

public abstract class BaseEnterCombat : BaseComponent
{
    public override void start(BaseBehaviour baseBehaviour)
    {

    }
	public abstract bool EnterCombat();
}
