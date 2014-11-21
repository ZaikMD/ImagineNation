﻿using UnityEngine;
using System.Collections;

public abstract class BaseTargeting : BaseComponent
{
    public override void start(BaseBehaviour baseBehaviour)
    {

    }

    public abstract GameObject CurrentTarget();
}
