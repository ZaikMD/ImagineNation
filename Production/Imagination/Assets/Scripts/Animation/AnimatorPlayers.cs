using UnityEngine;
using System.Collections;

public class AnimatorPlayers : AnimatorController
{
    public enum Animations
    {
        Idle,
        Walk,
        Run,
        Jump,
        Falling,
        Landing,
        Ability,
        Combo_X,
        Combo_XX,
        Combo_XXX,
        Combo_Y,
        Combo_XY,
        Combo_XXY,
        Combo_XXXY,
        Death
    }

    protected override void Start()
    {
        base.Start();

        m_States = new string[]
        {
            "Idle",
            "Walk",
            "Run",
            "Jump",
            "Falling",
            "Landing",
            "Ability",
            "Combo_X",
            "Combo_XX",
            "Combo_XXX",
            "Combo_Y",
            "Combo_XY",
            "Combo_XXY",
            "Combo_XXXY",
            "Death"
        };
    }

    public virtual void playAnimation(Animations animation)
    {
        playAnimation(m_States[(int)animation]);
    }
}
